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
using System.Collections.ObjectModel;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private BackgroundWorker bwtest;
        private ObservableCollection<string> _extras = new ObservableCollection<string>();
        private ObservableCollection<string> _names = new ObservableCollection<string>();
        private ObservableCollection<string> _singlemessage = new ObservableCollection<string>();
        public ObservableCollection<string> Extras
        {
            get { return _extras; }
            set
            {
                if (value != _extras)
                {
                    _extras = value;
                    OnPropertyChanged("Extras");
                }
            }
        }

        public ObservableCollection<string> Names
        {
            get { return _names; }
            set
            {
                if (value != _names)
                {
                    _names = value;
                    OnPropertyChanged("Names");
                }
            }
        }


        public ObservableCollection<string> SingleMessage
        {
            get { return _singlemessage; }
            set
            {
                if (value != _singlemessage)
                {
                    _singlemessage = value;
                    OnPropertyChanged("SingleMessage");
                }
            }
        }

        public MainWindow()
        {
            _extras.Add("No puedes dejar el campo 'Nombre' en blanco.");
            _extras.Add("El campo 'edad' tiene el formato incorrecto.");
            _extras.Add("El RFC ya existe.");
            _extras.Add("Debes de inroducir al menos un valor para el campo 'Nombre comercial'");
            _extras.Add("Algo salió mal, contacta al administrador");

            _singlemessage.Add("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel gravida nunc. Curabitur facilisis, neque nec ornare aliquet, arcu sapien bibendum dolor, sit amet scelerisque enim ante at mi. Mauris pharetra mauris magna, eu accumsan lectus tempus at.");
            InitializeComponent();
            this.DataContext = this;
            
            bwtest = new BackgroundWorker();
            bwtest.DoWork += bwtest_DoWork;
            bwtest.RunWorkerCompleted += bwtest_RunWorkerCompleted;
            bwtest.WorkerReportsProgress = true;
            bwtest.ProgressChanged += bwtest_ProgressChanged;
            
            _names.Add("Transferenciasss");
            _names.Add("Vales");
            _names.Add("Tarjeta de crédito o débito");
            _names.Add("American Express");
            _names.Add("Efectivo");
            _names.Add("Otro");            

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
            //SuccessMessage.ItemsSource = Extras;
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
            SuccessMessage.IsActive = true;
            ErrorMessage.IsActive = true;
            InfoMessage.IsActive = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
