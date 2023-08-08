using System;
using System.Windows;
using System.Windows.Controls;

namespace FlexFieldTextBoxWPF;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:FlexFieldTextBoxWPF"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:FlexFieldTextBoxWPF;assembly=FlexFieldTextBoxWPF"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:CustomControl1/>
///
/// </summary>
public class FlexFieldTextBox : UserControl
{
    static FlexFieldTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexFieldTextBox), new FrameworkPropertyMetadata(typeof(FlexFieldTextBox)));
    }

    #region Events

    /// <summary>
    /// Raised when the text of any field changes.
    /// </summary>
    public event EventHandler<FieldChangedEventArgs> FieldChangedEvent;
    /// <summary>
    /// Raised when the text of any field is validated, such as when focus
    /// changes from one field to another.
    /// </summary>
    public event EventHandler<FieldValidatedEventArgs> FieldValidatedEvent;

    #endregion

    #region

    public static readonly DependencyProperty AllowInternalTabProperty = DependencyProperty.Register("AllowInternalTab", typeof(bool), typeof(FlexFieldTextBox));

    /// <summary>
    /// Gets or sets a value indicating whether the control allows the [Tab]
    /// key to index the fields within the control.
    /// </summary>
    public bool AllowInternalTab
    {
        get => (bool)GetValue(AllowInternalTabProperty);
        set => SetValue(AllowInternalTabProperty, value);
    }

    public static readonly DependencyProperty FieldCountProperty = DependencyProperty.Register("FieldCount", typeof(int), typeof(FlexFieldTextBox));

    /// <summary>
    /// Gets or sets the number of fields in the control. Default is 3;
    /// minimum is 1. Setting this value resets every field and separator
    /// to its default state.
    /// </summary>
    public int FieldCount
    {
        get => (int) GetValue(FieldCountProperty);
        set => SetValue(FieldCountProperty, value);
    }

    public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(FlexFieldTextBox));

    /// <summary>
    /// Gets or sets a value indicating whether the contents of the control
    /// can be changed.
    /// </summary>
    public bool ReadOnly
    {
        get => (bool)GetValue(ReadOnlyProperty);
        set => SetValue(ReadOnlyProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FlexFieldTextBox));

    /// <summary>
    /// Gets or sets the text of the control.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion
}

internal class FieldControl : TextBox
{
    #region Public Events

    public event EventHandler<CedeFocusEventArgs> CedeFocusEvent;
    public event EventHandler<FieldChangedEventArgs> FieldChangedEvent;
    public event EventHandler FieldSizeChangedEvent;
    public event EventHandler<FieldValidatedEventArgs> FieldValidatedEvent;

    #endregion  // Public Events
}

