using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        protected ToggleButton ToggleButton
        {
            get
            {
                return this.GetTemplateChild("PART_ToggleButton") as ToggleButton;
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
            int Contains = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(value.ToString(), this.Text, System.Globalization.CompareOptions.OrdinalIgnoreCase);
            return Contains >= 0 ? true : false;        
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
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    // Arrow Down -> Open DropDown
                    if (!this.IsDropDownOpen)
                        this.IsDropDownOpen = true;

                    //this.SelectedIndex = 0;
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
            if (e.Key == Key.Tab || e.Key == Key.Enter)
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
                        //this.RefreshFilter();
                        //this.IsDropDownOpen = true;

                        //// Unselect
                        //this.EditableTextBox.SelectionStart = int.MaxValue;
                        AsyncFilter();
                    }
                    else
                        this.IsDropDownOpen = false;
                }

                base.OnKeyUp(e);

                // Update Filter Value
                this.currentFilter = this.Text;
            }
        }

        public async void AsyncFilter()
        {
            this.RefreshFilter();

            if(this.Items.Count > 0)
                this.IsDropDownOpen = true;

             //Unselect
            this.EditableTextBox.SelectionStart = int.MaxValue;
        }

        /// <summary>
        /// Make sure the text corresponds to the selection when leaving the control.
        /// </summary>
        /// <param name="e">A KeyBoardFocusChangedEventArgs.</param>
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.IsEditable)
            {
                this.ClearFilter();
                int temp = this.SelectedIndex;
                this.SelectedIndex = -1;
                this.Text = String.Empty;
                this.SelectedIndex = temp;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (this.SelectedIndex > -1)
                this.EditableTextBox.Text = this.SelectedValue.ToString();

            this.EditableTextBox.SelectionStart = int.MaxValue;
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
