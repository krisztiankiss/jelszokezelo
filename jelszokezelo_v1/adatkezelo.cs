using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace jelszokezelo_v1
{
    //Ez a jó
    public partial class adatkezelo : Form
    {
        public List<string> felhasznalonevek = new List<string>();// felhasználónevek
        public List<string> jelszo = new List<string>();// jelszavak
        public int index = 0; // egy elem jelenlegi helye
        FileStream fajl;


        string titkosjelszo = "kismacska"; // titkosító jelszó
        public adatkezelo()
        {
            InitializeComponent();
        }

        // Fájl vizsgálata, létrehozása vagy beolvasása, nevek és jelszavak tárolása
        private void adatkezelo_Load(object sender, EventArgs e)
        {
            
            if (!File.Exists("felhasznalok.txt"))
            {
                 fajl = new FileStream("felhasznalok.txt", FileMode.Create);
            }
            else
            {
                 fajl = new FileStream("felhasznalok.txt", FileMode.Open);
            }
            
            StreamReader beolvas = new StreamReader(fajl);         
            string egysor = beolvas.ReadLine();
            if (egysor == null)
            {

            }
            else  { 
                while (egysor != null)
                {
                    felhasznalonevek.Add(egysor);
                    egysor = beolvas.ReadLine();
                    jelszo.Add(egysor);
                    egysor = beolvas.ReadLine();

                }

                for (int i = 0; i < felhasznalonevek.Count; i++)
                {
                    lb_felhasznalo.Items.Add(felhasznalonevek[i]);
                }

               
            }
            beolvas.Close();

            //A felhsaználó létrehozás gomb letiltása, az üres értérkek bevitele ellen
            btn_letrehozas.Enabled = false;
            //A visszafejtett jelszót megjelenítő label "elrejtése"
            lb_pass_en.Visible = false;        
        }

        // A listán kijeleölt felhasználó, jelszavának megjelenítése
        private void lb_felhasznalo_SelectedIndexChanged(object sender, EventArgs e)
        {         
            tb_felhasznalojelszo.Text = "";
            for (int i = 0; i < felhasznalonevek.Count; i++)
            {
             
                if (lb_felhasznalo.SelectedItem.Equals(felhasznalonevek[i]))
                {           
                    tb_felhasznalojelszo.Text += jelszo[i];
                }
                
            }
        }

        // Felhasználó létrehozás, jelszó titkosítás és tárolás
        private void btn_letrehozas_Click(object sender, EventArgs e)
        {
                string titkositott_jel = titkositas.Encrypt(tb_ujfelhasznalojelszo.Text, titkosjelszo);
                felhasznalonevek.Add(tb_ujfelhasznalonev.Text);
                jelszo.Add(titkositott_jel);

                lb_felhasznalo.Items.Add(tb_ujfelhasznalonev.Text);

                FileStream fajl = new FileStream("felhasznalok.txt", FileMode.Create);
                StreamWriter iro = new StreamWriter(fajl);

                for (int i = 0; i < felhasznalonevek.Count; i++)
                {
                    iro.WriteLine(felhasznalonevek[i]);
                    // string titkositott_jel = titkositas.Encrypt(jelszo[i], titkosjelszo);
                    iro.WriteLine(jelszo[i]);
                }

                iro.Close();

                tb_ujfelhasznalonev.Text = "";
                tb_ujfelhasznalojelszo.Text = "";              
        }

        // Keresés a listában
        private void bt_kereses_Click(object sender, EventArgs e)
        {
            index = 0;
            for (int i = 0; i < felhasznalonevek.Count; i++)
            {
                if(tb_keres.Text.Equals(felhasznalonevek[i]))
                {
                    //tb_1.Text += felhasznalonevek[i];
                    lb_felhasznalo.SelectedIndex = i-1;
                    index = i;
                }
            }
        }

        // Már meglévő felhasználó jelszavának felülítása, titkosítása éstárolása
        private void btn_ujjelszo_Click(object sender, EventArgs e)
        {

            if (lb_felhasznalo.SelectedItems.Count==0) 
            {
                MessageBox.Show("Jelölj ki egy felhasználót!", "Hiba");
                tb_ujjelszo.Text = "";
                tb_ujjelszo2.Text = "";
                
               
            }
            else if (tb_ujjelszo.Text.Equals(tb_ujjelszo2.Text))
            {
                //jelszó felülírás
                jelszo[index] = tb_ujjelszo.Text;
                string titkositott_jel = titkositas.Encrypt(jelszo[index], titkosjelszo);
                jelszo[index] = titkositott_jel;
                //megerősítő üzenet
                MessageBox.Show("Az új jelszó mentése megtörtént", "Üzenet");
                tb_ujjelszo.Text = "";
                tb_ujjelszo2.Text = "";

                //fájlba tárolás

           
                   FileStream fajl = new FileStream("felhasznalok.txt", FileMode.Create);
                   StreamWriter iro = new StreamWriter(fajl);

                for (int i = 0; i < felhasznalonevek.Count; i++)
                {
                    iro.WriteLine(felhasznalonevek[i]);
                    iro.WriteLine(jelszo[i]);                 
                }
                   
                   iro.Close();
           
            }
            else
            {               
                tb_ujjelszo2.Text = "";
                tb_ujjelszo2.Focus();
                MessageBox.Show("A megadott jelszavak nem egyeznek!", "Hiba");
            }
        }

        // Keresés könnyítés sok felhasználó esetén, kezdőbetű szűréssel.
        private void kereso(object sender, EventArgs e)
        {
            lb_felhasznalo.Items.Clear();

            foreach(string str in felhasznalonevek)
            {
                if(str.StartsWith(tb_keres.Text,StringComparison.CurrentCultureIgnoreCase))
                {
                    lb_felhasznalo.Items.Add(str);
                }
            }

        }

        // Kilépés gomb
        private void label8_Click(object sender, EventArgs e)
        {
            //megerősítés kérése
            DialogResult dontes;
            dontes = MessageBox.Show("Biztos ki szeretnél lépni?","Üzenet.", MessageBoxButtons.YesNo);

            if (dontes == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        // A felhasználó létrehozása gomb engedélyezése,
        // ha minimum 2 karakter van a felhasználónévből és a jelszóból is 
        private void letrehozas_enged(object sender, EventArgs e)
        {
            if(tb_ujfelhasznalonev.Text.Length>1 && tb_ujfelhasznalojelszo.Text.Length>1)
            {
                btn_letrehozas.Enabled = true;
            }
            else
            {
                btn_letrehozas.Enabled = false;
            }
        }

        // A visszafejtett jelszó megjelenítése, és elrejtése
        private void pw_mutat(object sender, EventArgs e)
        {
            lb_pass_en.Visible = true;
            lb_pass_en.Text = "A titkosítatlan jelszó: " + titkositas.Decrypt(tb_felhasznalojelszo.Text, titkosjelszo);
            
        }
        private void pw_elrejt(object sender, EventArgs e)
        {
            lb_pass_en.Visible = false;
        }

        // Kijelölt felhasználó törlése, megerősítés kérése
        private void bt_felhasznalotorles_Click(object sender, EventArgs e)
        {
            if (lb_felhasznalo.SelectedItems.Count == 0)
            {
                MessageBox.Show("Nincs kijeleölt felhasználó!", "Hiba");
            }
            else
            {
                string kijelölt_nev = lb_felhasznalo.SelectedItem.ToString();
                List<string> uj_felhasznalok = new List<string>();
                List<string> uj_jelszo = new List<string>();

                for (int i = 0; i < felhasznalonevek.Count; i++)
                {
                    if (felhasznalonevek[i].Equals(kijelölt_nev)) { }
                    else
                    {
                        uj_felhasznalok.Add(felhasznalonevek[i]);
                        uj_jelszo.Add(jelszo[i]);
                    }
                }
                felhasznalonevek.Clear();
                jelszo.Clear();
                
                DialogResult torles;
                torles = MessageBox.Show("Biztos törölni szeretnéd a kijelölt felhasználót?", "Üzenet.", MessageBoxButtons.YesNo);

                if (torles == DialogResult.Yes)
                {
                    lb_felhasznalo.Items.Clear();
                    File.Delete("felhasznalok.txt");
                    fajl = new FileStream("felhasznalok.txt", FileMode.Create);
                    StreamWriter iro = new StreamWriter(fajl);
                    for (int i = 0; i < uj_felhasznalok.Count; i++)
                    {
                        iro.WriteLine(uj_felhasznalok[i]);
                        iro.WriteLine(titkositas.Encrypt(uj_jelszo[i], titkosjelszo));
                        lb_felhasznalo.Items.Add(uj_felhasznalok[i]);
                    }
                    iro.Close();
                    tb_felhasznalojelszo.Clear();

                }
                
                
            }
        }
    }
}
