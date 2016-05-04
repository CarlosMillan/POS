using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Gestionix.POS.GUI
{
    /// <summary>
    /// Interaction logic for ModalPopup.xaml
    /// </summary>
    public partial class UCModalPopup : UserControl
    {
        #region Properties
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(UCModalPopup), new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChangedProperty)));
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(UCModalPopup), new PropertyMetadata(null));
        /// <summary>
        /// Set a UIElement as element will be behind of ModalPopup
        /// </summary>
        public UIElement Target
        {
            get { return (UIElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        #endregion

        #region Changed events
        private static void IsActiveChangedProperty(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UCModalPopup M = (o as UCModalPopup);

            if (M.Target != null)
            {
                if (M.IsActive)
                {
                    M.Visibility = Visibility.Visible;
                    M.FadeInAnimation(.3f);
                    M.Target.IsEnabled = false;
                }
                else
                {
                    M.FadeOutAnimation(.3f,
                                       new EventHandler((s, er) =>
                                       {
                                           M.Visibility = Visibility.Collapsed;
                                           M.Target.IsEnabled = true;
                                       }));

                }
            }
            else
                throw new Exception("Dependency property Target must be assigned");
        }
        #endregion

        #region Methods
        private void FadeInAnimation(float secondsduration, EventHandler oncompleteanimation = null)
        {
            FadeAnimation(0, .7f, secondsduration, oncompleteanimation);
        }

        private void FadeOutAnimation(float secondsduration, EventHandler oncompleteanimation = null)
        {
            FadeAnimation(.7f, 0, secondsduration, oncompleteanimation);
        }

        private void FadeAnimation(float from, float to, float secondsduration, EventHandler oncompleteanimation = null)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = from;
            da.To = to;
            da.Duration = new Duration(TimeSpan.FromSeconds(secondsduration));

            if (oncompleteanimation != null && oncompleteanimation is EventHandler)
                da.Completed += oncompleteanimation;

            this.Background.BeginAnimation(Brush.OpacityProperty, da);
        }
        #endregion

        public UCModalPopup()
        {
            InitializeComponent();
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
