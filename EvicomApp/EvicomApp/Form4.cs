using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvicomApp
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form12 yeni = new Form12();
            yeni.ShowDialog();
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form13 yeni = new Form13();
            yeni.ShowDialog();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form14 yeni = new Form14();
            yeni.ShowDialog();
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form15 yeni = new Form15();
            yeni.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form16 yeni = new Form16();
            yeni.ShowDialog();
        }
    }
}
