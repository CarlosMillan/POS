using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gestionix.POS.GUI
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

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(StatesButton), new PropertyMetadata(false, new PropertyChangedCallback(OnBussyPropertyChanged)));
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                ExecuteAnimation();
                base.OnKeyDown(e);
            }
            else
                e.Handled = true;
        }

        protected override void OnClick()
        {
            ExecuteAnimation();
            base.OnClick();
        }

        static StatesButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatesButton), new FrameworkPropertyMetadata(typeof(StatesButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (HoverBrush == null) SetValue(HoverBrushProperty, Functions.IncreasedColor((SolidColorBrush)this.Background));

            Ring.Width = this.Height / 2;
            Ring.Height = this.Height / 2;
        }

        private void ExecuteAnimation()
        {
            if (!this.NoAsyncAnimation)
                this.IsBusy = true;
        }
    }
}
