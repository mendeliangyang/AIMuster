using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace AIMuster.Behaviors
{
    public static class NumberTextBoxInputBehavior
    {
        public static readonly DependencyProperty IsNumericProperty =
            DependencyProperty.RegisterAttached(
                "IsNumeric",
                typeof(bool),
                typeof(NumberTextBoxInputBehavior),
                new UIPropertyMetadata(false, OnIsNumericChanged));

    public static bool GetIsNumeric(DependencyObject obj) => (bool)obj.GetValue(IsNumericProperty);
    public static void SetIsNumeric(DependencyObject obj, bool value) => obj.SetValue(IsNumericProperty, value);

    private static void OnIsNumericChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox)
        {
            if ((bool)e.NewValue)
            {
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                DataObject.AddPastingHandler(textBox, OnPaste);
            }
            else
            {
                textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                DataObject.RemovePastingHandler(textBox, OnPaste);
            }
        }
    }

    private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
    }

    private static void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            var text = (string)e.DataObject.GetData(typeof(string));
            if (!Regex.IsMatch(text, "^[0-9]+$"))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }
}
}
