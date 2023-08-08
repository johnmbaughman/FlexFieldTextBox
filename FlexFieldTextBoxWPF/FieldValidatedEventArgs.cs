namespace FlexFieldTextBoxWPF;

public class FieldValidatedEventArgs
{
    /// <summary>
    /// Gets or sets index for field that is raising FieldValidatedEvent.
    /// </summary>
    public int FieldIndex { get; set; }

    /// <summary>
    /// Gets or sets text for field that is raising FieldValidatedEvent.
    /// </summary>
    public string Text { get; set; }
}