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
    ///     <MyNamespace:CurrencyHintTextBox/>
    ///
    /// </summary>
    public class CurrencyHintTextBox : HintTextBox
    {
        public static readonly DependencyProperty DecimalValueProperty = DependencyProperty.Register("DecimalValue", typeof(decimal), typeof(CurrencyHintTextBox), new PropertyMetadata(null));

        [Description("Decimal value")]
        public decimal DecimalValue
        {
            get { return (decimal)GetValue(DecimalValueProperty); }
            set { SetValue(DecimalValueProperty, value); }
        }

        static CurrencyHintTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyHintTextBox), new FrameworkPropertyMetadata(typeof(CurrencyHintTextBox)));            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Text = DecimalValue.FormatCurrency();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"[\d\.\-]");
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
            Text = Text.RawDecimal();
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);
            decimal Value;
            Decimal.TryParse(Text, out Value);
            DecimalValue = Value;
            Text = DecimalValue.FormatCurrency();
        }
    }
}
