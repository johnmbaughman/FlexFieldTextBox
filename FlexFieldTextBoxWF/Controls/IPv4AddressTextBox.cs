namespace FlexFieldTextBoxWF.Controls;

public class IPv4AddressTextBox : FlexFieldTextBox
{
    public IPv4AddressTextBox()
    {
        // the format of this control is '255.255.255.255'

        // set FieldCount first
        //
        FieldCount = 4;

        // every field is 3 digits max
        //
        SetMaxLength(3);

        // every separator is '.'...
        //
        SetSeparatorText(".");

        // except for the first and last separators
        //
        SetSeparatorText(0, string.Empty);
        SetSeparatorText(FieldCount, string.Empty);

        // set the range of every field to ( 0, 255 )
        //
        SetRange(0, 255);

        // add ',' keys to cede focus for every field
        //
        var e = new KeyEventArgs(Keys.OemPeriod);
        AddCedeFocusKey(e);
        e = new KeyEventArgs(Keys.Decimal);
        AddCedeFocusKey(e);

        // this should be the last thing
        //
        Size = MinimumSize;
    }
}