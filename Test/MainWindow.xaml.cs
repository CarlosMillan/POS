﻿using Gestionix.POS;
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
            bwtest.WorkerReportsProgress = true;
            bwtest.ProgressChanged += bwtest_ProgressChanged;
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
            //Keyboard.Focus(TxtSeatch);
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
    }
}
