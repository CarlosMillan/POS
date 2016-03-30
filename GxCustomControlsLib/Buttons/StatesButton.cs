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
        public WaitingRing Ring
        {
            get { return this.GetTemplateChild("PART_Loading") as WaitingRing; }
        }

        public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register("IsLocked", typeof(bool), typeof(StatesButton), new PropertyMetadata(new PropertyChangedCallback(OnLockedPropertyChanged)));
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(StatesButton), new PropertyMetadata(new PropertyChangedCallback(OnBussyPropertyChanged)));
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty NoAsyncAnimationProperty = DependencyProperty.Register("NoAsyncAnimation", typeof(bool), typeof(StatesButton), new PropertyMetadata(false));
        public bool NoAsyncAnimation
        {
            get { return (bool)GetValue(NoAsyncAnimationProperty); }
            set { SetValue(NoAsyncAnimationProperty, value); }
        }

        public static readonly DependencyProperty HoverBrushProperty = DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(StatesButton), new PropertyMetadata(null));
        public Brush HoverBrush
        {
            get { return (Brush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }

        private static void OnLockedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            StatesButton B = (o as StatesButton);

            if (!B.IsBusy)
            {
                if (B.IsLocked) B.IsEnabled = false;
                else B.IsEnabled = true;
            }
        }

        private static void OnBussyPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            StatesButton B = (o as StatesButton);

            if (B.IsBusy)
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
            ExecuteAnimation();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            ExecuteAnimation();            
        }

        static StatesButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatesButton), new FrameworkPropertyMetadata(typeof(StatesButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if(HoverBrush == null) SetValue(HoverBrushProperty, IncreasedColor((SolidColorBrush)this.Background));

            Ring.Width = this.Height / 2;
            Ring.Height = this.Height / 2;
        }

        private void ExecuteAnimation()
        {
            if (!this.NoAsyncAnimation)
            {
                this.IsBusy = true;
                this.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private Brush IncreasedColor(SolidColorBrush basecolor)
        {
            float IncrementPercentage = .20f;
            int[] RGB = new int[3];
            RGB[0] = basecolor.Color.R;
            RGB[1] = basecolor.Color.G;
            RGB[2] = basecolor.Color.B;

            RGB[0] += (int)(RGB[0] * IncrementPercentage);
            RGB[1] += (int)(RGB[1] * IncrementPercentage);
            RGB[2] += (int)(RGB[2] * IncrementPercentage);

            RGB[0] = RGB[0] > 255 ? 255 : RGB[0];
            RGB[1] = RGB[1] > 255 ? 255 : RGB[1];
            RGB[2] = RGB[2] > 255 ? 255 : RGB[2];
            
            return new SolidColorBrush(Color.FromRgb((byte)RGB[0], (byte)RGB[1], (byte)RGB[2]));
        }
    }
}
