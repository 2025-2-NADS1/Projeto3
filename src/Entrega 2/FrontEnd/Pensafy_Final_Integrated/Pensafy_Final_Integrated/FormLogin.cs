using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pensafy_Final_Integrated
{
    public class FormLogin : Form
    {
        private TextBox txtEmail;
        private TextBox txtSenha;
        private Button btnEntrar;
        private Button btnCadastrar;
        public FormLogin()
        {
            InitializeComponent();
            Theme.AplicarGradiente(this, Color.FromArgb(18, 164, 120), Color.FromArgb(46, 204, 113));
        }
        private void InitializeComponent()
        {
            this.Text = "Pensafy - Login";
            this.ClientSize = new Size(480, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            Label lblEmail = new Label() { Text = "E-mail", Left = 40, Top = 80, Width = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 12F, FontStyle.Regular) };
            txtEmail = new TextBox() { Left = 40, Top = 110, Width = 380, Height = 36, Font = new Font("Segoe UI", 12F) };
            Label lblSenha = new Label() { Text = "Senha", Left = 40, Top = 160, Width = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 12F, FontStyle.Regular) };
            txtSenha = new TextBox() { Left = 40, Top = 190, Width = 380, Height = 36, Font = new Font("Segoe UI", 12F), UseSystemPasswordChar = true };
            btnEntrar = new Button() { Text = "Entrar", Left = 40, Top = 250, Width = 380, Height = 48, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
            btnEntrar.Click += BtnEntrar_Click;
            btnCadastrar = new Button() { Text = "Cadastrar", Left = 40, Top = 310, Width = 380, Height = 36 };
            btnCadastrar.Click += BtnCadastrar_Click;
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblSenha);
            this.Controls.Add(txtSenha);
            this.Controls.Add(btnEntrar);
            this.Controls.Add(btnCadastrar);
        }
        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Informe e-mail e senha.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Database.Login(txtEmail.Text.Trim(), txtSenha.Text.Trim());
            if (id == -1)
            {
                MessageBox.Show("Usuário ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (var mapa = new FormMapa(id))
            {
                this.Hide();
                mapa.ShowDialog();
                this.Show();
            }
        }
        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            using (var cad = new FormCadastro())
            {
                this.Hide();
                cad.ShowDialog();
                this.Show();
            }
        }
    }
}
