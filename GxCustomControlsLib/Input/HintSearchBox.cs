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
        public static readonly DependencyProperty IsSearchingProperty = DependencyProperty.Register("IsSearching", typeof(bool), typeof(HintSearchBox), new PropertyMetadata(false));
        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HintSearchBox));
        
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

        public event RoutedEventHandler Search
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }

        static HintSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintSearchBox), new FrameworkPropertyMetadata(typeof(HintSearchBox)));            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button CloseActionButton = this.GetTemplateChild("PART_SecundaryIcon") as Button;
            Button SearchActionButton = this.GetTemplateChild("PART_PrimaryIcon") as Button;
            CloseActionButton.Click += CloseActionButton_Click;
            SearchActionButton.Click += SearchActionButton_Click;
        }

        private void SearchActionButton_Click(object sender, RoutedEventArgs e)
        {
            IsSearching = true;
            RaiseEvent(new RoutedEventArgs(HintSearchBox.SearchEvent));
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
            {
                IsSearching = true;
                RaiseEvent(new RoutedEventArgs(HintSearchBox.SearchEvent));
            }
        }
    }    
}
