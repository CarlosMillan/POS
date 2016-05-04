using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Gestionix.POS.GUI
{
    /// <summary>
    /// Interaction logic for InformationMessage.xaml
    /// </summary>
    public partial class UCInformationMessage : UserControl, INotifyPropertyChanged
    {
        #region Const
        private const float FADE_ANIMATION_DURATION = 0.4f;  // Time in seconds
        #endregion

        #region Properties
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(InformationMessageType), typeof(UCInformationMessage), new PropertyMetadata(InformationMessageType.Error, new PropertyChangedCallback(OnTypePropertyChanged)));
        public InformationMessageType Type
        {
            get { return (InformationMessageType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(UCInformationMessage), new PropertyMetadata(false, new PropertyChangedCallback(OnIsActivePropertyChanged)));
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty InformationMessageBrushProperty = DependencyProperty.Register("InformationMessageBrush", typeof(Brush), typeof(UCInformationMessage), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        public Brush InformationMessageBrush 
        {
            get { return (Brush)GetValue(InformationMessageBrushProperty); }
            set { SetValue(InformationMessageBrushProperty, value); }
        }

        public static readonly DependencyProperty InformationMessageBackgroundBrushProperty = DependencyProperty.Register("InformationMessageBackgroundBrush", typeof(Brush), typeof(UCInformationMessage), new PropertyMetadata(new SolidColorBrush(Colors.LightPink)));
        public Brush InformationMessageBackgroundBrush
        {
            get { return (Brush)GetValue(InformationMessageBackgroundBrushProperty); }
            set { SetValue(InformationMessageBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty InformationMessageIconProperty = DependencyProperty.Register("InformationMessageIcon", typeof(ControlTemplate), typeof(UCInformationMessage), new PropertyMetadata(null, new PropertyChangedCallback(OnTypePropertyChanged)));
        public ControlTemplate InformationMessageIcon
        {
            get { return (ControlTemplate)GetValue(InformationMessageIconProperty); }
            set { SetValue(InformationMessageIconProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value);}
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(UCInformationMessage), new PropertyMetadata(new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as UCInformationMessage;
            if (control != null)
                control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }

        }

        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Do your stuff here.
        }
                
        private bool _showbullets;
        public bool ShowBullets
        {
            get { return _showbullets; }            
        }
        #endregion

        #region Ctors
        public UCInformationMessage()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Visibility = System.Windows.Visibility.Collapsed;
            SetInformationMessageColor();            
        }
        #endregion

        #region Methods and Events
        public override void OnApplyTemplate()
        {            
            base.OnApplyTemplate();

            if (this.ItemsSource != null && (ItemsSource as ObservableCollection<string>).Count > 1)
            {
                _showbullets = (ItemsSource as ObservableCollection<string>).Count > 1;
                OnPropertyChanged("ShowBullets");
            }
        }

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
            (sender as Gestionix.POS.GUI.UCInformationMessage).SetInformationMessageColor();
        }

        private static void OnIsActivePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UCInformationMessage M = (sender as Gestionix.POS.GUI.UCInformationMessage);

            if (M.IsActive)
            {
                M.Visibility = Visibility.Visible;
                M.FadeInAnimation(FADE_ANIMATION_DURATION);
            }
            else
                M.Visibility = Visibility.Collapsed;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            FadeOutAnimation(FADE_ANIMATION_DURATION, 
                             new EventHandler((s, er) => 
                             {
                                 this.IsActive = false;                                 
                             }));
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
