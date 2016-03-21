using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using Gestionix;
using Gestionix.POS;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker bwtest;
        public List<string> Meesages;

        public MainWindow()
        {
            InitializeComponent();
            
            bwtest = new BackgroundWorker();
            bwtest.DoWork += bwtest_DoWork;
            bwtest.RunWorkerCompleted += bwtest_RunWorkerCompleted;
            bwtest.WorkerReportsProgress = true;
            bwtest.ProgressChanged += bwtest_ProgressChanged;

            List<String> names = new List<string>();
            names.Add("Transferencia");
            names.Add("Vales");
            names.Add("Tarjeta de crédito o débito");
            names.Add("American Express");
            names.Add("Efectivo");
            names.Add("Otro");
            Cmb.ItemsSource = names;

            Meesages = new List<string>();
            Meesages.Add("mensaje 1");
            Meesages.Add("mensaje 2");
            Meesages.Add("mensaje 3");
            Meesages.Add("mensaje 4");
            Meesages.Add("mansaje 5");
            Meesages.Add("mensaje 6");

            System.Globalization.CultureInfo C = System.Globalization.CultureInfo.CreateSpecificCulture("es-MX");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = C;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = C;

            GestionixPOSCulture.SetPOSCulture(SupportedCultures.MEXICO);
        }

        void bwtest_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TxtSearchingResult.Text = "buscando..." + e.ProgressPercentage.ToString();
        }

        void bwtest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HintSearchBox T = (HintSearchBox)e.Result;
            T.IsSearching = false;
            TxtSearchingResult.Text = "ya buscó algo";
        }

        void bwtest_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int seconds = 0; seconds < 3; seconds++)
            {
                bwtest.ReportProgress(seconds + 1);
                Thread.Sleep(1000);
            }
            e.Result = e.Argument;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(TxtSeatch);
        }

        private void TxtSeatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HintSearchBox t = (HintSearchBox)sender;

                if(!bwtest.IsBusy)
                    bwtest.RunWorkerAsync(t);
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            RingTest.IsActivated = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RingTest.IsActivated = true;
        }

        private void TxtSeatch_Search(object sender, RoutedEventArgs e)
        {
            bwtest.RunWorkerAsync(sender as HintSearchBox);
        }

        private void TxtS_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("entró al textchanged");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("entró al textchanged visualbrush");
        }

        private void TxtP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("entró a password");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SuccessMessage.Visibility = System.Windows.Visibility.Visible;
            ErrorMessage.Visibility = System.Windows.Visibility.Visible;
            InfoMessage.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
