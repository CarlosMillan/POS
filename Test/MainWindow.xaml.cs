using Gestionix.POS;
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

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker bwtest;

        public MainWindow()
        {
            InitializeComponent();
            bwtest = new BackgroundWorker();
            bwtest.DoWork += bwtest_DoWork;
            bwtest.RunWorkerCompleted += bwtest_RunWorkerCompleted;
        }

        void bwtest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HintSearchBox T = (HintSearchBox)e.Result;
            T.IsSearching = false;
        }

        void bwtest_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(5000);
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
                bwtest.RunWorkerAsync(t);
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            RingTest.IsActivate = ((System.Windows.Controls.Primitives.ToggleButton)e.Source).IsChecked ?? false;
        }
    }
}
