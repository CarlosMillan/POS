using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InformationMessage.xaml
    /// </summary>
    public partial class InformationMessage : UserControl
    {
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

        private ObservableCollection<string> _messagecontent;
        public ObservableCollection<string> MessageContent
        {
            get { return _messagecontent; }
        }

        private bool _showbullets;
        public bool ShowBullets
        {
            get { return _showbullets; }            
        }

        public InformationMessage()
        {
            _messagecontent = new ObservableCollection<string>();            
            //_messagecontent.Add("Probando Error 1");
            //_messagecontent.Add("Probando Error 2");
            //_messagecontent.Add("Probando Error 3");
            //_messagecontent.Add("Probando Error 4");
            _messagecontent.Add("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce eget velit mattis, ornare urna at, mollis metus. Sed sed justo posuere odio rutrum viverra sagittis in ligula. Nullam sollicitudin velit eros.");
            //_messagecontent.Add("nnaa !");
            InitializeComponent();
            this.DataContext = this;
            _showbullets = _messagecontent.Count > 1;
            SetInformationMessageColor();            
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
            (sender as Gestionix.POS.InformationMessage).SetInformationMessageColor();
        }
    }

    public enum InformationMessageType
    {
        Error,
        Success,
        Informative        
    }
}
