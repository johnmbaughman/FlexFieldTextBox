namespace FlexFieldTextBoxWPF;

internal class CedeFocusEventArgs
{
    public Action Action { get; set; }

    public Direction Direction { get; set; }

    public int FieldIndex { get; set; }

    public Selection Selection { get; set; }
}

internal enum Action
{
    None,
    Trim,
    Home,
    End
}

internal enum Direction
{
    Forward,
    Reverse
}

internal enum Selection
{
    None,
    All
}