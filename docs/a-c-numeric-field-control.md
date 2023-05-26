> Recreated the original doc/web page as much as possible.

# A C# Numeric Field Control

![avatar](../docs/images/mid-5741Avatar.png) [mid=5741](https://www.codeproject.com/script/Membership/View.aspx?mid=5741)

30 Apr 2008 MIT 8 min read

An abstract base for a numeric fielded control.

![demo screenshot](../docs/images/DemoScreenshot.png)

## Introduction

After creating an [IP address control for the .NET Framework](http://www.codeproject.com/KB/miscctrl/IpAddrCtrlLib.aspx), I applied the lessons learned towards creating an abstract control. This control can be easily configured to allow input of any value that is a collection of numeric fields, such as an IP address, IP address with ports, IPv6 address, MAC address, telephone number, or Social Security Number.

## Background

`FlexFieldControl` is an abstract `UserControl`-based class that aggregates **n** controls of type `FieldControl` and **n+1** controls of type `SeparatorControl`. These controls are arranged horizontally in an alternating fashion, beginning with a `SeparatorControl`. Below is an image of a default `FlexFieldControl`-based control, with the aggregated controls highlighted:

![defaultcontrol](../docs/images/DefaultControl.png)

By default, `FlexFieldControl` has three `FieldControl`s and four `SeparatorControl`s. Following the API below is an example of how to extend `FlexFieldControl` to create a new control, `MACAddressControl`, with just eleven lines of code.

## Using the Code

Once the library containing `FlexFieldControl` (*FlexFieldControlLib.dll*) is built, add the DLL as a reference to your Windows Forms project. `FlexFieldControl` is defined as `abstract`; it will not appear in the Toolbox of the Windows Forms designer. However, any control based on `FlexFieldControl` will. 

> Future note from 2023: It works now and does show the default control in the Toolbox.

### Public Instance Properties

- `AutoHeight` - Gets or sets a value indicating whether the control is automatically sized vertically according to the current font and border. The default value is `true`
- `Blank` - Gets a value indicating whether all of the fields in the control are empty
- `BorderStyle` - Gets or sets the border style of the control. The default value is `BorderStyle.Fixed3D`
- `FieldCount` - Gets or sets the number of fields in the control. The default value is `3` and the minimum value is `1`. Setting this value resets every field and separator to its default state
- `ReadOnly` - Gets or sets a value indicating whether the contents of the control can be changed
- `Text` - Gets or sets the text of the control

### Public Instance Methods

- `AddCedeFocusKey` - Adds a specific keystroke to a field that cedes focus to the next field in the control. By default, the [Space] key will cede focus for any field that is not blank
- `Clear` - Clears all text from all fields in the control
- `ClearCedeFocusKeys` - Removes all keystrokes that cede focus from a field
- `GetCasing` - Gets the character casing for a field in the control. The default is `CharacterCasing.Normal`
- `GetFieldText` - Gets the text for a field in the control
- `GetLeadingZeros` - Gets whether a field that is not blank has leading zeros. The default value is `false`
- `GetMaxLength` - Gets the maximum number of characters allowed for a field. The default value is `3`
- `GetRangeHigh` - Gets the maximum value allowed for a field. The default value is based on the maximum length and value format of the field
- `GetRangeLow` - Gets the minimum value allowed for a field. The default value is `0`
- `GetSeparatorText` - Gets the text for a separator in the control
- `GetValue` - Gets the value for a field in the control. If the field is blank, its value is the same as its low range value
- `GetValueFormat` - Gets the value format for a field in the control. The default is `ValueFormat.Decimal`
- `HasFocus` - Gets whether a field in the control has input focus
- `IsBlank` - Gets whether a field in the control is blank
- `ResetCedeFocusKeys` - Resets a field to its default cede focus key, the [Space] key
- `SetCasing` - Sets the character casing for a field in the control `CharacterCasing.Lower` will minimize the horizontal size for a field that has a value format of `ValueFormat.Hexadecimal`
- `SetFieldText` - Sets the text for a field in the control
- `SetFocus` - Sets input focus to a field in the control
- `SetLeadingZeros` - Sets whether a field that is not blank has leading zeros
- `SetMaxLength` - Sets the maximum number of characters allowed for a field. The minimum value is 1; the maximum value depends on the value format: `9` characters for decimal values and `7` characters for hexadecimal values
- `SetRange` - Sets the minimum and maximum values allowed for a field in the control
- `SetSeparatorText` - Sets the text for a separator in the control
- `SetValue` - Sets the value for a field in the control
- `SetValueFormat` - Sets the value format for a field in the control
- `ToString` - Gets the text of the control. If any field is blank, that field's text will be its low range value

The ~~client~~ code can register a handler for the public event, `FieldChangedEvent`, to be notified when the text in any field of the control changes. Another public event, `FieldValidatedEvent`, notifies when the text of any field is validated. Note that `Text` and `ToString()` may not return the same value. If there are any empty fields in the control, `Text` will return a value that will reflect the empty fields. `ToString()` will fill in any empty fields with that field's low range value.

### Creating MACAddressControl

Here is the source code for creating a simple `MACAddressControl`:

``` csharp
using System;
using System.Windows.Forms;
using FlexFieldControlLib;

namespace TestFlexFieldControl
{
    class MACAddressControl : FlexFieldControl
    {
        public MACAddressControl()
        {
            // the format of this control is 'FF:FF:FF:FF:FF:FF'
            // set FieldCount first
            //
            FieldCount = 6;

            // every field is 2 digits max
            //
            SetMaxLength( 2 );

            // every separator is ':'...
            //
            SetSeparatorText( ":" );

            // except for the first and last separators
            //
            SetSeparatorText( 0, String.Empty );
            SetSeparatorText( FieldCount, String.Empty );

            // the value format is hexadecimal
            //
            SetValueFormat( ValueFormat.Hexadecimal );

            // use leading zeros for every field
            //
            SetLeadingZeros( true );

            // use uppercase only
            //
            SetCasing( CharacterCasing.Upper );

            // add ':' key to cede focus for every field
            //
            KeyEventArgs e = new KeyEventArgs( Keys.OemSemicolon );
            AddCedeFocusKey( e );

            // this should be the last thing
            //
            Size = MinimumSize;
        }
    }
}
```

The first step in the constructor should always be setting the `FieldCount` for the control. Otherwise, any prior settings will be reset. For instance, if the value format of every field was set to `ValueFormat.Hexadecimal` and then `FieldCount` was changed, the value format of each field would be reset to its default, `ValueFormat.Decimal`.

The next step is to set the maximum number of characters allowed in each field. In this case, every field is set to the same maximum length via `SetMaxLength( 2 )`. Alternatively, each field could be set individually: `SetMaxLength( 0, 2 )`, `SetMaxLength( 1, 2 )`, `SetMaxLength( 2, 2 )`, etc. The first parameter in `SetMaxLength` is the zero-based index of a field in the control.

By default, each separator has the text "<", "><", or ">" depending on its location within the control. For this `MACAddressControl`, each separator except for the first and last has ":" as its text. `SetSeparatorText( ":" )` sets the text of every separator to ":". To clear the text of the first and last separators, `SetSeparatorText( 0, String.Empty )` and `SetSeparatorText( FieldCount, String.Empty )` are called.

The default value format of each field is `ValueFormat.Decimal`, but this control uses hexadecimal values in each field. Calling `SetValueFormat( ValueFormat.Hexadecimal )` sets every field in the control to a hexadecimal value format. Alternatively, `SetValueFormat` can be called with a field index to set the value format of an individual field.

Since MAC addresses are commonly displayed with leading zeros in each field -- i.e., "08:09:0A:0B:0C:0D" rather than "8:9:A:B:C:D" -- each field is set to show leading zeros with the call `SetLeadingZeros( true )`. Alternatively, `SetLeadingZeros` can be called with a field index to set the leading zeros property of an individual field. By default, each field does not display leading zeros.

This control forces hexadecimal letters in every field to be displayed as capitals by calling `SetCasing( CharacterCasing.Upper )`. If a user enters "a" in any field, the control will display "A" in that field. Alternatively, `SetCasing` can be called with a field index to set the character casing of each field individually. As a convenience to the user, this control adds the [;] key as a cede focus key to each field, using:

``` csharp
KeyEventArgs e = new KeyEventArgs( Keys.OemSemicolon );
AddCedeFocusKey( e );
```

If the value of a field can be represented by one character, the user can enter that character and then press the [;] key or the [Space] key to advance the input focus of the control to the next field. Alternatively, `AddCedeFocusKey` can be called with a field index to set the cede focus keys for each field individually.

The final call in the constructor, `Size = MinimumSize`, forces the control to be as small as possible. As settings change, the control will only grow to accommodate them; it will not automatically shrink. Once the control is placed on a form, it can be resized as necessary.

### History

- 29 April 2008
  - Added propagation for `PreviewKeyDown`, `KeyDown`, and `KeyUp` events
- 20 November 2007
  - Added `Dispose()` calls when recreating subcontrols on `FieldCount` change. Thanks to [Glenn](http://www.codeproject.com/script/profile/whos_who.asp?id=1003504) for reporting this problem
- 23 October 2007
  - `ReadOnly` should now really be read-only
- 4 October 2007
  - Added proper event propagation for some mouse events
  - Added the `AnyBlank` property
  - Removed superfluous code
  - Addressed a potential resource leak when calculating text size
  - Compliant with FxCop 1.35
- 23 July 2007
  - Fixed bug related to setting text of fields with leading zeros. Thanks once again to zeno_nz for reporting this problem
- 15 July 2007
  - `GotFocus` and `LostFocus` events are consistently raised
  - The `AllowInternalTab` property was added to allow tabbing within the control. The default value is `false`
- 27 June 2007
  - Changes to `Text` property are persisted in design mode
  - The `KeyPress` event can be trapped by clients of the library. Thanks to zeno_nz for requesting this enhancement
- 5 June 2007
  - Initial release

### License

This article, along with any associated source code and files, is licensed under [The MIT License](http://www.opensource.org/licenses/mit-license.php)

#### Written By
[**mid=5741**](https://www.codeproject.com/Members/mid-5741)

![ohcanada](../docs/images/canadaflag-small.png) Canada ![avatar](../docs/images/mid-5741Avatar.png)