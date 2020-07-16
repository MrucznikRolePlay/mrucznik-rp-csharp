using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class MessageDialogBuilder : DialogBuilder<MessageDialog, MessageDialogBuilder, MessageDialogBuilder.IFrom>,
        MessageDialogBuilder.IFrom
    {
        public new interface IFrom
        {
            MessageDialog Build();
        }

        public override IFrom Continue()
        {
            return this;
        }

        public override MessageDialog Build()
        {
            var dialog = new MessageDialog(Caption, Message, LeftButton, RightButton);
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
            return dialog;
        }
    }
}