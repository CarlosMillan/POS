using System;
using System.Windows;
using System.Windows.Input;

namespace Gestionix.POS.GUI
{
    public class HintPercentageBox : HintNumericBox
    {
        static HintPercentageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintPercentageBox), new FrameworkPropertyMetadata(typeof(HintPercentageBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Text = ValidateInput(Text, FormatPercentageInput);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            GotFocus(Text.PercentageFormatToDecimal());
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Text = ValidateInput(Text, FormatPercentageInput);
        }

        private string FormatPercentageInput(string a, decimal v)
        {
            if (!String.IsNullOrEmpty(a))
                return v.FormatPercentage(Scale);
            else return a;
        }
    }
}
