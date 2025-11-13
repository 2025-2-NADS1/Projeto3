using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pensafy_Final_Integrated
{
    public class FormCadastro : Form
    {
        private TextBox txtNome, txtEmail, txtSenha;
        private Button btnCadastrar;
        public FormCadastro()
        {
            InitializeComponent();
            Theme.AplicarGradiente(this, Color.FromArgb(18,164,120), Color.FromArgb(46,204,113));
        }
        private void InitializeComponent()
        {
            this.Text = "Pensafy - Cadastro";
            this.ClientSize = new Size(520, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            Label lblNome = new Label() { Text = "Nome", Left = 40, Top = 60, Width = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 12F) };
            txtNome = new TextBox() { Left = 40, Top = 90, Width = 420, Height = 34, Font = new Font("Segoe UI", 12F) };
            Label lblEmail = new Label() { Text = "E-mail", Left = 40, Top = 140, Width = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 12F) };
            txtEmail = new TextBox() { Left = 40, Top = 170, Width = 420, Height = 34, Font = new Font("Segoe UI", 12F) };
            Label lblSenha = new Label() { Text = "Senha", Left = 40, Top = 220, Width = 100, ForeColor = Color.White, Font = new Font("Segoe UI", 12F) };
            txtSenha = new TextBox() { Left = 40, Top = 250, Width = 420, Height = 34, Font = new Font("Segoe UI", 12F), UseSystemPasswordChar = true };
            btnCadastrar = new Button() { Text = "Cadastrar", Left = 40, Top = 310, Width = 420, Height = 44, Font = new Font("Segoe UI", 12F, FontStyle.Bold) };
            btnCadastrar.Click += BtnCadastrar_Click;
            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblSenha);
            this.Controls.Add(txtSenha);
            this.Controls.Add(btnCadastrar);
        }
        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha todos os campos.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Database.UsuarioExiste(txtEmail.Text.Trim()))
            {
                MessageBox.Show("E-mail já cadastrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Database.CadastrarUsuario(txtNome.Text.Trim(), txtEmail.Text.Trim(), txtSenha.Text.Trim());
            MessageBox.Show("Cadastro realizado com sucesso! Volte ao login para entrar.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
