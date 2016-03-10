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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gestionix.POS
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
