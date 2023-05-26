using System.Runtime.InteropServices;

namespace FlexFieldTextBoxWF;

internal static class NativeMethods
{
    [DllImport( "user32.dll" )]
    public static extern nint GetWindowDC( nint hWnd );

    [DllImport( "user32.dll" )]
    public static extern int ReleaseDC( nint hWnd, nint hDC );

    [DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
    [return: MarshalAs( UnmanagedType.Bool )]
    public static extern bool GetTextMetrics( nint hdc, out TextMetric lptm );

    [DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
    public static extern nint SelectObject( nint hdc, nint hgdiobj );

    [DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
    [return: MarshalAs( UnmanagedType.Bool )]
    public static extern bool DeleteObject( nint hdc );

    [Serializable, StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
    public struct TextMetric
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public char tmFirstChar;
        public char tmLastChar;
        public char tmDefaultChar;
        public char tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    }
}