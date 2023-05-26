using System.Globalization;

namespace FlexFieldTextBoxWF;

internal class HexadecimalValue : IValueFormatter
{
    public virtual int MaxFieldLength => string.Format( CultureInfo.InvariantCulture, "{0:x}", int.MaxValue ).Length - 1;

    public virtual string RegExString => "[0-9a-fA-F]";

    public virtual Size GetCharacterSize( Graphics g, Font font, CharacterCasing casing )
      {
         const int measureCharCount = 10;

         var charSize = new Size( 0, 0 );

         if ( casing == CharacterCasing.Lower )
         {
            for ( var c = 'a'; c <= 'f'; ++c )
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
         }
         else
         {
            for ( var c = 'A'; c <= 'F'; ++c )
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
         }

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
          switch (e.KeyCode)
          {
              case >= Keys.A and <= Keys.F:
              case >= Keys.NumPad0 and <= Keys.NumPad9:
                  return true;
              default:
                  return e.KeyCode is >= Keys.D0 and <= Keys.D9;
          }
      }

      public virtual int MaxValue( int fieldLength )
      {
          fieldLength = Math.Min( fieldLength, MaxFieldLength );
         var valueString = new string( 'f', fieldLength );

         return int.TryParse( valueString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result ) ? result : 0;
      }

      public virtual int Value( string text )
      {
         if ( string.IsNullOrEmpty(text))
         {
            return 0;
         }

         return int.TryParse( text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result ) ? result : 0;
      }

      public virtual string ValueText( int value, CharacterCasing casing )
      {
          return string.Format(CultureInfo.InvariantCulture, casing == CharacterCasing.Upper ? "{0:X}" : "{0:x}", value);
      }

      private static readonly TextFormatFlags TextFormatFlags = TextFormatFlags.HorizontalCenter 
                                                                 | TextFormatFlags.SingleLine 
                                                                 | TextFormatFlags.NoPadding;
}