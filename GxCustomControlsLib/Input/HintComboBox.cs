﻿using System;
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

        protected string OldFilter = String.Empty;
        protected string Filter = String.Empty;

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
            if (Filter.Length == 0)
            {
                return true;
            }

            // Case insensitive search
            int Contains = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(value.ToString(), Filter, System.Globalization.CompareOptions.OrdinalIgnoreCase);
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
            }
            else
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    // Arrow Down -> Open DropDown
                    if (!this.IsDropDownOpen)
                        this.IsDropDownOpen = true;
                    else
                    {
                        if (e.Key == Key.Up)
                            SelectUpItem();
                        else if (e.Key == Key.Down)
                            SelectDownItem();
                    }
                }

                base.OnPreviewKeyDown(e);
            }

            OldFilter = this.Text;
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
            if (OldFilter != this.Text)
            {
                if (this.Text.Length > 0)
                {
                    Filter = this.Text;
                    AsyncFilter();
                }

                base.OnKeyUp(e);
            }
        }

        public async void AsyncFilter()
        {
            this.RefreshFilter();

            if(this.Items.Count > 0)
                this.IsDropDownOpen = true;

            if (this.EditableTextBox != null)
                this.EditableTextBox.SelectionStart = this.Text.Length;
        }

        /// <summary>
        /// Make sure the text corresponds to the selection when leaving the control.
        /// </summary>
        /// <param name="e">A KeyBoardFocusChangedEventArgs.</param>
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.IsEditable)
            {
                if (e.KeyboardDevice.FocusedElement.GetType() == typeof(TextBox))
                {
                    if (this.SelectedIndex == -1)
                    {
                        Filter = String.Empty;
                        this.Text = String.Empty;
                    }

                    this.IsDropDownOpen = false;
                    //this.ClearFilter();
                    //int temp = this.SelectedIndex;
                    //this.SelectedIndex = -1;
                    //this.Text = String.Empty;
                    //this.SelectedIndex = temp;
                }
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (this.SelectedIndex > -1)
            {
                if (this.EditableTextBox != null)
                    this.EditableTextBox.Text = this.SelectedValue.ToString();
                else base.OnSelectionChanged(e);
            }
            else this.EditableTextBox.Text = Filter;

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


        private void SelectDownItem()
        {
            int DownIndex = this.SelectedIndex + 1;

            if (DownIndex >= this.Items.Count)
                DownIndex = -1;

            this.SelectedIndex = DownIndex;            
        }

        private void SelectUpItem()
        {
            int UpIndex = this.SelectedIndex - 1;

            if (UpIndex < -1)
                UpIndex = this.Items.Count - 1;

            this.SelectedIndex = UpIndex;
        }
    }
}
