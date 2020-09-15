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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        }
    }
}
