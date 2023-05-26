using System.Collections;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace FlexFieldTextBoxWF;

public class FlexFieldTextBoxDesigner : ControlDesigner
{
    public override SelectionRules SelectionRules
    {      
        get
        {
            var control = (FlexFieldTextBox)Control;

            if ( control.AutoHeight )
            {
                return SelectionRules.Moveable | SelectionRules.Visible |
                       SelectionRules.RightSizeable | SelectionRules.LeftSizeable;
            }

            return SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible;
        }
    }

    public override IList SnapLines
    {
        get
        {
            var control = (FlexFieldTextBox)Control;

            var snapLines = base.SnapLines;

            snapLines.Add( new SnapLine( SnapLineType.Baseline, control.Baseline ) );

            return snapLines;
        }
    }
}