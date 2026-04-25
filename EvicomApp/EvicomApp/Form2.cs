using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvicomApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private bool adminMi = false;
        private string hedefKlasor = Path.Combine(Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\")), "Resim");
        private string uzanti;
        private string hedefResimYolu;
        private bool resimGirildiMi = false;

        public void setAdmin()
        {
            adminMi = true;
            label1.Text = "Admin";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog yeni = new OpenFileDialog();
            yeni.Filter = "Resim Dosyaları (*.jpg; *.jpeg; *.png; *.bmp;)|*.jpg; *.jpeg; *.png; *.bmp;";
            yeni.Title = "Yüklenecek resmi seçiniz";
            if (yeni.ShowDialog() == DialogResult.OK)
            {
                uzanti = Path.GetExtension(yeni.FileName);
                hedefResimYolu = Path.GetFullPath(yeni.FileName);
                pictureBox1.Image = Image.FromFile(yeni.FileName);
                resimGirildiMi= true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(resimGirildiMi == true)
            {
                try
                {
                    if (!Directory.Exists(hedefKlasor))
                    {
                        Directory.CreateDirectory(hedefKlasor);
                    }
                    int dosyaSayisi = Directory.GetFiles(hedefKlasor).Length;
                    string yeniDosyaAdi = (dosyaSayisi + 1).ToString() + uzanti;
                    string hedefYol = Path.Combine(hedefKlasor, yeniDosyaAdi);
                    File.Copy(hedefResimYolu, hedefYol, true);
                    MessageBox.Show($"Resim \"{yeniDosyaAdi}\" adıyla EvicomApp\\EvicomApp\\Resim klasörüne kaydedildi");
                    this.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya seçilirken bir hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bilgileri giriniz");
            }

        }
    }
}
