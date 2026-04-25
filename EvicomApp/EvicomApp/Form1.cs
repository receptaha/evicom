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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string giris = "yok";
        private bool adminMi = false;

        public void girisOnay(string giristuru)
        {
            giris = giristuru;
        }

        private void gizle()
        {
            this.Enabled = false;
            this.Opacity = 0;
            this.Visible = false;
        }

        public void girisUser()
        {
            this.Enabled = true;
            this.Opacity = 100;
            button3.Visible = false;
            this.Visible = true;
        }

        public void girisAdmin()
        {
            this.Enabled = true;
            this.Opacity = 100;
            this.Visible = true;
            adminMi = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 yeni = new Form2();
            button2.Enabled = false;
            if (adminMi)
            {
                yeni.setAdmin();
            }
            yeni.ShowDialog();
            if(yeni.DialogResult == DialogResult.Cancel)
            {
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 yeni = new Form3();
            button1.Enabled = false;
            if (adminMi)
            {
                yeni.setAdmin();
            }
            yeni.ShowDialog();
            if (yeni.DialogResult == DialogResult.Cancel)
            {
                button1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 yeni = new Form4();
            button3.Enabled = false;
            yeni.ShowDialog();
            if (yeni.DialogResult == DialogResult.Cancel)
            {
                button3.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 yeni = new Form5();
            button4.Enabled = false;
            yeni.ShowDialog();
            if (yeni.DialogResult == DialogResult.Cancel)
            {
                button4.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.gizle();
            Form6 yeni = new Form6();
            yeni.Owner = this;
            yeni.ShowDialog();
            if (adminMi)
            {
                label1.Text = "Admin";
            }
            if (yeni.DialogResult == DialogResult.Cancel && giris == "yok")
            {
                this.Close();
            }
        }
    }
}
