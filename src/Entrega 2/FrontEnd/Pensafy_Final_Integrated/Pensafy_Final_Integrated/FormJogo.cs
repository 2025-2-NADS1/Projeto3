using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Pensafy_Final_Integrated
{
    public class FormJogo : Form
    {
        private Label lblPalavra;
        private TextBox txtTentativa;
        private Button btnTentar;
        private Label lblTentativas;
        private Label lblPontos;

        private string palavraSecreta;
        private char[] exibicao;
        private int tentativasRestantes = 3;
        private int pontosGanhos = 0;

        private int idUsuario;
        private int idPartida;
        private int idPalavra;

        public FormJogo(int usuario, string dificuldade)
        {
            idUsuario = usuario;
            InitializeComponent();
            Theme.AplicarGradiente(this, Color.FromArgb(18, 164, 120), Color.FromArgb(46, 204, 113));

            var p = Database.ObterPalavraPorDificuldade(dificuldade);
            palavraSecreta = p.Texto.ToUpper();
            idPalavra = p.Id;
            idPartida = Database.CriarPartida(idUsuario, idPalavra, palavraSecreta);

            exibicao = palavraSecreta.Select(c => c==' ' ? ' ' : '_').ToArray();
            AtualizarUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Pensafy - Jogo";
            this.ClientSize = new Size(520, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblPalavra = new Label() { Left = 40, Top = 40, Width = 440, Height = 60, Font = new Font("Segoe UI", 26F, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter };
            txtTentativa = new TextBox() { Left = 140, Top = 120, Width = 240, Height = 34, Font = new Font("Segoe UI", 16F) };
            btnTentar = new Button() { Text = "TENTAR", Left = 140, Top = 170, Width = 240, Height = 50, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
            btnTentar.Click += BtnTentar_Click;
            lblTentativas = new Label() { Left = 40, Top = 240, Width = 200, Height = 30, Font = new Font("Segoe UI", 12F), ForeColor = Color.White };
            lblPontos = new Label() { Left = 260, Top = 240, Width = 220, Height = 30, Font = new Font("Segoe UI", 12F), ForeColor = Color.White, TextAlign = ContentAlignment.TopRight };

            Controls.Add(lblPalavra);
            Controls.Add(txtTentativa);
            Controls.Add(btnTentar);
            Controls.Add(lblTentativas);
            Controls.Add(lblPontos);
        }

        private void AtualizarUI()
        {
            lblPalavra.Text = string.Join(" ", exibicao);
            lblTentativas.Text = $"Tentativas: {tentativasRestantes}";
            lblPontos.Text = $"Pontos ganhos: {pontosGanhos}";
        }

        private void BtnTentar_Click(object sender, EventArgs e)
        {
            string tentativa = txtTentativa.Text.Trim().ToUpper();
            txtTentativa.Clear();
            if (string.IsNullOrEmpty(tentativa))
            {
                MessageBox.Show("Digite uma letra ou palavra.");
                return;
            }

            bool acertou = false;
            if (tentativa.Length == 1)
            {
                char ch = tentativa[0];
                bool found = false;
                for (int i = 0; i < palavraSecreta.Length; i++)
                {
                    if (palavraSecreta[i] == ch)
                    {
                        exibicao[i] = ch;
                        found = true;
                    }
                }
                acertou = found && !exibicao.Contains('_') ? true : found;
            }
            else
            {
                acertou = tentativa == palavraSecreta;
                if (acertou) exibicao = palavraSecreta.ToCharArray();
            }

            // registrar tentativa
            Database.RegistrarTentativa(idPartida, tentativa, acertou ? "ACERTOU" : "ERROU", palavraSecreta.IndexOf(tentativa));

            if (acertou && !exibicao.Contains('_'))
            {
                pontosGanhos += 50;
                Database.AtualizarPontuacao(idUsuario, pontosGanhos);
                MessageBox.Show($"Parabéns! Você concluiu a palavra e ganhou {pontosGanhos} pontos.", "Vitória");
                if (pontosGanhos >= 100)
                {
                    Database.GerarCupom(idUsuario, Guid.NewGuid().ToString().Substring(0,8).ToUpper(), "Pontos100");
                }
                this.Close();
                return;
            }

            if (!acertou)
            {
                tentativasRestantes--;
                if (tentativasRestantes <= 0)
                {
                    MessageBox.Show($"Fim de jogo! A palavra era: {palavraSecreta}", "Fim");
                    this.Close();
                    return;
                }
            }

            AtualizarUI();
        }
    }
}
