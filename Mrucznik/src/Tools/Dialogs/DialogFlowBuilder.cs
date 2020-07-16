using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public abstract class DialogFlowBuilder
    {
        Dialog 
        
        protected DialogFlowBuilder()
        {
        }

        public DialogFlowBuilder Add(Dialog builder)
        {
            dialog.Response += (sender, args) =>
            {
                if (args.DialogButton == DialogButton.Left)
                {
                    LeftButtonAction(sender, args);
                }
            };
            dialog.Response += (sender, args) =>
            {
                if (args.DialogButton == DialogButton.Right)
                {
                    RightButtonAction(sender, args);
                }
            };
            return this;
        }
    }
}