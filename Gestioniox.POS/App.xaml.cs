using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Gestionix.POS.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoadResources();
        }

        #region Helpers
        private void LoadResources()
        {
            try
            {
                if (POSResources.Strings == null)
                {
                    POSResources.Strings = new System.Windows.ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/Resources/Dictionaries/Dictionary-ES-MX.xaml")
                    };
                }

                if (POSResources.SolidColors == null)
                {
                    POSResources.SolidColors = new System.Windows.ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/Resources/Dictionaries/Colors.xaml")
                    };
                }
            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }
        }
        #endregion
    }
}
