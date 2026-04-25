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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Owner is Form1 eski)
            {
                eski.girisOnay("var");
                eski.girisUser();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Owner is Form1 eski)
            {
                eski.girisOnay("admin");
                eski.girisAdmin();
                this.Dispose();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form7 yeni = new Form7();
            yeni.ShowDialog();
        }
    }
}
