namespace FlexFieldTextBoxWF;

/// <summary>
/// Determines the styles permitted in numeric string arguments for the
/// field.
/// </summary>
public enum ValueFormat
{
    /// <summary>
    /// Indicates that the numeric string represents a decimal value. Valid
    /// decimal values include the numeric digits 0-9.
    /// </summary>
    Decimal,
    /// <summary>
    /// Indicates that the numeric string represents a hexadecimal value.
    /// Valid hexadecimal values include the numeric digits 0-9 and the
    /// hexadecimal digits A-F and a-f.
    /// </summary>
    Hexadecimal
}