namespace FlexFieldTextBoxWF.Controls;

public class PhoneNumberTextBox : FlexFieldTextBox
{
    public PhoneNumberTextBox()
    {
        // the format of this control is '(999) 999-9999'

        // set FieldCount first
        //
        FieldCount = 3;

        // every field is 3 digits max...
        //
        SetMaxLength(3);

        // except for the last field
        //
        SetMaxLength(FieldCount - 1, 4);

        // first separator is '('...
        //
        SetSeparatorText(0, "(");

        // second separator is ')'...
        //
        SetSeparatorText(1, ") ");

        // third separator is '-'...
        //
        SetSeparatorText(2, "-");

        // and the last is ''
        //
        SetSeparatorText(FieldCount, string.Empty);

        // the ')' key is the same as the '0' key so it cannot be added as a
        // cede focus key to the first field

        // add '-' keys to cede focus to the second field
        //
        var e = new KeyEventArgs(Keys.OemMinus);
        AddCedeFocusKey(FieldCount - 2, e);
        e = new KeyEventArgs(Keys.Subtract);
        AddCedeFocusKey(FieldCount - 2, e);

        // this should be the last thing
        //
        Size = MinimumSize;
    }
}