using System.Globalization;

namespace FlexFieldTextBoxWF;

internal class DecimalValue : IValueFormatter
{
    public virtual int MaxFieldLength => string.Format( CultureInfo.InvariantCulture, "{0:d}", int.MaxValue ).Length - 1;

    public virtual string RegExString => "[0-9]";

    public virtual Size GetCharacterSize( Graphics g, Font font, CharacterCasing casing )
      {
         const int measureCharCount = 10;

         var charSize = new Size( 0, 0 );

         for ( var c = '0'; c <= '9'; ++c )
         {
            var newSize = TextRenderer.MeasureText( g, new string( c, measureCharCount ), font, new Size( 0, 0 ),
               TextFormatFlags );

            newSize.Width = (int)Math.Ceiling( newSize.Width / (double)measureCharCount );

            if ( newSize.Width > charSize.Width )
            {
               charSize.Width = newSize.Width;
            }

            if ( newSize.Height > charSize.Height )
            {
               charSize.Height = newSize.Height;
            }
         }

         return charSize;
      }

      public virtual bool IsValidKey( KeyEventArgs e )
      {
          if (e.KeyCode is >= Keys.NumPad0 and <= Keys.NumPad9) return true;
          return e.KeyCode is >= Keys.D0 and <= Keys.D9;
      }

      public virtual int MaxValue( int fieldLength )
      {
          fieldLength = Math.Min( fieldLength, MaxFieldLength );
         var valueString = new string( '9', fieldLength );

         return int.TryParse( valueString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result ) ? result : 0;
      }

      public virtual int Value( string text )
      {
         if ( string.IsNullOrEmpty(text))
         {
            return 0;
         }

         return int.TryParse( text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result ) ? result : 0;
      }

      public virtual string ValueText( int value, CharacterCasing casing )
      {
         return value.ToString( CultureInfo.InvariantCulture );
      }

      private static readonly TextFormatFlags TextFormatFlags = TextFormatFlags.HorizontalCenter 
                                                                | TextFormatFlags.SingleLine 
                                                                | TextFormatFlags.NoPadding;
}