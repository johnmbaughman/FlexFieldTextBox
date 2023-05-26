using System.Text.RegularExpressions;
using System.Text;

namespace FlexFieldTextBoxWF;

internal class SeparatorControl : Control
{
    public event EventHandler<SeparatorMouseEventArgs> SeparatorMouseEvent;
    public event EventHandler<EventArgs> SeparatorSizeChangedEvent;

    public Point MidPoint
    {
        get
        {
            var midPoint = Location;
            midPoint.Offset(Width / 2, Height / 2);
            return midPoint;
        }
    }

    public new Size MinimumSize => CalculateMinimumSize();

    public bool ReadOnly
    {
        get => _readOnly;
        set
        {
            _readOnly = value;
            Invalidate();
        }
    }

    public string RegExString
    {
        get
        {
            var sb = new StringBuilder();

            foreach (var c in Text)
            {
                sb.Append("[");
                sb.Append(Regex.Escape(c.ToString()));
                sb.Append("]");
            }

            return sb.ToString();
        }
    }

    public int SeparatorIndex { get; set; }

    public override string Text
    {
        get => base.Text;
        set
        {
            base.Text = value;
            Size = MinimumSize;
        }
    }

    public override string ToString()
    {
        return Text;
    }

    public SeparatorControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.UserPaint, true);

        BackColor = SystemColors.Window;

        Size = MinimumSize;
        TabStop = false;
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        Size = MinimumSize;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (SeparatorMouseEvent == null) return;
        var args = new SeparatorMouseEventArgs
        {
            Location = PointToScreen(e.Location),
            SeparatorIndex = SeparatorIndex
        };

        SeparatorMouseEvent(this, args);
    }

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

        var foreColor = ForeColor;

        if (!Enabled)
        {
            foreColor = SystemColors.GrayText;
        }
        else if (ReadOnly)
        {
            if (!_backColorChanged)
            {
                foreColor = SystemColors.WindowText;
            }
        }

        using (var backgroundBrush = new SolidBrush(backColor))
        {
            e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
        }

        TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle,
           foreColor, TextFormatFlags);

    }

    protected override void OnParentBackColorChanged(EventArgs e)
    {
        base.OnParentBackColorChanged(e);

        BackColor = Parent.BackColor;

        _backColorChanged = true;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);

        if (SeparatorSizeChangedEvent != null)
        {
            SeparatorSizeChangedEvent(this, EventArgs.Empty);
        }
    }

    private Size CalculateMinimumSize()
    {
        var g = Graphics.FromHwnd(Handle);

        var minimumSize = TextRenderer.MeasureText(g,
           Text, Font, Size, TextFormatFlags);

        g.Dispose();

        return minimumSize;
    }

    private static readonly TextFormatFlags TextFormatFlags = TextFormatFlags.HorizontalCenter
                                                              | TextFormatFlags.NoPrefix
                                                              | TextFormatFlags.SingleLine
                                                              | TextFormatFlags.NoPadding;

    private bool _readOnly;
    private bool _backColorChanged;
}