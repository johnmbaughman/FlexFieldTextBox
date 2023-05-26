namespace FlexFieldTextBoxWF;

internal interface IValueFormatter
{
    int MaxFieldLength { get; }

    string RegExString { get; }

    Size GetCharacterSize( Graphics g, Font font, CharacterCasing casing );

    bool IsValidKey( KeyEventArgs e );

    int MaxValue( int fieldLength );
 
    int Value( string text );
    string ValueText( int value, CharacterCasing casing );
}