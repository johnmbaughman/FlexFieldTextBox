namespace FlexFieldTextBoxWF.Controls;

public class SSNTextBox : FlexFieldTextBox
{
    public SSNTextBox()
    {
        // the format of this control is '999-99-9999'

        // set FieldCount first
        //
        FieldCount = 3;

        // first field is 3 digits...
        //
        SetMaxLength(0, 3);

        // second field is 2 digits...
        //
        SetMaxLength(1, 2);

        // last field is 4 digits
        //
        SetMaxLength(2, 4);

        // every separator is '-'...
        //
        SetSeparatorText("-");

        // except for the first and last separators
        //
        SetSeparatorText(0, string.Empty);
        SetSeparatorText(FieldCount, string.Empty);

        // use leading zeros on every field
        //
        SetLeadingZeros(true);

        // add '-' keys to cede focus for every field
        //
        var e = new KeyEventArgs(Keys.OemMinus);
        AddCedeFocusKey(e);
        e = new KeyEventArgs(Keys.Subtract);
        AddCedeFocusKey(e);

        // this should be the last thing
        //
        Size = MinimumSize;
    }
}