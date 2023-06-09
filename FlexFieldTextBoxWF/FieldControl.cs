﻿using System.Globalization;
using System.Text;

namespace FlexFieldTextBoxWF;

internal class FieldControl : TextBox
{
     #region Public Events

      public event EventHandler<CedeFocusEventArgs> CedeFocusEvent;
      public event EventHandler<FieldChangedEventArgs> FieldChangedEvent;
      public event EventHandler FieldSizeChangedEvent;
      public event EventHandler<FieldValidatedEventArgs> FieldValidatedEvent;

      #endregion  // Public Events

      #region Public Properties

      public bool AllowPrecedingZero
      {
         get => _allowPrecedingZero;
         set
         {
            _allowPrecedingZero = value;

            if (_allowPrecedingZero) return;

            if ( !Blank )
            {
                Text = _leadingZeros ? GetPaddedText() : GetCasedText();
            }
         }
      }

      public bool Blank => TextLength == 0;

      public int FieldIndex { get; set; } = -1;

      public bool LeadingZeros
      {
         get => _leadingZeros;
         set
         {
            _leadingZeros = value;

            if ( !Blank )
            {
               Text = _leadingZeros ? GetPaddedText() : GetCasedText();
            }
         }
      }

      public override int MaxLength
      {
         get => base.MaxLength;
         set
         {
            if ( value < 1 )
            {
               value = 1;
            }
            else if ( value > _valueFormatter.MaxFieldLength )
            {
               value = _valueFormatter.MaxFieldLength;
            }

            var preserveValue = !Blank;

            var previousFieldValue = Value;

            base.MaxLength = value;

            ResetRange();

            ResetSize();

            if ( preserveValue )
            {
               Value = previousFieldValue;
            }
         }
      }

      public Point MidPoint
      {
         get
         {
            var midPoint = Location;
            midPoint.Offset( Width / 2, Height / 2 );
            return midPoint;
         }
      }

      public new Size MinimumSize => CalculateMinimumSize();

      public string RegExString
      {
         get
         {
            var sb = new StringBuilder();

            sb.Append( _valueFormatter.RegExString );

            sb.Append(CultureInfo.InvariantCulture, $"{{0,{MaxLength}}}");

            return sb.ToString();
         }
      }

      public int RangeLow
      {
         get => _rangeLow;
         set
         {
            if ( value < 0 )
            {
               _rangeLow = 0;
            }
            else if ( value > RangeHigh )
            {
               _rangeLow = RangeHigh;
            }
            else
            {
               _rangeLow = value;
            }

            if ( !Blank && _valueFormatter.Value( Text ) < _rangeLow )
            {
               Text = _valueFormatter.ValueText( _rangeLow, CharacterCasing );
            }
         }
      }

      public int RangeHigh
      {
         get => _rangeHigh;
         set
         {
            if ( value < RangeLow )
            {
               _rangeHigh = RangeLow;
            }
            else if ( value > _valueFormatter.MaxValue( MaxLength ) )
            {
               _rangeHigh = _valueFormatter.MaxValue( MaxLength );
            }
            else
            {
               _rangeHigh = value;
            }

            if ( !Blank && _valueFormatter.Value( Text ) > _rangeHigh )
            {
               Text = _valueFormatter.ValueText( _rangeHigh, CharacterCasing );
            }
         }
      }

      public override string Text
      {
         get => base.Text;
         set
         {
            base.Text = value;

            if ( base.TextLength != 0 && LeadingZeros )
            {
               base.Text = GetPaddedText();
            }
         }
      }

      public int Value
      {
         get
         {
            var result = _valueFormatter.Value( Text );
            
            return result < RangeLow ? RangeLow : result;
         }
         set => Text = _valueFormatter.ValueText( value, CharacterCasing );
      }

      public ValueFormat ValueFormat
      {
         get => _valueFormat;
         set
         {
             if (_valueFormat == value) return;
             _valueFormat = value;

             var convertValue = !Blank;

             var previousFieldValue = Value;

             _valueFormatter = _valueFormat switch
             {
                 ValueFormat.Decimal => new DecimalValue(),
                 ValueFormat.Hexadecimal => new HexadecimalValue(),
                 _ => _valueFormatter
             };

             ResetRange();

             ResetCedeFocusKeys();

             ResetSize();

             if ( convertValue )
             {
                 Value = previousFieldValue;
             }
         }
      }

      #endregion     // Public Properties

      #region Public Methods
      
      public bool AddCedeFocusKey( KeyEventArgs e )
      {
         if ( _valueFormatter.IsValidKey( e ) )
         {
            return false;
         }

         if (_cedeFocusKeys.Contains(e)) return false;
         _cedeFocusKeys.Add( e );

         return true;

      }
  
      public void ClearCedeFocusKeys()
      {
         _cedeFocusKeys.Clear();
      }

      public void ResetCedeFocusKeys()
      {
         ClearCedeFocusKeys();

         var e = new KeyEventArgs( Keys.Space );
         _cedeFocusKeys.Add( e );
      }

      public void TakeFocus( Action action )
      {
         Focus();

         switch ( action )
         {
            case Action.Trim:

               if ( TextLength > 0 )
               {
                  var newLength = TextLength - 1;
                  base.Text = Text[..newLength];
               }

               SelectionStart = TextLength;

               return;

            case Action.Home:

               SelectionStart = 0;
               SelectionLength = 0;

               return;

            case Action.End:

               SelectionStart = TextLength;

               return;
         }
      }

      public void TakeFocus( Direction direction, Selection selection )
      {
         Focus();

         if ( selection == Selection.All )
         {
            SelectionStart = 0;
            SelectionLength = TextLength;
         }
         else
         {
            SelectionStart = direction == Direction.Forward ? 0 : TextLength;
         }
      }

      public override string ToString()
      {
         if ( LeadingZeros )
         {
            return GetPaddedText();
         }

         return !Blank ? GetCasedText() : _valueFormatter.ValueText( Value, CharacterCasing );
      }

      #endregion // Public Methods

      #region Constructor

      public FieldControl()
      {
         BorderStyle = BorderStyle.None;
         MaxLength = 3;
         Size = MinimumSize;
         TabStop = false;
         TextAlign = HorizontalAlignment.Center;

         ResetCedeFocusKeys();
      }

      #endregion     // Constructor

      #region Protected Methods

      protected override void OnFontChanged( EventArgs e )
      {
         base.OnFontChanged( e );

         Size = MinimumSize;
      }

      protected override void OnKeyDown( KeyEventArgs e )
      {
         base.OnKeyDown( e );

         switch ( e.KeyCode )
         {
            case Keys.Home:
               SendCedeFocusEvent( Action.Home );
               return;

            case Keys.End:
               SendCedeFocusEvent( Action.End );
               return;
         }

         if ( IsCedeFocusKey( e ) )
         {
            SendCedeFocusEvent( Direction.Forward, Selection.All );
            e.SuppressKeyPress = true;
            return;
         }

         if ( IsForwardKey( e ) )
         {
             if ( e.Control )
             {
                 SendCedeFocusEvent( Direction.Forward, Selection.All );
                 return;
             }

             if (SelectionLength != 0 || SelectionStart != TextLength) return;
             SendCedeFocusEvent( Direction.Forward, Selection.None );
             return;
         }

         if ( IsReverseKey( e ) )
         {
             if ( e.Control )
             {
                 SendCedeFocusEvent( Direction.Reverse, Selection.All );
                 return;
             }

             if (SelectionLength != 0 || SelectionStart != 0) return;
             SendCedeFocusEvent( Direction.Reverse, Selection.None );
             return;
         }
         if ( IsBackspaceKey( e ) )
         {
             HandleBackspaceKey( e );
         }
         else if ( !_valueFormatter.IsValidKey( e ) &&
                   !IsEditKey( e ) &&
                   !IsEnterKey( e ) )
         {
             e.SuppressKeyPress = true;
         }
      }

      protected override void OnParentBackColorChanged( EventArgs e )
      {
         base.OnParentBackColorChanged( e );

         BackColor = Parent.BackColor;
    }

      protected override void OnParentForeColorChanged( EventArgs e )
      {
         base.OnParentForeColorChanged( e );

         ForeColor = Parent.ForeColor;
      }

      protected override void OnSizeChanged( EventArgs e )
      {
         base.OnSizeChanged( e );

         if ( FieldSizeChangedEvent != null )
         {
            FieldSizeChangedEvent( this, EventArgs.Empty );
         }
      }

      protected override void OnTextChanged( EventArgs e )
      {
         base.OnTextChanged( e );

         if ( !Blank )
         {
            var value = _valueFormatter.Value( Text );

            if ( value > RangeHigh )
            {
               base.Text = _valueFormatter.ValueText( RangeHigh, CharacterCasing );
               SelectionStart = 0;
            }
            else if ( TextLength == MaxLength && value < RangeLow )
            {
               base.Text = _valueFormatter.ValueText( RangeLow, CharacterCasing );
               SelectionStart = 0;
            }
            else
            {
               var originalLength = TextLength;
               var newSelectionStart = SelectionStart;

               base.Text = GetCasedText();

               if ( TextLength < originalLength )
               {
                  newSelectionStart -= originalLength - TextLength;
                  SelectionStart = Math.Max( 0, newSelectionStart );
               }
            }
         }

         SendFieldChangedEvent();

         if ( TextLength == MaxLength && Focused && SelectionStart == TextLength )
         {
            SendCedeFocusEvent( Direction.Forward, Selection.All );
         }
      }

      protected override void OnValidating( System.ComponentModel.CancelEventArgs e )
      {
         base.OnValidating( e );

         if (Blank) return;
         if ( _valueFormatter.Value( Text ) < RangeLow )
         {
             Text = _valueFormatter.ValueText( RangeLow, CharacterCasing );
         }

         if ( LeadingZeros )
         {
             Text = GetPaddedText();
         }
      }

      protected override void OnValidated( EventArgs e )
      {
         base.OnValidated( e );

         if (FieldValidatedEvent == null) return;
         var args = new FieldValidatedEventArgs
         {
             FieldIndex = FieldIndex,
             Text = Text
         };

         FieldValidatedEvent( this, args );
      }

      protected override void WndProc( ref Message m )
      {
         switch ( m.Msg )
         {
            case 0x007b:  // WM_CONTEXTMENU
               return;
         }

         base.WndProc( ref m );
      }

      #endregion     // Protected Methods

      #region Private Methods

      private Size CalculateMinimumSize()
      {
         var g = Graphics.FromHwnd( Handle );

         var minSize = _valueFormatter.GetCharacterSize( g,
            Font, CharacterCasing );

         g.Dispose();

         minSize.Width *= MaxLength;

         return minSize;
      }

      private string GetCasedText()
      {
         var value = _valueFormatter.Value( Text );

         var valueString = _valueFormatter.ValueText( value, CharacterCasing );

         var textIndex = 0;
         var valueStringIndex = 0;

         var sb = new StringBuilder();

         var nonZeroFound = false;

         var zeroAppended = false;

         while ( textIndex < TextLength &&
                 valueStringIndex < valueString.Length )
         {
            if ( !nonZeroFound && Text[textIndex] == '0' )
            {
               if ( LeadingZeros || AllowPrecedingZero )
               {
                  sb.Append( '0' );
               }
               else if ( !zeroAppended && value == 0 )
               {
                  sb.Append( '0' );
                  zeroAppended = true;
               }

               ++textIndex;          
            }
            else if ( Text[textIndex] == valueString[valueStringIndex] )
            {
               sb.Append( valueString[valueStringIndex] );

               ++textIndex;
               ++valueStringIndex;

               nonZeroFound = true;
            }
            else if ( char.IsUpper( Text[textIndex] ) )
            {
               sb.Append( char.ToUpper( valueString[valueStringIndex], CultureInfo.InvariantCulture ) );

               ++textIndex;
               ++valueStringIndex;

               nonZeroFound = true;
            }
            else
            {
               ++textIndex;
            }
         }

         return sb.ToString();
      }

      private string GetPaddedText()
      {
         var sb = new StringBuilder( GetCasedText() );

         while ( sb.Length < MaxLength )
         {
            sb.Insert( 0, '0' );
         }

         return sb.ToString();
      }

      private void HandleBackspaceKey( KeyEventArgs e )
      {
          if (ReadOnly || (TextLength != 0 && (SelectionStart != 0 || SelectionLength != 0))) return;
          SendCedeFocusEvent( Action.Trim );
          e.SuppressKeyPress = true;
      }

      private static bool IsBackspaceKey( KeyEventArgs e )
      {
          return e.KeyCode == Keys.Back;
      }

      private bool IsCedeFocusKey( KeyEventArgs e )
      {
         var isCedeFocusKey = _cedeFocusKeys.Any(key => e.KeyCode == key.KeyCode);

         if (!isCedeFocusKey) return false;
         return TextLength != 0 && SelectionLength == 0 && SelectionStart != 0;
      }

      private static bool IsEditKey( KeyEventArgs e )
      {
         if ( e.KeyCode is Keys.Back or Keys.Delete )
         {
            return true;
         }

         return e.Modifiers == Keys.Control &&
                e.KeyCode is Keys.C or Keys.V or Keys.X;
      }

      private static bool IsEnterKey( KeyEventArgs e )
      {
          return e.KeyCode is Keys.Enter or Keys.Return;
      }

      private static bool IsForwardKey( KeyEventArgs e )
      {
          return e.KeyCode is Keys.Right or Keys.Down;
      }

      private static bool IsReverseKey( KeyEventArgs e )
      {
          return e.KeyCode is Keys.Left or Keys.Up;
      }

      private void ResetRange()
      {
         RangeLow = 0;
         RangeHigh = _valueFormatter.MaxValue( base.MaxLength );
      }

      private void ResetSize()
      {
         Size = MinimumSize;
      }

      private void SendCedeFocusEvent( Action action )
      {
          if (CedeFocusEvent == null) return;
          var args = new CedeFocusEventArgs
          {
              Action = action,
              FieldIndex = FieldIndex
          };

          CedeFocusEvent( this, args );
      }

      private void SendCedeFocusEvent( Direction direction, Selection selection )
      {
          if (CedeFocusEvent == null) return;
          var args = new CedeFocusEventArgs
          {
              Action = Action.None,
              Direction = direction,
              FieldIndex = FieldIndex,
              Selection = selection
          };

          CedeFocusEvent( this, args );
      }

      private void SendFieldChangedEvent()
      {
          if (FieldChangedEvent == null) return;
          var args = new FieldChangedEventArgs
          {
              FieldIndex = FieldIndex,
              Text = Text
          };
          FieldChangedEvent( this, args );
      }

      #endregion     // Private Methods

      #region Private Constants

      private const int MeasureCharCount = 10;

      #endregion     // Private Constants

      #region Private Data

      private ValueFormat _valueFormat = ValueFormat.Decimal;
      private IValueFormatter _valueFormatter = new DecimalValue();
      private bool _leadingZeros;
      private bool _allowPrecedingZero;

      private int _rangeLow;  // = 0
      private int _rangeHigh = int.MaxValue;

      private readonly List<KeyEventArgs> _cedeFocusKeys = new();

      #endregion     // Private Data
}