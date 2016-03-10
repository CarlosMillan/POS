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
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Gestionix.POS"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Gestionix.POS;assembly=Gestionix.POS"
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
    ///     <MyNamespace:HintTextBox/>
    ///
    /// </summary>
    public class HintTextBox : TextBox
    {
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(HintTextBox), new PropertyMetadata(null));
        public static readonly DependencyProperty HintTextBrushProperty = DependencyProperty.Register("HintTextBrush", typeof(Brush), typeof(HintTextBox), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty HintTextVerticalAligmentProperty = DependencyProperty.Register("HintTextVerticalAligment", typeof(AlignmentY), typeof(HintTextBox), new PropertyMetadata(AlignmentY.Center));
        public static readonly DependencyProperty HintTextHorizontalAligmentProperty = DependencyProperty.Register("HintTextHorizontalAligment", typeof(AlignmentX), typeof(HintTextBox), new PropertyMetadata(AlignmentX.Left));
        public static readonly DependencyProperty BindableSelectionStartProperty = DependencyProperty.Register("BindableSelectionStart",typeof(int),typeof(HintTextBox),new PropertyMetadata(OnBindableSelectionStartChanged));
        public static readonly DependencyProperty BindableSelectionLengthProperty =DependencyProperty.Register("BindableSelectionLength",typeof(int),typeof(HintTextBox),new PropertyMetadata(OnBindableSelectionLengthChanged));

        private bool changeFromUI;

        [Description("Suggested text")]
        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        public Brush HintTextBrush
        {
            get { return (Brush)GetValue(HintTextBrushProperty); }
            set { SetValue(HintTextBrushProperty, value); }
        }

        public AlignmentY HintTextVerticalAligment
        {
            get { return (AlignmentY)GetValue(HintTextVerticalAligmentProperty); }
            set { SetValue(HintTextVerticalAligmentProperty, value); }
        }

        public AlignmentX HintTextHorizontalAligment
        {
            get { return (AlignmentX)GetValue(HintTextHorizontalAligmentProperty); }
            set { SetValue(HintTextHorizontalAligmentProperty, value); }
        }

        public int BindableSelectionStart
        {
            get { return (int)this.GetValue(BindableSelectionStartProperty); }
            set{ this.SetValue(BindableSelectionStartProperty, value);}
        }

        public int BindableSelectionLength
        {
            get{ return (int)this.GetValue(BindableSelectionLengthProperty);}
            set{this.SetValue(BindableSelectionLengthProperty, value);}
        }
         
        static HintTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintTextBox), new FrameworkPropertyMetadata(typeof(HintTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            #region Adjust HintText position
            if (this.VerticalContentAlignment == System.Windows.VerticalAlignment.Top)
                SetValue(HintTextVerticalAligmentProperty, AlignmentY.Top);
            else if (this.VerticalContentAlignment == System.Windows.VerticalAlignment.Bottom)
                SetValue(HintTextVerticalAligmentProperty, AlignmentY.Bottom);
            else if (this.VerticalContentAlignment == System.Windows.VerticalAlignment.Center)
                SetValue(HintTextVerticalAligmentProperty, AlignmentY.Center);

            if (this.TextAlignment == System.Windows.TextAlignment.Left)
                SetValue(HintTextHorizontalAligmentProperty, AlignmentX.Left);
            else if (this.TextAlignment == System.Windows.TextAlignment.Center)
                SetValue(HintTextHorizontalAligmentProperty, AlignmentX.Center);
            else if (this.TextAlignment == System.Windows.TextAlignment.Right)
                SetValue(HintTextHorizontalAligmentProperty, AlignmentX.Right);
            #endregion

            this.SelectionChanged += this.OnSelectionChanged;
        }

        private static void OnBindableSelectionStartChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var textBox = dependencyObject as HintTextBox;

            if (!textBox.changeFromUI)
            {
                int newValue = (int)args.NewValue;
                textBox.SelectionStart = newValue;
            }
            else
            {
                textBox.changeFromUI = false;
            }
        }

        private static void OnBindableSelectionLengthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var textBox = dependencyObject as HintTextBox;

            if (!textBox.changeFromUI)
            {
                int newValue = (int)args.NewValue;
                textBox.SelectionLength = newValue;
            }
            else
            {
                textBox.changeFromUI = false;
            }
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.BindableSelectionStart != this.SelectionStart)
            {
                this.changeFromUI = true;
                this.BindableSelectionStart = this.SelectionStart;
            }

            if (this.BindableSelectionLength != this.SelectionLength)
            {
                this.changeFromUI = true;
                this.BindableSelectionLength = this.SelectionLength;
            }
        }
    }
}
