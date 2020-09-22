using System;
using System.Collections.Generic;
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
//Lisää koodi
using System.Collections.ObjectModel;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private const string V = "M/d/yyyy";
        //Aqui se observa la tabla de KURSSI
        private ObservableCollection<Kurssi> kurssit = new ObservableCollection<Kurssi>();
        private KoulustaDataSet set = new KoulustaDataSet();

        //Aqui se observa la tabla de OPPILAAT  
        private ObservableCollection<Oppilas> oppilaat = new ObservableCollection<Oppilas>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLisaa_Click(object sender, RoutedEventArgs e)
        {
            KoulustaDataSet ds = new KoulustaDataSet();
            KoulustaDataSet.KurssiRow rivi = ds.Kurssi.NewKurssiRow();

            rivi.Kurssinnimi = this.textBoxKurssinnimi.Text;

            //jos päättymispäivä ei ole valittu
            if (this.DatePickerAlkamisPvm.SelectedDate == null)
            {
                rivi.Alkamispvm = DateTime.Now;
            }
            else
            {
                rivi.Alkamispvm = this.DatePickerAlkamisPvm.SelectedDate.Value;

            }

            //jos päättymispäivää ei ole valittu
            if (this.DatePickerAlkamisPvm.SelectedDate == null)
            {
                rivi.Paattymispvm = DateTime.Now;
            }
            else
            {
                rivi.Paattymispvm = this.DatePickerPaattymisPvm.SelectedDate.Value;
            }
            ds.Kurssi.AddKurssiRow(rivi);
            KoulustaDataSetTableAdapters.KurssiTableAdapter adap = new KoulustaDataSetTableAdapters.KurssiTableAdapter();
            adap.Update(ds.Kurssi);
            HaeData();
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HaeData();
        }


        //-----------------------------------------------------------
        private void TyhjaaKurssiLiittyma()
        {
            this.textBoxKurssinnimi.Text = "";
            this.DatePickerAlkamisPvm.SelectedDate = null;
            this.DatePickerPaattymisPvm.SelectedDate = null;
        }
        private void TyhjaaOppilasLiittyma()
        {
            this.textBoxEtunimi.Text = "";
            this.textBoxSukunimi.Text = "";
            this.textBoxSähköposti.Text = "";
        }
        //------------------------------------------------------------

        private void HaeData()

        {
            //DateTime dt = new DateTime();
            KoulustaDataSet ds = new KoulustaDataSet();
             KoulustaDataSetTableAdapters.KurssiTableAdapter adap = new KoulustaDataSetTableAdapters.KurssiTableAdapter();
            kurssit.Clear();

            adap.Fill(ds.Kurssi);
            foreach (DataRow row in ds.Tables["Kurssi"].Rows)
            {
                Kurssi k = new Kurssi();
                // DateTime dt = DateTime[DateTime.Now];
                //dt = DateTime.Now(V);
                k.Id = int.Parse(row["Id"].ToString());
                k.Kurssinnimi = row["Kurssinnimi"].ToString();
                k.Alkamispvm = DateTime.Parse(row["Alkamispvm"].ToString());
                k.Paattymispvm = DateTime.Parse(row["Paattymispvm"].ToString());
                kurssit.Add(k);
            }
            this.listView.ItemsSource = kurssit;

            //Aqui se suma la parte de OPPILAT
            this.comboBoxKurssi.Items.Clear();
            this.comboBoxKurssi.Items.Add("Valitse kursii");
            foreach (Kurssi k in kurssit)
            {
                this.comboBoxKurssi.Items.Add(k.Id + " " + k.Kurssinnimi);
            }
            this.comboBoxKurssi.SelectedIndex = 0;

            //Esta es la parte de OPPILAAT
            KoulustaDataSetTableAdapters.OppilaatTableAdapter adap2 = new KoulustaDataSetTableAdapters.OppilaatTableAdapter();
            oppilaat.Clear();
            adap2.Fill(ds.Oppilaat); 
            foreach (DataRow row in ds.Tables["Oppilaat"].Rows)
            {
                Oppilas o = new Oppilas();
                o.Id = int.Parse(row["Id"].ToString());
                o.KurssiId = int.Parse(row["KurssiId"].ToString());
                o.Etunimi = row["Etunimi"].ToString();
                o.Sukunimi = row["Sukunimi"].ToString();
                o.Sahkoposti = row["Sahkoposti"].ToString();
                oppilaat.Add(o);
            }
            this.listViewOppilaat.ItemsSource = oppilaat;

            //Inicializar datos con esta función, estas funciones están definidas arriba
            TyhjaaKurssiLiittyma();
            TyhjaaOppilasLiittyma();

            //Aqui está la tercera parte KURSIIN OPPILAAT
            this.comboBoxKurssinimet.Items.Clear();
            this.comboBoxKurssinimet.Items.Add("valitse kurssi");
            foreach (Kurssi k in kurssit)
            {
                this.comboBoxKurssinimet.Items.Add(k.Id + " " + k.Kurssinnimi);
            }
            this.comboBoxKurssinimet.SelectedIndex = 0;
        }

        private void buttonMutta_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView.SelectedIndex != -1) //jokin rivi on valittu
            {
                this.textBoxKurssinnimi.Text = (this.listView.Items[this.listView.SelectedIndex] as Kurssi).Kurssinnimi;

                //Actualizar el dia de empezar el curso
                this.DatePickerAlkamisPvm.SelectedDate = (this.listView.Items[this.listView.SelectedIndex] as Kurssi).Alkamispvm;
                this.DatePickerAlkamisPvm.DisplayDate = this.DatePickerAlkamisPvm.SelectedDate.Value;

                //Actualizar el dia en que termina el curso
                this.DatePickerPaattymisPvm.SelectedDate = (this.listView.Items[this.listView.SelectedIndex] as Kurssi).Paattymispvm;
                this.DatePickerPaattymisPvm.DisplayDate = this.DatePickerPaattymisPvm.SelectedDate.Value;

                //-----------------------------------------------------------------
                //Aqui va la parte del botón
                this.buttonMutta.Visibility = Visibility.Hidden;
                //Aqui se cumple el IF cuando buttonMutta cambia este aparece
                this.buttonTallenna.Visibility = Visibility.Visible;

            }
        }

        private void buttonTallenna_Click(object sender, RoutedEventArgs e)
        {
            KoulustaDataSet ds = new KoulustaDataSet();
            KoulustaDataSetTableAdapters.KurssiTableAdapter adap = new KoulustaDataSetTableAdapters.KurssiTableAdapter();
            adap.Fill(ds.Kurssi);
            ds.Tables["Kurssi"].Rows[this.listView.SelectedIndex]["Kurssinnimi"] = this.textBoxKurssinnimi.Text;
            ds.Tables["Kurssi"].Rows[this.listView.SelectedIndex]["Alkamispvm"] = this.DatePickerAlkamisPvm.SelectedDate;
            ds.Tables["Kurssi"].Rows[this.listView.SelectedIndex]["Paattymispvm"] = this.DatePickerPaattymisPvm.SelectedDate;
            adap.Update(ds);
            HaeData();
            this.buttonMutta.Visibility = Visibility.Visible;
            this.buttonTallenna.Visibility = Visibility.Hidden;
         }

        private void buttoPoista_Click(object sender, RoutedEventArgs e)
        {
            if ( this.listView.SelectedIndex != -1) //jokin rivi on valittu
            {
                KoulustaDataSet ds = new KoulustaDataSet();
                KoulustaDataSetTableAdapters.KurssiTableAdapter adap = new KoulustaDataSetTableAdapters.KurssiTableAdapter();
                adap.Fill(ds.Kurssi);
                ds.Tables["Kurssi"].Rows[this.listView.SelectedIndex].Delete();
                adap.Update(ds.Kurssi);
                HaeData();
            }
        }

        private void buttonLisaaOppilas_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxKurssi.SelectedIndex != 0)
            {
                KoulustaDataSet ds = new KoulustaDataSet();
                KoulustaDataSet.OppilaatRow rivi = ds.Oppilaat.NewOppilaatRow();
                string strkurssi = this.comboBoxKurssi.Text;
                int paikka = strkurssi.IndexOf(' ');
                rivi.KurssiId = int.Parse(strkurssi.Substring(0, paikka));
                rivi.Etunimi = this.textBoxEtunimi.Text;
                rivi.Sukunimi = this.textBoxSukunimi.Text;
                rivi.Sahkoposti = this.textBoxSähköposti.Text;
                ds.Oppilaat.AddOppilaatRow(rivi);
                KoulustaDataSetTableAdapters.OppilaatTableAdapter adap = new KoulustaDataSetTableAdapters.OppilaatTableAdapter();
                adap.Update(ds.Oppilaat);
                HaeData();
            }
        }

        private void buttonMuutaOppilas_Click(object sender, RoutedEventArgs e)
        {
            if (this.listViewOppilaat.SelectedIndex != -1) // jokin rivi on valittu
            {
                int index = 0;
                foreach (Kurssi k in kurssit)
                {
                    if (k.Id == ((this.listViewOppilaat.Items[this.listViewOppilaat.SelectedIndex] as Oppilas).KurssiId))
                    {
                        break;
                    }
                    index++;
                }
                this.comboBoxKurssi.SelectedIndex = index + 1;
                this.textBoxEtunimi.Text = (this.listViewOppilaat.Items[this.listViewOppilaat.SelectedIndex] as Oppilas).Etunimi;
                this.textBoxSukunimi.Text = (this.listViewOppilaat.Items[this.listViewOppilaat.SelectedIndex] as Oppilas).Sukunimi;
                this.textBoxSähköposti.Text = (this.listViewOppilaat.Items[this.listViewOppilaat.SelectedIndex] as Oppilas).Sahkoposti;

                this.buttonMuutaOppilas.Visibility = Visibility.Hidden;
                //Cambio de VISIVILIDAD con el BUTTON de cambiar o guardar
                this.buttonTallennaOppilas.Visibility = Visibility.Visible;
            }
        }

        private void buttonTallennaOppilas_Click(object sender, RoutedEventArgs e)
        {
            KoulustaDataSet ds = new KoulustaDataSet();
            KoulustaDataSetTableAdapters.OppilaatTableAdapter adap = new KoulustaDataSetTableAdapters.OppilaatTableAdapter();
            adap.Fill(ds.Oppilaat);
            string strkurssi = this.comboBoxKurssi.Text;
            int paikka = strkurssi.IndexOf(' ');
            int KurssiId = int.Parse(strkurssi.Substring(0, paikka));
            ds.Tables["Oppilas"].Rows[this.listViewOppilaat.SelectedIndex]["KurssiId"] = KurssiId;
            ds.Tables["Oppilas"].Rows[this.listViewOppilaat.SelectedIndex]["Etunimi"] = this.textBoxEtunimi.Text;
            ds.Tables["Oppilas"].Rows[this.listViewOppilaat.SelectedIndex]["Sukunimi"] = this.textBoxSukunimi.Text;
            ds.Tables["Oppilas"].Rows[this.listViewOppilaat.SelectedIndex]["Sahkoposti"] = this.textBoxSähköposti.Text;
            adap.Update(ds.Oppilaat);
            HaeData();
            this.buttonMuutaOppilas.Visibility = Visibility.Visible;
            this.buttonTallennaOppilas.Visibility = Visibility.Hidden;
        }

        private void buttonPoistaOppilas_Click(object sender, RoutedEventArgs e)
        {
            if (this.listViewOppilaat.SelectedIndex != -1)//jokin rivi on valittu
            {
                KoulustaDataSet ds = new KoulustaDataSet();
                KoulustaDataSetTableAdapters.OppilaatTableAdapter adap = new KoulustaDataSetTableAdapters.OppilaatTableAdapter();
                adap.Fill(ds.Oppilaat);
                ds.Tables["Oppilas"].Rows[this.listViewOppilaat.SelectedIndex].Delete();
                adap.Update(ds.Oppilaat);
                HaeData();
            }
        }

        private void comboBoxKurssi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBoxKurssinimet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Oppilas> kurssinoppilaat = new ObservableCollection<Oppilas>();
            KoulustaDataSet ds = new KoulustaDataSet();

            KoulustaDataSetTableAdapters.OppilaatTableAdapter adap2 = new KoulustaDataSetTableAdapters.OppilaatTableAdapter();
            kurssinoppilaat.Clear();
            adap2.Fill(ds.Oppilaat);
            foreach (DataRow row in ds.Tables["Oppilaat"].Rows)
            {
                Oppilas o = new Oppilas();
                o.Id = int.Parse(row["Id"].ToString());
                o.KurssiId = int.Parse(row["KurssiId"].ToString());
                o.Etunimi = row["Etunimi"].ToString();
                o.Sukunimi = row["Sukunimi"].ToString();
                o.Sahkoposti = row["Sahkoposti"].ToString();
                if (this.comboBoxKurssinimet.SelectedIndex > 0)
                {
                    string strkurssi = this.comboBoxKurssinimet.SelectedValue.ToString();
                    int paikka = strkurssi.IndexOf(' ');
                    int kurssiid = int.Parse(strkurssi.Substring(0, paikka));
                    if (o.KurssiId == kurssiid)
                    {
                        kurssinoppilaat.Add(o);
                    }
                }
            }
            this.listViewKurssiOppilaat.ItemsSource = kurssinoppilaat;
        }
    }
}
