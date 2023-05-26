namespace FlexFieldTextBoxWF.Controls;

public class IPv4AddressPortTextBox : FlexFieldTextBox
{
    public IPv4AddressPortTextBox()
    {
        // the format of this control is '255.255.255.255 : 65535'

        // set FieldCount first
        //
        FieldCount = 5;

        // every field is 3 digits max...
        //
        SetMaxLength(3);

        // except for the last field
        //
        SetMaxLength(FieldCount - 1, 5);

        // every separator is '.'...
        //
        SetSeparatorText(".");

        // except for the first and last separators...
        //
        SetSeparatorText(0, string.Empty);
        SetSeparatorText(FieldCount, string.Empty);

        // and the next to last
        //
        SetSeparatorText(FieldCount - 1, " : ");

        // set the range of every field to ( 0, 255 )...
        //
        SetRange(0, 255);

        // except for the last field
        //
        SetRange(FieldCount - 1, 0, 65535);

        // add ',' keys to cede focus for every field...
        //
        var e = new KeyEventArgs(Keys.OemPeriod);
        AddCedeFocusKey(e);
        e = new KeyEventArgs(Keys.Decimal);
        AddCedeFocusKey(e);

        // except for the last two fields
        //
        ResetCedeFocusKeys(FieldCount - 2);
        ResetCedeFocusKeys(FieldCount - 1);

        // add ':' keys to next to last field
        //
        e = new KeyEventArgs(Keys.OemSemicolon);
        AddCedeFocusKey(FieldCount - 2, e);

        // this should be the last thing
        //
        Size = MinimumSize;
    }
}