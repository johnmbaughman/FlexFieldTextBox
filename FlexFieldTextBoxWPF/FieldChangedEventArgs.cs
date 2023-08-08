namespace FlexFieldTextBoxWPF;

public class FieldChangedEventArgs
{
    /// <summary>
    /// Gets or sets index of field that is raising FieldChangedEvent.
    /// </summary>
    public int FieldIndex { get; set; }

    /// <summary>
    /// Gets or sets text of field that is raising FieldChangedEvent.
    /// </summary>
    public string Text { get; set; }
}