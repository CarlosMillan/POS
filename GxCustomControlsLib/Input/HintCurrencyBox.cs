using System;
using System.Windows;
using System.Windows.Input;

namespace Gestionix.POS.GUI
{
    public class HintCurrencyBox : HintNumericBox
    {
        static HintCurrencyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintCurrencyBox), new FrameworkPropertyMetadata(typeof(HintCurrencyBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Text = ValidateInput(Text, FormatCurrencyInput);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            GotFocus(Text.CurrencyFormatToDecimal());
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Text = ValidateInput(Text, FormatCurrencyInput);
        }

        private string FormatCurrencyInput(string a, decimal v)
        {
            if (!String.IsNullOrEmpty(a))
                return v.FormatCurrency(Scale);
            else return a;
        }
    }
}
