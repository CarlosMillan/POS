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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gestionix.POS
{
    /// <summary>
    /// Interaction logic for ModalPopup.xaml
    /// </summary>
    public partial class ModalPopup : UserControl
    {
        #region Properties
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ModalPopup), new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChangedProperty)));
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(ModalPopup), new PropertyMetadata(null));
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
            ModalPopup M = (o as ModalPopup);

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

        //private static void TargetChangedProperty(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    ModalPopup M = (o as ModalPopup);

        //}
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

            this.BeginAnimation(OpacityProperty, da);
        }
        #endregion

        public ModalPopup()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
