using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pensafy_Final_Integrated
{
    public class FormRanking : Form
    {
        private DataGridView dgv;
        public FormRanking()
        {
            InitializeComponent();
            Theme.AplicarGradiente(this, Color.FromArgb(18,164,120), Color.FromArgb(46,204,113));
            CarregarRanking();
        }
        private void InitializeComponent()
        {
            this.Text = "Pensafy - Ranking";
            this.ClientSize = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            dgv = new DataGridView() { Left = 20, Top = 20, Width = 540, Height = 420, ReadOnly = true, AllowUserToAddRows = false };
            dgv.Columns.Add("Posicao", "Posição");
            dgv.Columns.Add("Nome", "Jogador");
            dgv.Columns.Add("Pontos", "Pontos");
            this.Controls.Add(dgv);
        }
        private void CarregarRanking()
        {
            dgv.Rows.Clear();
            var lista = Database.ObterRanking();
            int pos = 1;
            foreach(var item in lista)
            {
                dgv.Rows.Add(pos.ToString(), item.Nome, item.Pontos.ToString());
                pos++;
            }
        }
    }
}
