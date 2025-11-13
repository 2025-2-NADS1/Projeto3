using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pensafy_Final_Integrated
{
    public class FormMapa : Form
    {
        private int idUsuario;
        private Button btnFacil, btnMedia, btnDificil, btnRanking;
        private Label lblPontos;

        public FormMapa(int usuario)
        {
            idUsuario = usuario;
            InitializeComponent();
            Theme.AplicarGradiente(this, Color.FromArgb(18,164,120), Color.FromArgb(46,204,113));
            AtualizarMapa();
        }

        private void InitializeComponent()
        {
            this.Text = "Pensafy - Mapa de Ilhas";
            this.ClientSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            btnFacil = new Button() { Text = "Ilha Fácil", Left = 100, Top = 200, Width = 200, Height = 80, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
            btnFacil.Click += (s,e) => AbrirJogo("Facil");
            btnMedia = new Button() { Text = "Ilha Média", Left = 350, Top = 200, Width = 200, Height = 80, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
            btnMedia.Click += (s,e) => AbrirJogo("Media");
            btnDificil = new Button() { Text = "Ilha Difícil", Left = 600, Top = 200, Width = 200, Height = 80, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
            btnDificil.Click += (s,e) => AbrirJogo("Dificil");
            btnRanking = new Button() { Text = "Ranking", Left = 380, Top = 380, Width = 140, Height = 40 };
            btnRanking.Click += (s,e) => { using(var r=new FormRanking()){ this.Hide(); r.ShowDialog(); this.Show(); } };
            lblPontos = new Label() { Left = 20, Top = 20, Width = 300, Height = 30, ForeColor = Color.White, Font = new Font("Segoe UI",12F) };
            this.Controls.Add(btnFacil);
            this.Controls.Add(btnMedia);
            this.Controls.Add(btnDificil);
            this.Controls.Add(btnRanking);
            this.Controls.Add(lblPontos);
        }

        private void AtualizarMapa()
        {
            int pontos = Database.ObterPontuacaoTotal(idUsuario);
            lblPontos.Text = $"Pontuação: {pontos}";
            btnFacil.Enabled = true;
            btnMedia.Enabled = pontos >= 100;
            btnDificil.Enabled = pontos >= 200;
        }

        private void AbrirJogo(string dificuldade)
        {
            int pontos = Database.ObterPontuacaoTotal(idUsuario);
            if (dificuldade=="Media" && pontos < 100)
            {
                MessageBox.Show("Alcançe 100 pontos para desbloquear a Ilha Média!", "Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dificuldade=="Dificil" && pontos < 200)
            {
                MessageBox.Show("Alcançe 200 pontos para desbloquear a Ilha Difícil!", "Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using(var jogo = new FormJogo(idUsuario, dificuldade))
            {
                this.Hide();
                jogo.ShowDialog();
                this.Show();
            }
            AtualizarMapa();
        }
    }
}
