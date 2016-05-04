using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Gestionix.POS.GUI
{
    public class HintNumericBox : HintTextBox
    {
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(int), typeof(HintNumericBox), new PropertyMetadata(2));        

        [Description("Scale decimal")]
        public int Scale
        {
            get { return (int)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        static HintNumericBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintNumericBox), new FrameworkPropertyMetadata(typeof(HintNumericBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();            
            Text = ValidateInput(Text, FormatNumberInput);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"[\d\.\-]");
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
            GotFocus(Text.DecimalFormatToDecimal());
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);            
            Text = ValidateInput(Text, FormatNumberInput);
        }

        protected string ValidateInput(string input, Func<string, decimal, string> convertfunc)
        {
            return convertfunc(input, input.DecimalFormatToDecimal());
        }

        protected new void GotFocus(decimal value)
        {
            if (!String.IsNullOrWhiteSpace(Text))
            {
                decimal DecimalValue = value;
                string NewString = DecimalValue.FormatDecimal();

                if (Text != NewString)
                    Text = NewString;
            }
        }

        private string FormatNumberInput(string a, decimal v)
        {
            if (!String.IsNullOrEmpty(a))
                return v.FormatNumber(Scale);
            else return a;
        }
    }
}
