using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hianyzasok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        struct OsztalyHiany
        {
            public string osztaly;
            public int orak;
        }

        struct Hianyzasok : IComparable
        {
            public string nev;
            public string osztaly;
            public int elsoNap;
            public int utolsoNap;
            public int mulasztottOrak;

            public int CompareTo(object obj)
            {
                if (((Hianyzasok)obj).mulasztottOrak == this.mulasztottOrak)
                {
                    return 0;
                }
                else if (((Hianyzasok)obj).mulasztottOrak == this.mulasztottOrak)
                {
                    return -1;
                }
                return 1;
            }
        }

        List<Hianyzasok> hianyzasok = new List<Hianyzasok>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn1Feladat_Click(object sender, RoutedEventArgs e)
        {
            lbListaKiir.Items.Clear();

            string fileNev = "";

            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                fileNev = ofd.FileName;
            }

            Hianyzasok adat = new Hianyzasok();

            string sor;
            string[] darabol;

            using (StreamReader sr = new StreamReader(fileNev))
            {
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    sor = sr.ReadLine();
                    lbListaKiir.Items.Add(sor);

                    darabol = sor.Split(';');
                    adat.nev = darabol[0];
                    adat.osztaly = darabol[1];
                    adat.elsoNap = int.Parse(darabol[2]);
                    adat.utolsoNap = int.Parse(darabol[3]);
                    adat.mulasztottOrak = int.Parse(darabol[4]);

                    hianyzasok.Add(adat);
                }

                txb1FeladatEredmeny.Text = "Az adatok beolvasása sikeres volt.";
            }
        }

        private void btn2Feladat_Click(object sender, RoutedEventArgs e)
        {
            int osszesHianyzas = 0;
            for (int i = 0; i < hianyzasok.Count; i++)
            {
                osszesHianyzas += hianyzasok[i].mulasztottOrak;
            }
            txb2FeladatEredmeny.Text = osszesHianyzas.ToString();
        }

        private void btn3Feladat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn4Feladat_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < hianyzasok.Count; i++)
            {
                if (txb3FeladatNevBeker.Text == hianyzasok[i].nev)
                {
                    if (hianyzasok[i].mulasztottOrak > 0)
                    {
                        txb4FeladatEredmeny.Text = "A tanuló hiányzott szeptemberben";
                        break;
                    }
                    else
                    {
                        txb4FeladatEredmeny.Text = "A tanuló nem hiányzott szeptemberben";
                        break;
                    }
                }
            }
        }

        private void btn5Feladat_Click(object sender, RoutedEventArgs e)
        {
            lbHianyzokKiir.Items.Clear();
            for (int i = 0; i < hianyzasok.Count; i++)
            {
                if (txb3FeladatNapBeker.Text == hianyzasok[i].elsoNap.ToString())
                {
                    txbNap.Text = txb3FeladatNapBeker.Text;
                    lbHianyzokKiir.Items.Add(hianyzasok[i].nev + " (" + hianyzasok[i].osztaly + ")");
                }
            }
        }

        private void btn6Feladat_Click(object sender, RoutedEventArgs e)
        {
            SortedDictionary<string, int> szotar = new SortedDictionary<string, int>();

            for (int i = 0; i < hianyzasok.Count; i++)
            {
                if (!szotar.ContainsKey(hianyzasok[i].osztaly)) //Ha nincs benne van a szótárban.
                {
                    szotar.Add(hianyzasok[i].osztaly, hianyzasok[i].mulasztottOrak);
                }
                else if (szotar.ContainsKey(hianyzasok[i].osztaly)) //Ha már benne van a szótárban.
                {
                    szotar[hianyzasok[i].osztaly] += hianyzasok[i].mulasztottOrak;
                }
            }

            //Kiíratás fájlba
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.DefaultExt = "csv";
            sfd.FileName = "osszesites";
            sfd.ShowDialog();

            using (StreamWriter sw = new StreamWriter(sfd.FileName))
            {
                foreach (var item in szotar)
                {
                    lb6HianyzokKiir.Items.Add(item);
                    sw.WriteLine(item);
                }

                MessageBox.Show("A mentés sikerült");
            }
        }
    }
}
