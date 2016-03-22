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

namespace Gestionix.POS
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Gestionix.POS.Buttons"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Gestionix.POS.Buttons;assembly=Gestionix.POS.Buttons"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Button/>
    ///
    /// </summary>
    public class StatesButton : Button
    {
        public ProgressRing Ring
        {
            get { return this.GetTemplateChild("PART_Loading") as ProgressRing; }
        }

        public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register("IsLocked", typeof(bool), typeof(StatesButton), new PropertyMetadata(new PropertyChangedCallback(OnLockedPropertyChanged)));
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        public static readonly DependencyProperty IsBussyProperty = DependencyProperty.Register("IsBussy", typeof(bool), typeof(StatesButton), new PropertyMetadata(new PropertyChangedCallback(OnBussyPropertyChanged)));
        public bool IsBussy
        {
            get { return (bool)GetValue(IsBussyProperty); }
            set { SetValue(IsBussyProperty, value); }
        }

        private static void OnLockedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            StatesButton B = (o as StatesButton);

            if (!B.IsBussy)
            {
                if (B.IsLocked) B.IsEnabled = false;
                else B.IsEnabled = true;
            }
        }

        private static void OnBussyPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            StatesButton B = (o as StatesButton);

            if (B.IsBussy)
            {
                B.IsEnabled = false;
                B.Ring.IsActivated = true;
            }
            else
            {
                B.IsEnabled = true;
                B.Ring.IsActivated = false;
            }
        }

        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
            ExecuteClick();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            ExecuteClick();
        }

        static StatesButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatesButton), new FrameworkPropertyMetadata(typeof(StatesButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();            
            Ring.Width = this.Height / 2;
            Ring.Height = this.Height / 2;
        }

        private void ExecuteClick()
        {
            this.IsBussy = true;
            this.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); 
        }
    }
}
