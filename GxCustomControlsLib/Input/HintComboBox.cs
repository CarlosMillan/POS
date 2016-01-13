using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gestionix.POS
{
    public class HintComboBox : ComboBox
    {
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(HintComboBox), new PropertyMetadata(null));

        [Description("Suggested text")]
        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        /// <summary>
        /// Caches the previous value of the filter.
        /// </summary>
        private string oldFilter = string.Empty;

        /// <summary>
        /// Holds the current value of the filter.
        /// </summary>
        private string currentFilter = string.Empty;

        /// <summary>
        /// Gets a reference to the internal editable textbox.
        /// </summary>
        /// <value>A reference to the internal editable textbox.</value>
        /// <remarks>
        /// We need this to get access to the Selection.
        /// </remarks>
        protected TextBox EditableTextBox
        {
            get
            {
                return this.GetTemplateChild("PART_EditableTextBox") as TextBox;
            }
        }

        
        static HintComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintComboBox), new FrameworkPropertyMetadata(typeof(HintComboBox)));
        }


        /// <summary>
        /// Keep the filter if the ItemsSource is explicitly changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the filter.</param>
        /// <param name="newValue">The current value of the filter.</param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(newValue);
                view.Filter += this.FilterPredicate;
            }

            if (oldValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);
                view.Filter -= this.FilterPredicate;
            }

            base.OnItemsSourceChanged(oldValue, newValue);
        }


        /// <summary>
        /// The Filter predicate that will be applied to each row in the ItemsSource.
        /// </summary>
        /// <param name="value">A row in the ItemsSource.</param>
        /// <returns>Whether or not the item will appear in the DropDown.</returns>
        private bool FilterPredicate(object value)
        {
            // No filter, no text
            if (value == null)
            {
                return false;
            }

            // No text, no filter
            if (this.Text.Length == 0)
            {
                return true;
            }

            // Case insensitive search     
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-MX");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-MX");            
            int aa = string.Compare("MillÁn ñ", "mIllan ñ", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.CompareOptions.IgnoreNonSpace | System.Globalization.CompareOptions.IgnoreCase);
            bool ys = CompareIgnoreAccents("MilLán n", "millan ñ");
            return value.ToString().ToLower().RemoveDiacritics().Contains(this.Text.ToLower().RemoveDiacritics());            
        }

        private static bool CompareIgnoreAccents(string s1, string s2)
        {
            string s_1 = RemoveAccents(s1);
            string s_2 = RemoveAccents(s2);
            return string.Compare(
                s_1, s_2, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        private static string RemoveAccents(string s)
        {
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");

            return destEncoding.GetString(
                Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(s)));
        }


        /// <summary>
        /// Confirm or cancel the selection when Tab, Enter, or Escape are hit. 
        /// Open the DropDown when the Down Arrow is hit.
        /// </summary>
        /// <param name="e">Key Event Args.</param>
        /// <remarks>
        /// The 'KeyDown' event is not raised for Arrows, Tab and Enter keys.
        /// It is swallowed by the DropDown if it's open.
        /// So use the Preview instead.
        /// </remarks>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                // Explicit Selection -> Close ItemsPanel
                this.IsDropDownOpen = false;
            }
            else if (e.Key == Key.Escape)
            {
                // Escape -> Close DropDown and redisplay Filter
                this.IsDropDownOpen = false;
                this.SelectedIndex = -1;
                this.Text = this.currentFilter;
            }
            else
            {
                if (e.Key == Key.Down)
                {
                    // Arrow Down -> Open DropDown
                    this.IsDropDownOpen = true;
                }

                base.OnPreviewKeyDown(e);
            }

            // Cache text
            this.oldFilter = this.Text;
        }

        /// <summary>
        /// Modify and apply the filter.
        /// </summary>
        /// <param name="e">Key Event Args.</param>
        /// <remarks>
        /// Alternatively, you could react on 'OnTextChanged', but navigating through 
        /// the DropDown will also change the text.
        /// </remarks>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                
            }
            else if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                // Explicit Select -> Clear Filter
                this.ClearFilter();
            }
            else
            {
                // The text was changed
                if (this.Text != this.oldFilter)
                {
                    // Clear the filter if the text is empty,
                    // apply the filter if the text is long enough
                    if (this.Text.Length > 0)
                    {
                        this.RefreshFilter();
                        this.IsDropDownOpen = true;

                        // Unselect
                        this.EditableTextBox.SelectionStart = int.MaxValue;
                    }
                }

                base.OnKeyUp(e);

                // Update Filter Value
                this.currentFilter = this.Text;
            }
        }

        /// <summary>
        /// Make sure the text corresponds to the selection when leaving the control.
        /// </summary>
        /// <param name="e">A KeyBoardFocusChangedEventArgs.</param>
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.IsTextSearchEnabled)
            {
                this.ClearFilter();
                int temp = this.SelectedIndex;
                this.SelectedIndex = -1;
                this.Text = string.Empty;
                this.SelectedIndex = temp;
            }

            base.OnPreviewLostKeyboardFocus(e);
        }

        ////
        // Helpers
        ////

        /// <summary>
        /// Re-apply the Filter.
        /// </summary>
        private void RefreshFilter()
        {
            if (this.ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource);
                view.Refresh();
            }
        }

        /// <summary>
        /// Clear the Filter.
        /// </summary>
        private void ClearFilter()
        {
            this.currentFilter = string.Empty;
            this.RefreshFilter();
        } 
    }
}
