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
        private DataSet1 set = new DataSet1();

        //Aqui se observa la tabla de OPPILAAT  
        private ObservableCollection<Oppilas> oppilaat = new ObservableCollection<Oppilas>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLisaa_Click(object sender, RoutedEventArgs e)
        {
            DataSet1 ds = new DataSet1();
            DataSet1.KurssiRow rivi = ds.Kurssi.NewKurssiRow();

            rivi.Kurssinnimi = this.textBoxKurssinnimi.Text;
            rivi.Alkamispvm = this.DatePickerAlkamisPvm.SelectedDate.Value;
            rivi.Paattymispvm = this.DatePickerPaattymisPvm.SelectedDate.Value;
            ds.Kurssi.AddKurssiRow(rivi);
            DataSet1TableAdapters.KurssiTableAdapter adap = new DataSet1TableAdapters.KurssiTableAdapter();
            adap.Update(ds.Kurssi);
            HaeData();
        }
        //
        // Summary:
        //     Gets a System.DateTime object that is set to the current date and time on this
        //     computer, expressed as the local time.
        //
        // Returns:
        //     An object whose value is the current local date and time.
        //public static DateTime Now { get; }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HaeData();
        }
        private void HaeData()

        {
            //DateTime dt = new DateTime();
            DataSet1 ds = new DataSet1();
            DataSet1TableAdapters.KurssiTableAdapter adap = new DataSet1TableAdapters.KurssiTableAdapter();
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
            DataSet1TableAdapters.OppilaatTableAdapter adap2 = new DataSet1TableAdapters.OppilaatTableAdapter();
            oppilaat.Clear();
            adap2.Fill(ds.Oppilaat); 
            foreach (DataRow row in ds.Tables["Oppilas"].Rows)
            {
                Oppilas o = new Oppilas();
                o.Id = int.Parse(row["Id"].ToString());
                o.KurssiId = int.Parse(row["KurssiId"].ToString());
                o.Etunimi = row["Etunimi"].ToString();
                o.Sukunimi = row["Sukunimi"].ToString();
                o.Sahkoposti = row["Sähköposti"].ToString();
                oppilaat.Add(o);
            }
            this.listViewOppilaat.ItemsSource = oppilaat;
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
            DataSet1 ds = new DataSet1();
            DataSet1TableAdapters.KurssiTableAdapter adap = new DataSet1TableAdapters.KurssiTableAdapter();
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
                DataSet1 ds = new DataSet1();
                DataSet1TableAdapters.KurssiTableAdapter adap = new DataSet1TableAdapters.KurssiTableAdapter();
                adap.Fill(ds.Kurssi);
                ds.Tables["Kurssi"].Rows[this.listView.SelectedIndex].Delete();
                adap.Update(ds.Kurssi);
                HaeData();
            }
        }

        private void buttonLisaaOppilas_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxKurssi.SelectedIndex > 0)
            {
                DataSet1 ds = new DataSet1();
                DataSet1.OppilaatRow rivi = ds.Oppilaat.NewOppilaatRow();
                string strkurssi = this.comboBoxKurssi.Text;
                int paikka = strkurssi.IndexOf(' ');
                rivi.KurssiId = int.Parse(strkurssi.Substring(0, paikka));
                rivi.Etunimi = this.textBoxEtunimi.Text;
                rivi.Sukunimi = this.textBoxSukunimi.Text;
                rivi.Sahkoposti = this.textBoxSähköposti.Text;
                ds.Oppilaat.AddOppilaatRow(rivi);
                DataSet1TableAdapters.OppilaatTableAdapter adap = new DataSet1TableAdapters.OppilaatTableAdapter();
                adap.Update(ds.Oppilaat);
                HaeData();
            }
        }

        private void buttonMuutaOppilas_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
