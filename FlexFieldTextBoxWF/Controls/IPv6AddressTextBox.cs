namespace FlexFieldTextBoxWF.Controls;

public class IPv6AddressTextBox : FlexFieldTextBox
{
    public IPv6AddressTextBox()
    {
        // the format of this control is 'ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff'

        // set FieldCount first
        //
        FieldCount = 8;

        // every field is 4 digits max
        //
        SetMaxLength(4);

        // every separator is ':'...
        //
        SetSeparatorText(":");

        // except for the first and last separators
        //
        SetSeparatorText(0, string.Empty);
        SetSeparatorText(FieldCount, string.Empty);

        // the value format is hexadecimal
        //
        SetValueFormat(ValueFormat.Hexadecimal);

        // use leading zeros for every field
        //
        SetLeadingZeros(true);

        // use lowercase only
        //
        SetCasing(CharacterCasing.Lower);

        // add ':' key to cede focus for every field
        //
        var e = new KeyEventArgs(Keys.OemSemicolon);
        AddCedeFocusKey(e);

        // this should be the last thing
        //
        Size = MinimumSize;
    }
}