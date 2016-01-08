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
    public class HintCurrencyBox : HintNumericBox
    {
        static HintCurrencyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintCurrencyBox), new FrameworkPropertyMetadata(typeof(HintCurrencyBox)));
        }

        public override void OnApplyTemplate()
        {
            if (Scale == 0) SetValue(ScaleProperty, 2);
            Text = ValidateInput(Text, FormatCurrencyInput);
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
