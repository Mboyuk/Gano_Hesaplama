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
    public partial class KullaniciSayfa : Form
    {
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-6SJ94PD\\SQLEXPRESS02;Initial Catalog=OgrenciBilgi;Integrated Security=True");
        public void kullaniciAdiAl(String Ad)
        {
            String kullaniciAdi = Ad;
            label6.Text = kullaniciAdi;
        }
      
        
        public KullaniciSayfa()
        {
            InitializeComponent();
        }
        public void verileriGoster()
        {
           
            String ad = label6.Text;
            SqlDataAdapter da = new SqlDataAdapter("select * from "+ad, baglan);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            String ad="Ders".Trim();
            
            verileriGoster();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglan.Open();
            String KullaniciAdi = "insert into " + label6.Text;
            SqlCommand komut = new SqlCommand(KullaniciAdi+"(DersAdi,Vize,Final,Kredi) values(@DersAdi,@VizeNotu,@FinalNotu,@Kredin)", baglan);
            komut.Parameters.AddWithValue("@DersAdi", textBox1.Text);
            komut.Parameters.AddWithValue("@VizeNotu", textBox2.Text);
            komut.Parameters.AddWithValue("@FinalNotu", textBox3.Text);
            komut.Parameters.AddWithValue("@Kredin", textBox4.Text);
            komut.ExecuteNonQuery();
            verileriGoster();
            
            baglan.Close();
            
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglan.Open();
            String KullaniciAdi = "delete from " + label6.Text;
            
            SqlCommand komut = new SqlCommand(KullaniciAdi+" where DersAdi=@DersAdi",baglan);
            komut.Parameters.AddWithValue("@DersAdi", textBox5.Text);
            komut.ExecuteNonQuery();
            verileriGoster();
            baglan.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            String dersAdi = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            String vize = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            String final = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            String kredi = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            textBox1.Text = dersAdi;
            textBox2.Text = vize;
            textBox3.Text = final;
            textBox4.Text = kredi;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglan.Open();
            
            String ko = "update " + label6.Text;
            SqlCommand komut = new SqlCommand(ko+" set DersAdi='" + textBox1.Text + "',Vize='" + textBox2.Text + "',Final='" + textBox3.Text + "',Kredi='" + textBox4.Text+"' where DersAdi='"+textBox1.Text+"'", baglan);
            komut.ExecuteNonQuery();
            verileriGoster();
            baglan.Close();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {         
            baglan.Open();
            String kullaniciAdi = "select count(*) from " + label6.Text;
            SqlCommand komut11 = new SqlCommand(kullaniciAdi, baglan);
            int satirSayisi = Convert.ToInt32(komut11.ExecuteScalar());
            komut11.ExecuteNonQuery();
            baglan.Close();
            
            int[] vize = new int[satirSayisi];
            int[] final = new int[satirSayisi];
            int[] kredi = new int[satirSayisi];
            double[] ortalama = new double[satirSayisi];
            int i = 0,j=0,k=0;
            double orta = 0.0;
            int toplamKredi = 0;
            baglan.Open();
            String ad = "select * from " + label6.Text;
            SqlCommand komut = new SqlCommand(ad,baglan);
            
            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                
                vize[i] = Convert.ToInt32(oku["Vize"].ToString());
                final[i] = Convert.ToInt32(oku["Final"].ToString());
                kredi[i] = Convert.ToInt32(oku["Kredi"].ToString());
                i++;
            }
           
           
           // textBox6.Text = ((double)(vize[1] * 0.4) + (double)(final[1] * 0.6)).ToString();
           for(j=0;j<satirSayisi;j++)
            {
                ortalama[j] = ((vize[j] * 0.4) + (final[j] * 0.6));
            }
         
           for( k=0;k<satirSayisi;k++)
            {
                orta = orta + (double)(harfKontrol(ortalama[k]) * kredi[k]);
                
            }
           for(int l=0;l<satirSayisi;l++)
            {
                toplamKredi = toplamKredi + kredi[l];
            }

            textBox8.Text = orta.ToString();
            textBox9.Text = toplamKredi.ToString();
            double gano = (double)(orta/toplamKredi);
            textBox6.Text = gano.ToString();

           double harfKontrol(double a)
            {
                if (a >= 84.5 && a <= 100)//AA
                {
                    return 4.0;
                }
                else if (a >= 74.5 && a < 84.5)//BA
                {
                    return 3.5;
                }
                else if (a >= 64.5 && a < 74.5)//BB
                {
                    return 3.0;
                }
                else if (a >= 56.5 && a < 64.5)//CB
                {
                    return 2.5;
                }
                else if (a >= 49.5 && a < 56.5)//CC
                {
                    return 2.0;
                }
                else if (a >= 44.5 && a < 49.5)//DC
                {
                    return 1.5;
                }
                else if (a >= 39.5 && a < 44.5)//DD
                {
                    return 1.0;
                }
                else if (a >= 29.5 && a < 39.5)//FD
                {
                    return 0.5;
                }
                else if (a >= 0 && a < 29.5)//FF
                {
                    return 0.0;
                }
                else
                    return 500;
                
            }
            baglan.Close();
            if (gano > 3.00 && gano < 3.49)
            {
                label11.Text = "Aferim çalışıyorsun";
            }
            else if (gano > 3.49 && gano < 3.99)
            {
                label11.Text = "inanılmazsın... SECTUM SEMPRA";
            }
            else if (gano > 2.5 && gano < 2.99)
            {
                label11.Text = "Biraz daha çalışırsan daha iyi olur ";
            }
            else if (gano > 2.00 && gano < 2.49)
            {
                label11.Text = "iyi değil ortağımm. Daha fazla çalış";
            }
            else if (gano > 1.5 && gano < 1.99)
            {
                label11.Text = "lol oynamayı bırakta azcık ders çalışş yoksa bu okul bitmez";
            }
            else if (gano > 1.00 && gano < 1.49)
            {
                label11.Text = "Abuvvvvvvvvvvvvv durum çookkk vahimmm. Bilgisayar oynamayı bırakmalısın belki de";
            }
            else
                label11.Text = "yorumsuzzz.. Galdıııııııınnnnnnnnnnnnn. Bu okul bitmez hacım";


           /* textBox6.Text = ortalama[0] + " " + ortalama[1]
            + " " + ortalama[2];*/
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baglan.Open();
            String kullaniciAdi = "select count(*) from " + label6.Text;
            SqlCommand komut11 = new SqlCommand(kullaniciAdi, baglan);
            int satirSayisi = Convert.ToInt32(komut11.ExecuteScalar());
            komut11.ExecuteNonQuery();
            textBox7.Text = satirSayisi.ToString();
            baglan.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        
    }
}
