using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GanoHesap
{
    public partial class Form1 : Form
    {
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-6SJ94PD\\SQLEXPRESS02;Initial Catalog=OgrenciBilgi;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }
       
        
        private void button2_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("select *from Kullanici1", baglan);
            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if ((textBox4.Text == oku["KullanıcıAdı"].ToString().Trim()) && (textBox3.Text == oku["Şifre"].ToString().Trim()))
                {
                    // Session["YöneticiGiriş"] = true;
                    String KullaniciAdi = textBox4.Text;
                    KullaniciSayfa kullaniciSayfa = new KullaniciSayfa();
                    kullaniciSayfa.kullaniciAdiAl(KullaniciAdi);
                    
                    kullaniciSayfa.Show();
                    // this.Hide();

                }
            }
            baglan.Close();

        }

        
    }
}
