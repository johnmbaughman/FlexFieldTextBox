using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace FlexFieldTextBoxWF;

[Designer(typeof(FlexFieldTextBoxDesigner))]
partial class FlexFieldTextBox
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        //components = new System.ComponentModel.Container();
        //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //this.ClientSize = new System.Drawing.Size(800, 450);

        Cursor = Cursors.IBeam;

        InitializeControls();

        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.ContainerControl, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.UserPaint, true);

        AdjustSize();
    }

    #endregion

    #region Public Events

    /// <summary>
    /// Raised when the text of any field changes.
    /// </summary>
    public event EventHandler<FieldChangedEventArgs> FieldChangedEvent;
    /// <summary>
    /// Raised when the text of any field is validated, such as when focus
    /// changes from one field to another.
    /// </summary>
    public event EventHandler<FieldValidatedEventArgs> FieldValidatedEvent;

    #endregion  // Public Events

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether the control allows the [Tab]
    /// key to index the fields within the control.
    /// </summary>
    [Browsable(true)]
    public bool AllowInternalTab
    {
        get
        {
            return _fieldControls.Select(fc => fc.TabStop).FirstOrDefault();
        }
        set
        {
            foreach (var fc in _fieldControls)
            {
                fc.TabStop = value;
            }
        }
    }

    /// <summary>
    /// Gets whether any field in the control is blank.
    /// </summary>
    public bool AnyBlank
    {
        get
        {
            foreach (var fc in _fieldControls)
            {
                if (fc.Blank)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control is automatically
    /// sized vertically according to the current font and border. Default is
    /// true.
    /// </summary>
    [Browsable(true)]
    public bool AutoHeight
    {
        get => _autoHeight;
        set
        {
            if (_autoHeight == value) return;
            _autoHeight = value;
            if (_autoHeight)
            {
                AdjustSize();
            }
        }
    }

    /// <summary>
    /// Gets a horizontal snapline associated with the base of the text
    /// string.
    /// </summary>
    public int Baseline
    {
        get
        {
            var textMetric = GetTextMetrics(Handle, Font);

            var offset = textMetric.tmAscent + 1;

            switch (BorderStyle)
            {
                case BorderStyle.Fixed3D:
                    offset += _fixed3DOffset.Height;
                    break;
                case BorderStyle.FixedSingle:
                    offset += _fixedSingleOffset.Height;
                    break;
            }

            return offset;
        }
    }

    /// <summary>
    /// Gets whether every field in the control is blank.
    /// </summary>
    public bool Blank
    {
        get
        {
            return _fieldControls.All(fc => fc.Blank);
        }
    }

    /// <summary>
    /// Gets or sets the type of border that is drawn around the control.
    /// </summary>
    [Browsable(true)]
    public new BorderStyle BorderStyle
    {
        get => _borderStyle;
        set
        {
            if (_borderStyle == value) return;
            _borderStyle = value;
            AdjustSize();
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the number of fields in the control. Default is 3;
    /// minimum is 1. Setting this value resets every field and separator
    /// to its default state.
    /// </summary>
    [Browsable(true)]
    public int FieldCount
    {
        get => _fieldCount;
        set
        {
            if (value < 1)
            {
                value = 1;
            }

            if (value == _fieldCount) return;
            _fieldCount = value;
            InitializeControls();
            AdjustSize();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the control has input focus.
    /// </summary>
    public override bool Focused
    {
        get
        {
            return _fieldControls.Any(fc => fc.Focused);
        }
    }

    /// <summary>
    /// Gets the minimum size for the control.
    /// </summary>
    [Browsable(true)]
    public new Size MinimumSize => CalculateMinimumSize();

    /// <summary>
    /// Gets or sets a value indicating whether the contents of the control
    /// can be changed. 
    /// </summary>
    [Browsable(true)]
    public bool ReadOnly
    {
        get => _readOnly;
        set
        {
            _readOnly = value;

            foreach (var fc in _fieldControls)
            {
                fc.ReadOnly = _readOnly;
            }

            foreach (var sc in _separatorControls)
            {
                sc.ReadOnly = _readOnly;
            }

            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the text of the control.
    /// </summary>
    [Bindable(true)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text
    {
        get
        {
            var sb = new StringBuilder();

            for (var index = 0; index < FieldCount; ++index)
            {
                sb.Append(_separatorControls[index].Text);
                sb.Append(_fieldControls[index].Text);
            }

            sb.Append(_separatorControls[FieldCount].Text);

            return sb.ToString();
        }
        set => Parse(value);
    }

    #endregion  // Public Properties

    #region Public Methods

    /// <summary>
    /// Adds a KeyEventArgs to every field that indicates when a field should
    /// cede focus to the next field in the control. By default, each field
    /// has a single cede focus key -- the [Space] key. 
    /// </summary>
    /// <param name="e"><see cref="System.Windows.Forms.Keys">KeyCode</see>
    /// indicates which keyboard key will cede focus.</param>
    public void AddCedeFocusKey(KeyEventArgs e)
    {
        foreach (var fc in _fieldControls)
        {
            fc.AddCedeFocusKey(e);
        }
    }

    /// <summary>
    /// Adds a KeyEventArgs to a specific field that indicates when a field
    /// should cede focus to the next field in the control. By default, each
    /// field has a single cede focus key -- the [Space] key. 
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public bool AddCedeFocusKey(int fieldIndex, KeyEventArgs e)
    {
        return IsValidFieldIndex(fieldIndex) && _fieldControls[fieldIndex].AddCedeFocusKey(e);
    }

    /// <summary>
    /// Clears all content from the fields in the control.
    /// </summary>
    public void Clear()
    {
        foreach (var fc in _fieldControls)
        {
            fc.Clear();
        }
    }

    /// <summary>
    /// Removes every cede focus key from every field in the control.
    /// </summary>
    public void ClearCedeFocusKeys()
    {
        foreach (var fc in _fieldControls)
        {
            fc.ClearCedeFocusKeys();
        }
    }

    /// <summary>
    /// Removes every cede focus key from a specific field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    public void ClearCedeFocusKeys(int fieldIndex)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].ClearCedeFocusKeys();
        }
    }

    /// <summary>
    /// Gets whether a field allows a preceding zero.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public bool GetAllowPrecedingZero(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) && _fieldControls[fieldIndex].AllowPrecedingZero;
    }

    /// <summary>
    /// Gets the character casing for a field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public CharacterCasing GetCasing(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].CharacterCasing : CharacterCasing.Normal;
    }

    /// <summary>
    /// Gets the text of a specific field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public string GetFieldText(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].Text : string.Empty;
    }

    /// <summary>
    /// Gets whether a field that is not blank has leading zeros.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public bool GetLeadingZeros(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) && _fieldControls[fieldIndex].LeadingZeros;
    }

    /// <summary>
    /// Gets the maximum number of characters allowed in a specific field.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public int GetMaxLength(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].MaxLength : 0;
    }

    /// <summary>
    /// Gets the high inclusive boundary of allowed values for a specific
    /// field. The default value varies based on ValueFormat and MaxLength,
    /// but it is always the maximum value that can be represented by the
    /// field.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public int GetRangeHigh(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].RangeHigh : 0;
    }

    /// <summary>
    /// Gets the low inclusive boundary of allowed values for a specific
    /// field. Default value is 0.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public int GetRangeLow(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].RangeLow : 0;
    }

    /// <summary>
    /// Gets the text for a specific separator in the control.
    /// </summary>
    /// <param name="separatorIndex"></param>
    /// <returns></returns>
    public string GetSeparatorText(int separatorIndex)
    {
        return IsValidSeparatorIndex(separatorIndex) ? _separatorControls[separatorIndex].Text : string.Empty;
    }

    /// <summary>
    /// Gets the value of a specific field. If the field is blank, its value
    /// is the same as its low range value.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public int GetValue(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].Value : 0;
    }

    /// <summary>
    /// Gets the value format for a specific field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public ValueFormat GetValueFormat(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) ? _fieldControls[fieldIndex].ValueFormat : ValueFormat.Decimal;
    }

    /// <summary>
    /// Determines if a specific field has input focus. True indicates that
    /// the field has focus.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public bool HasFocus(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) && _fieldControls[fieldIndex].Focused;
    }

    /// <summary>
    /// Determines if every field in the control is blank. True indicates that
    /// every field in the control is blank.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <returns></returns>
    public bool IsBlank(int fieldIndex)
    {
        return IsValidFieldIndex(fieldIndex) && _fieldControls[fieldIndex].Blank;
    }

    /// <summary>
    /// Removes every cede focus key from every field, and adds the default
    /// cede focus key -- the [Space] key -- to every field.
    /// </summary>
    public void ResetCedeFocusKeys()
    {
        foreach (var fc in _fieldControls)
        {
            fc.ResetCedeFocusKeys();
        }
    }

    /// <summary>
    /// Removes every cede focus key from a specific field, and adds the
    /// default cede focus key -- the [Space] key -- to the field.
    /// </summary>
    /// <param name="fieldIndex"></param>
    public void ResetCedeFocusKeys(int fieldIndex)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].ResetCedeFocusKeys();
        }
    }

    /// <summary>
    /// Toggles whether every field allows a preceding zero.
    /// </summary>
    /// <param name="allowPrecedingZero"></param>
    public void SetAllowPrecedingZero(bool allowPrecedingZero)
    {
        foreach (var fc in _fieldControls)
        {
            fc.AllowPrecedingZero = allowPrecedingZero;
        }
    }

    /// <summary>
    /// Toggles whether a specific field allows a preceding zero.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="allowPrecedingZero"></param>
    public void SetAllowPrecedingZero(int fieldIndex, bool allowPrecedingZero)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].AllowPrecedingZero = allowPrecedingZero;
        }
    }


    /// <summary>
    /// Sets the character casing for every field in the control.
    /// </summary>
    /// <param name="casing"></param>
    public void SetCasing(CharacterCasing casing)
    {
        foreach (var fc in _fieldControls)
        {
            fc.CharacterCasing = casing;
            fc.Size = fc.MinimumSize;
        }
    }

    /// <summary>
    /// Sets the character casing for a specific field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="casing"></param>
    public void SetCasing(int fieldIndex, CharacterCasing casing)
    {
        if (!IsValidFieldIndex(fieldIndex)) return;
        _fieldControls[fieldIndex].CharacterCasing = casing;
        _fieldControls[fieldIndex].Size = _fieldControls[fieldIndex].MinimumSize;
    }

    /// <summary>
    /// Sets text for every field in the control.
    /// </summary>
    /// <param name="text"></param>
    public void SetFieldText(string text)
    {
        foreach (var fc in _fieldControls)
        {
            fc.Text = text;
        }
    }

    /// <summary>
    /// Sets text for a specific field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="text"></param>
    public void SetFieldText(int fieldIndex, string text)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].Text = text;
        }
    }

    /// <summary>
    /// Sets input focus to a field in the control.
    /// </summary>
    /// <param name="fieldIndex"></param>
    public void SetFocus(int fieldIndex)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].TakeFocus(Direction.Forward, Selection.All);
        }
    }

    /// <summary>
    /// Toggles whether the value for every field is displayed with leading
    /// zeros.
    /// </summary>
    /// <param name="leadingZeros"><code>true</code> indicates that the value
    /// is displayed with leading zeros.</param>
    public void SetLeadingZeros(bool leadingZeros)
    {
        foreach (var fc in _fieldControls)
        {
            fc.LeadingZeros = leadingZeros;
        }
    }

    /// <summary>
    /// Toggles whether the value for a specific field is displayed with
    /// leading zeros.
    /// </summary>
    /// <param name="fieldIndex">Zero-based index for field.</param>
    /// <param name="leadingZeros"><c>true</c> indicates value should be
    /// displayed with leading zeros.</param>
    public void SetLeadingZeros(int fieldIndex, bool leadingZeros)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].LeadingZeros = leadingZeros;
        }
    }

    /// <summary>
    /// Sets the maximum length for every field in the control.
    /// </summary>
    /// <param name="maxLength"></param>
    public void SetMaxLength(int maxLength)
    {
        foreach (var fc in _fieldControls)
        {
            fc.MaxLength = maxLength;
        }

        AdjustSize();
    }

    /// <summary>
    /// Sets the maximum length for a specific field in the control. Default
    /// value is 3.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="maxLength"></param>
    public void SetMaxLength(int fieldIndex, int maxLength)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].MaxLength = maxLength;
        }

        AdjustSize();
    }

    /// <summary>
    /// Sets the low and high range for every field.
    /// </summary>
    /// <param name="low"></param>
    /// <param name="high"></param>
    public void SetRange(int low, int high)
    {
        foreach (var fc in _fieldControls)
        {
            fc.RangeLow = low;
            fc.RangeHigh = high;
        }
    }

    /// <summary>
    /// Sets the low and high range for a specific field.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="low"></param>
    /// <param name="high"></param>
    public void SetRange(int fieldIndex, int low, int high)
    {
        if (!IsValidFieldIndex(fieldIndex)) return;
        _fieldControls[fieldIndex].RangeLow = low;
        _fieldControls[fieldIndex].RangeHigh = high;
    }

    /// <summary>
    /// Sets the text for every separator.
    /// </summary>
    /// <param name="text"></param>
    public void SetSeparatorText(string text)
    {
        foreach (var sc in _separatorControls)
        {
            sc.Text = text;
        }

        AdjustSize();
    }

    /// <summary>
    /// Sets the text for a specific separator.
    /// </summary>
    /// <param name="separatorIndex"></param>
    /// <param name="text"></param>
    public void SetSeparatorText(int separatorIndex, string text)
    {
        if (!IsValidSeparatorIndex(separatorIndex)) return;
        _separatorControls[separatorIndex].Text = text;
        AdjustSize();
    }

    /// <summary>
    /// Sets the value for every field.
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int value)
    {
        foreach (var fc in _fieldControls)
        {
            fc.Value = value;
        }
    }

    /// <summary>
    /// Sets the value for a specific field.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="value"></param>
    public void SetValue(int fieldIndex, int value)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].Value = value;
        }
    }

    /// <summary>
    /// Sets the value format for every field.
    /// </summary>
    /// <param name="format"></param>
    public void SetValueFormat(ValueFormat format)
    {
        foreach (var fc in _fieldControls)
        {
            fc.ValueFormat = format;
        }

        AdjustSize();
    }

    /// <summary>
    /// Sets the value format for a specific field.
    /// </summary>
    /// <param name="fieldIndex"></param>
    /// <param name="format"></param>
    public void SetValueFormat(int fieldIndex, ValueFormat format)
    {
        if (IsValidFieldIndex(fieldIndex))
        {
            _fieldControls[fieldIndex].ValueFormat = format;
        }

        AdjustSize();
    }

    /// <summary>
    /// Converts the text of every separator and every field to a single
    /// string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var index = 0; index < FieldCount; ++index)
        {
            sb.Append(_separatorControls[index].ToString());
            sb.Append(_fieldControls[index].ToString());
        }

        sb.Append(_separatorControls[FieldCount].ToString());

        return sb.ToString();
    }

    #endregion  // Public Methods

    #region Protected Properties

    /// <summary>
    /// Gets the default size for the control.
    /// </summary>
    protected override Size DefaultSize => CalculateMinimumSize();

    #endregion  // Protected Properties

    #region Protected Methods

    /// <summary>
    /// Raises the BackColorChanged event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);

        _backColorChanged = true;
    }

    /// <summary>
    /// Adjusts the size of the control when font is changed.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnFontChanged(EventArgs e)
    {
        var origin = Location;
        base.OnFontChanged(e);
        Location = origin;
        AdjustSize();
    }

    /// <summary>
    /// Raises the GotFocus event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        _focused = true;
        _fieldControls[0].TakeFocus(Direction.Forward, Selection.All);
    }

    /// <summary>
    /// Raises the LostFocus event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLostFocus(EventArgs e)
    {
        if (Focused) return;
        base.OnLostFocus(e);
        _focused = false;
    }

    /// <summary>
    /// Raises the MouseDown event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        HandleMouseDown(e.Location);
    }

    /// <summary>
    /// Raises the MouseEnter event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseEnter(EventArgs e)
    {
        if (_hasMouse) return;
        _hasMouse = true;
        base.OnMouseEnter(e);
    }

    /// <summary>
    /// Raises the MouseLeave event. 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseLeave(EventArgs e)
    {
        if (HasMouse) return;
        base.OnMouseLeave(e);
        _hasMouse = false;
    }

    /// <summary>
    /// Clears the background and fills it with background color. If control
    /// has a border, draws it.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var backColor = BackColor;

        if (!_backColorChanged)
        {
            if (!Enabled || ReadOnly)
            {
                backColor = SystemColors.Control;
            }
        }

        using (var backgroundBrush = new SolidBrush(backColor))
        {
            e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
        }

        var rectBorder = new Rectangle(ClientRectangle.Left, ClientRectangle.Top,
           ClientRectangle.Width - 1, ClientRectangle.Height - 1);

        switch (BorderStyle)
        {
            case BorderStyle.Fixed3D:

                if (Application.RenderWithVisualStyles)
                {
                    ControlPaint.DrawVisualStyleBorder(e.Graphics, rectBorder);
                }
                else
                {
                    ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
                }
                break;

            case BorderStyle.FixedSingle:

                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                   SystemColors.WindowFrame, ButtonBorderStyle.Solid);
                break;
        }
    }

    /// <summary>
    /// Ensures that any size change of control is constrained by allowed
    /// range.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);

        AdjustSize();
    }

    #endregion     // Protected Methods

    #region Private Properties

    private bool HasMouse => DisplayRectangle.Contains(PointToClient(MousePosition));

    #endregion  // Private Properties

    #region Private Methods

    private void AdjustSize()
    {
        var minSize = MinimumSize;

        var newWidth = minSize.Width;
        var newHeight = minSize.Height;

        if (Width > newWidth)
        {
            newWidth = Width;
        }

        if (Height > newHeight && !AutoHeight)
        {
            newHeight = Height;
        }

        Size = new Size(newWidth, newHeight);

        LayoutControls();
    }

    private Size CalculateMinimumSize()
    {
        var minimumSize = new Size();

        foreach (var fc in _fieldControls)
        {
            minimumSize.Width += fc.Width;
            minimumSize.Height = Math.Max(minimumSize.Height, fc.Height);
        }

        foreach (var sc in _separatorControls)
        {
            minimumSize.Width += sc.Width;
            minimumSize.Height = Math.Max(minimumSize.Height, sc.Height);
        }

        switch (BorderStyle)
        {
            case BorderStyle.Fixed3D:
                minimumSize.Width += 2 * _fixed3DOffset.Width;
                minimumSize.Height = GetSuggestedHeight();
                break;

            case BorderStyle.FixedSingle:
                minimumSize.Width += 2 * _fixedSingleOffset.Width;
                minimumSize.Height = GetSuggestedHeight();
                break;
        }

        return minimumSize;
    }

    private void Cleanup()
    {
        foreach (var sc in _separatorControls)
        {
            Controls.Remove(sc);
            sc.Dispose();
        }

        foreach (var fc in _fieldControls)
        {
            Controls.Remove(fc);
            fc.Dispose();
        }

        _separatorControls.Clear();
        _fieldControls.Clear();
    }

    private int GetSuggestedHeight()
    {
        _referenceTextBox.AutoSize = true;
        _referenceTextBox.BorderStyle = BorderStyle;
        _referenceTextBox.Font = Font;
        return _referenceTextBox.Height;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806", Justification = "What should be done if ReleaseDC() doesn't work?")]
    private static NativeMethods.TextMetric GetTextMetrics(nint hwnd, Font font)
    {
        var hdc = NativeMethods.GetWindowDC(hwnd);

        NativeMethods.TextMetric textMetric;
        var hFont = font.ToHfont();

        try
        {
            var hFontPrevious = NativeMethods.SelectObject(hdc, hFont);
            NativeMethods.GetTextMetrics(hdc, out textMetric);
            NativeMethods.SelectObject(hdc, hFontPrevious);
        }
        finally
        {
            NativeMethods.ReleaseDC(hwnd, hdc);
            NativeMethods.DeleteObject(hFont);
        }

        return textMetric;
    }

    private void HandleMouseDown(Point location)
    {
        var midPointsCount = FieldCount * 2 - 1;

        var midPoints = new Point[midPointsCount];

        for (var index = 0; index < FieldCount; ++index)
        {
            midPoints[index * 2] = _fieldControls[index].MidPoint;

            if (index < FieldCount - 1)
            {
                midPoints[index * 2 + 1] = _separatorControls[index + 1].MidPoint;
            }
        }

        var midPointsIndex = 0;

        var fieldIndex = 0;
        var direction = Direction.Forward;

        while (midPointsIndex < midPointsCount)
        {
            if (location.X < midPoints[midPointsIndex].X)
            {
                break;
            }
            else if (direction == Direction.Forward)
            {
                direction = Direction.Reverse;
            }
            else
            {
                direction = Direction.Forward;
                ++fieldIndex;
            }

            ++midPointsIndex;
        }

        if (midPointsIndex == midPointsCount)
        {
            direction = Direction.Reverse;
        }

        _fieldControls[fieldIndex].TakeFocus(direction, Selection.None);
    }

    private void InitializeControls()
    {
        Cleanup();

        base.BackColor = SystemColors.Window;

        _backColorChanged = false;

        for (var index = 0; index < FieldCount; ++index)
        {
            var fc = new FieldControl();

            fc.CreateControl();

            fc.FieldIndex = index;
            fc.Name = Resources.FieldControlName + index.ToString(CultureInfo.InvariantCulture);
            fc.Parent = this;
            fc.ReadOnly = ReadOnly;

            fc.CedeFocusEvent += OnFocusCeded;
            fc.FieldChangedEvent += OnFieldChanged;
            fc.GotFocus += OnFieldGotFocus;
            fc.KeyDown += OnFieldKeyDown;
            fc.KeyPress += OnFieldKeyPressed;
            fc.KeyUp += OnFieldKeyUp;
            fc.LostFocus += OnFieldLostFocus;
            fc.PreviewKeyDown += OnFieldPreviewKeyDown;

            fc.FieldSizeChangedEvent += OnFieldSizeChanged;
            fc.FieldValidatedEvent += OnFieldValidated;

            fc.Click += OnSubControlClicked;
            fc.DoubleClick += OnSubControlDoubleClicked;
            fc.MouseClick += OnSubControlMouseClicked;
            fc.MouseDoubleClick += OnSubControlMouseDoubleClicked;
            fc.MouseEnter += OnSubControlMouseEntered;
            fc.MouseHover += OnSubControlMouseHovered;
            fc.MouseLeave += OnSubControlMouseLeft;
            fc.MouseMove += OnSubControlMouseMoved;

            _fieldControls.Add(fc);

            Controls.Add(fc);
        }

        for (var index = 0; index < FieldCount + 1; ++index)
        {
            var sc = new SeparatorControl();

            sc.CreateControl();

            sc.Name = Resources.SeparatorControlName + index.ToString(CultureInfo.InvariantCulture);
            sc.Parent = this;
            sc.ReadOnly = ReadOnly;
            sc.SeparatorIndex = index;

            sc.SeparatorMouseEvent += OnSeparatorMouseEvent;
            sc.SeparatorSizeChangedEvent += OnSeparatorSizeChanged;

            sc.Click += OnSubControlClicked;
            sc.DoubleClick += OnSubControlDoubleClicked;
            sc.MouseClick += OnSubControlMouseClicked;
            sc.MouseDoubleClick += OnSubControlMouseDoubleClicked;
            sc.MouseEnter += OnSubControlMouseEntered;
            sc.MouseHover += OnSubControlMouseHovered;
            sc.MouseLeave += OnSubControlMouseLeft;
            sc.MouseMove += OnSubControlMouseMoved;

            _separatorControls.Add(sc);

            Controls.Add(sc);
        }

        for (var index = 0; index < _separatorControls.Count; ++index)
        {
            string text;

            if (index == 0)
            {
                text = "<";
            }
            else if (index == _separatorControls.Count - 1)
            {
                text = ">";
            }
            else
            {
                text = "><";
            }

            _separatorControls[index].Text = text;
        }
    }

    private bool IsValidFieldIndex(int fieldIndex)
    {
        return fieldIndex >= 0 && fieldIndex < _fieldControls.Count;
    }

    private bool IsValidSeparatorIndex(int separatorIndex)
    {
        return separatorIndex >= 0 && separatorIndex < _separatorControls.Count;
    }

    private void LayoutControls()
    {
        SuspendLayout();

        var difference = Width - MinimumSize.Width;

        Debug.Assert(difference >= 0);

        var offsetCount = 2 * FieldCount;

        var div = difference / offsetCount;
        var mod = difference % offsetCount;

        var offsets = new int[offsetCount];

        for (var index = 0; index < offsetCount; ++index)
        {
            offsets[index] = div;

            if (index < mod)
            {
                ++offsets[index];
            }
        }

        var x = 0;
        var y = 0;

        switch (BorderStyle)
        {
            case BorderStyle.Fixed3D:
                x = _fixed3DOffset.Width;
                y = _fixed3DOffset.Height;
                break;

            case BorderStyle.FixedSingle:
                x = _fixedSingleOffset.Width;
                y = _fixedSingleOffset.Height;
                break;
        }

        for (var index = 0; index < FieldCount; ++index)
        {
            _separatorControls[index].Location = new Point(x, y);
            x += _separatorControls[index].Width;

            x += offsets[2 * index];

            _fieldControls[index].Location = new Point(x, y);
            x += _fieldControls[index].Width;

            x += offsets[2 * (FieldCount - index) - 1];
        }

        _separatorControls[FieldCount].Location = new Point(x, y);

        ResumeLayout(false);
    }

    private void OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        if (FieldChangedEvent != null)
        {
            FieldChangedEvent(this, e);
        }

        OnTextChanged(EventArgs.Empty);
    }

    private void OnFieldGotFocus(object sender, EventArgs e)
    {
        if (_focused) return;
        _focused = true;
        base.OnGotFocus(e);
    }

    private void OnFieldKeyDown(object sender, KeyEventArgs e)
    {
        OnKeyDown(e);
    }

    private void OnFieldKeyPressed(object sender, KeyPressEventArgs e)
    {
        OnKeyPress(e);
    }

    private void OnFieldKeyUp(object sender, KeyEventArgs e)
    {
        OnKeyUp(e);
    }

    private void OnFieldLostFocus(object sender, EventArgs e)
    {
        if (Focused) return;
        base.OnLostFocus(EventArgs.Empty);
        _focused = false;
    }

    private void OnFieldPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
        OnPreviewKeyDown(e);
    }

    private void OnFieldSizeChanged(object sender, EventArgs e)
    {
        AdjustSize();
        Invalidate();
    }

    private void OnFieldValidated(object sender, FieldValidatedEventArgs e)
    {
        if (FieldValidatedEvent != null)
        {
            FieldValidatedEvent(this, e);
        }
    }

    private void OnFocusCeded(object sender, CedeFocusEventArgs e)
    {
        switch (e.Action)
        {
            case Action.Home:

                _fieldControls[0].TakeFocus(Action.Home);
                return;

            case Action.End:

                _fieldControls[FieldCount - 1].TakeFocus(Action.End);
                return;

            case Action.Trim:

                if (e.FieldIndex == 0)
                {
                    return;
                }

                _fieldControls[e.FieldIndex - 1].TakeFocus(Action.Trim);
                return;
        }

        if (e is { Direction: Direction.Reverse, FieldIndex: 0 }
            || (e.Direction == Direction.Forward && e.FieldIndex == FieldCount - 1))
        {
            return;
        }

        var fieldIndex = e.FieldIndex;

        if (e.Direction == Direction.Forward)
        {
            ++fieldIndex;
        }
        else
        {
            --fieldIndex;
        }

        _fieldControls[fieldIndex].TakeFocus(e.Direction, e.Selection);
    }

    private void OnSeparatorMouseEvent(object sender, SeparatorMouseEventArgs e)
    {
        if (e.SeparatorIndex == 0)
        {
            _fieldControls[0].TakeFocus(Direction.Forward, Selection.None);
        }
        else if (e.SeparatorIndex == FieldCount)
        {
            _fieldControls[FieldCount - 1].TakeFocus(Direction.Reverse, Selection.None);
        }
        else
        {
            var location = PointToClient(e.Location);
            HandleMouseDown(location);
        }
    }

    private void OnSeparatorSizeChanged(object sender, EventArgs e)
    {
        AdjustSize();
        Invalidate();
    }

    private void OnSubControlClicked(object sender, EventArgs e)
    {
        OnClick(e);
    }

    private void OnSubControlDoubleClicked(object sender, EventArgs e)
    {
        OnDoubleClick(e);
    }

    private void OnSubControlMouseClicked(object sender, MouseEventArgs e)
    {
        OnMouseClick(e);
    }

    private void OnSubControlMouseDoubleClicked(object sender, MouseEventArgs e)
    {
        OnMouseDoubleClick(e);
    }

    private void OnSubControlMouseEntered(object sender, EventArgs e)
    {
        OnMouseEnter(e);
    }

    private void OnSubControlMouseHovered(object sender, EventArgs e)
    {
        OnMouseHover(e);
    }

    private void OnSubControlMouseLeft(object sender, EventArgs e)
    {
        OnMouseLeave(e);
    }

    private void OnSubControlMouseMoved(object sender, MouseEventArgs e)
    {
        OnMouseMove(e);
    }

    private void Parse(string text)
    {
        Clear();

        if (text == null)
        {
            return;
        }

        var sb = new StringBuilder();

        for (var index = 0; index < FieldCount; ++index)
        {
            sb.Append(_separatorControls[index].RegExString);

            sb.Append("(");
            sb.Append(_fieldControls[index].RegExString);
            sb.Append(")");
        }

        sb.Append(_separatorControls[FieldCount].RegExString);

        var regex = new Regex(sb.ToString());

        var match = regex.Match(text);

        if (!match.Success) return;

        if (match.Groups.Count != FieldCount + 1) return;
        for (var index = 0; index < FieldCount; ++index)
        {
            _fieldControls[index].Text = match.Groups[index + 1].Value;
        }
    }

    #endregion     // Private Methods

    #region Private Data

    private readonly TextBox _referenceTextBox = new();

    private int _fieldCount = 3;

    private bool _autoHeight = true;

    private readonly List<FieldControl> _fieldControls = new();
    private readonly List<SeparatorControl> _separatorControls = new();

    private readonly Size _fixed3DOffset = new(3, 3);
    private readonly Size _fixedSingleOffset = new(2, 2);

    private BorderStyle _borderStyle = BorderStyle.Fixed3D;

    private bool _readOnly;

    private bool _backColorChanged;

    private bool _focused;

    private bool _hasMouse;

    #endregion  Private Data
}
