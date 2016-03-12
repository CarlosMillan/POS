using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gestionix.POS
{
    /// <summary>
    /// Interaction logic for InformationMessage.xaml
    /// </summary>
    public partial class InformationMessage : UserControl, INotifyPropertyChanged
    {
        #region Const
        private const float FADE_ANIMATION_DURATION = 0.5f;  // Time in seconds
        #endregion

        #region Properties
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(InformationMessageType), typeof(InformationMessage), new PropertyMetadata(InformationMessageType.Error, new PropertyChangedCallback(OnTypePropertyChanged)));
        public InformationMessageType Type
        {
            get { return (InformationMessageType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty InformationMessageBrushProperty = DependencyProperty.Register("InformationMessageBrush", typeof(Brush), typeof(InformationMessage), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        public Brush InformationMessageBrush 
        {
            get { return (Brush)GetValue(InformationMessageBrushProperty); }
            set { SetValue(InformationMessageBrushProperty, value); }
        }

        public static readonly DependencyProperty InformationMessageBackgroundBrushProperty = DependencyProperty.Register("InformationMessageBackgroundBrush", typeof(Brush), typeof(InformationMessage), new PropertyMetadata(new SolidColorBrush(Colors.LightPink)));
        public Brush InformationMessageBackgroundBrush
        {
            get { return (Brush)GetValue(InformationMessageBackgroundBrushProperty); }
            set { SetValue(InformationMessageBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty InformationMessageIconProperty = DependencyProperty.Register("InformationMessageIcon", typeof(ControlTemplate), typeof(InformationMessage), new PropertyMetadata(null, new PropertyChangedCallback(OnTypePropertyChanged)));
        public ControlTemplate InformationMessageIcon
        {
            get { return (ControlTemplate)GetValue(InformationMessageIconProperty); }
            set { SetValue(InformationMessageIconProperty, value); }
        }

        public static readonly DependencyProperty MessageContentProperty = DependencyProperty.Register("MessageContent", typeof(List<string>), typeof(InformationMessage), new PropertyMetadata(null));
        public List<string> MessageContent
        {
            get { return (List<string>)GetValue(MessageContentProperty); }
            set { SetValue(MessageContentProperty, value); }
        }

        private bool _showbullets;
        public bool ShowBullets
        {
            get { return _showbullets; }            
        }
        #endregion

        #region Ctors
        public InformationMessage()
        {
            
            InitializeComponent();
            this.DataContext = this;
            //_showbullets = _messagecontent.Count > 1;
            SetInformationMessageColor();            
        }
        #endregion

        #region Methods and Events
        public void SetInformationMessageColor()
        {
            if (Type == InformationMessageType.Error)
            {
                InformationMessageBrush = this.FindResource("Red1") as SolidColorBrush;
                InformationMessageBackgroundBrush = this.FindResource("Red2") as SolidColorBrush;
                InformationMessageIcon = this.FindResource("AlertIcon") as ControlTemplate;
            }
            else if (Type == InformationMessageType.Informative)
            {
                InformationMessageBrush = this.FindResource("Blue1") as SolidColorBrush;
                InformationMessageBackgroundBrush = this.FindResource("Blue2") as SolidColorBrush;
                InformationMessageIcon = this.FindResource("InformativeIcon") as ControlTemplate;                
            }
            else if (Type == InformationMessageType.Success)
            {
                InformationMessageBrush = this.FindResource("Green1") as SolidColorBrush;
                InformationMessageBackgroundBrush = this.FindResource("Green2") as SolidColorBrush;
                InformationMessageIcon = this.FindResource("SuccessIcon") as ControlTemplate;
            }
        }

        private static void OnTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as Gestionix.POS.InformationMessage).SetInformationMessageColor();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FadeOutAnimation(FADE_ANIMATION_DURATION, 
                             new EventHandler((s, er) => 
                             { 
                                 this.Visibility = System.Windows.Visibility.Hidden; 
                             }));
        }

        private void MessageControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)            
                FadeInAnimation(FADE_ANIMATION_DURATION);            
        }

        private void FadeInAnimation(float secondsduration, EventHandler oncompleteanimation = null)
        {
            FadeAnimation(0, 1, secondsduration, oncompleteanimation);
        }

        private void FadeOutAnimation(float secondsduration, EventHandler oncompleteanimation = null)
        {
            FadeAnimation(1, 0, secondsduration, oncompleteanimation);
        }

        private void FadeAnimation(int from, int to, float secondsduration, EventHandler oncompleteanimation =  null)
        {            
            DoubleAnimation da = new DoubleAnimation();
            da.From = from;
            da.To = to;
            da.Duration = new Duration(TimeSpan.FromSeconds(secondsduration));

            if(oncompleteanimation != null && oncompleteanimation is EventHandler)
                da.Completed += oncompleteanimation;

            this.BeginAnimation(OpacityProperty, da);            
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public enum InformationMessageType
    {
        Error,
        Success,
        Informative        
    }
}
