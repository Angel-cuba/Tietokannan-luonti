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
        private ObservableCollection<Kurssi> kurssit = new ObservableCollection<Kurssi>();
        private DataSet1 set = new DataSet1();

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HaeData();
        }
        private void HaeData()
        {
            DataSet1 ds = new DataSet1();
            DataSet1TableAdapters.KurssiTableAdapter adap = new DataSet1TableAdapters.KurssiTableAdapter();
            kurssit.Clear();

            adap.Fill(ds.Kurssi);
            foreach (DataRow row in ds.Tables["Kurssi"].Rows)
            {
                Kurssi k = new Kurssi();
                k.Id = int.Parse(row["Id"].ToString());
                k.Kurssinnimi = row["Kurssinnimi"].ToString();
                k.Alkamispvm = DateTime.Parse(row["Alkamispvm"].ToString());
                k.Paattymispvm = DateTime.Parse(row["Paattymispvm"].ToString());
                kurssit.Add(k);
            }
            this.listView.ItemsSource = kurssit;
        }
    }
}
