using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Gestionix.POS
{
    public class HintSearchBox : HintTextBox
    {
        public static readonly DependencyProperty IsSearchingProperty = DependencyProperty.Register("IsSearching", typeof(bool), typeof(HintSearchBox), new FrameworkPropertyMetadata(OnIsSearchingPropertyChanged));
        
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

        static HintSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintSearchBox), new FrameworkPropertyMetadata(typeof(HintSearchBox)));            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button CloseActionButton = this.GetTemplateChild("PART_SecundaryIcon") as Button;            
            CloseActionButton.Click += CloseActionButton_Click;
        }

        private void CloseActionButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox TextBoxSearch = this.GetTemplateChild("PART_Writeable") as TextBox;
            TextBoxSearch.Clear();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Enter)            
                IsSearching = true;            
        }

        private static void OnIsSearchingPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            //if (!Boolean.Parse(e.NewValue.ToString()))
            //    ((HintTextBox)source).IsReadOnly = false;
        }
    }    
}
