using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class InputDialogBuilder : DialogBuilder<InputDialog, InputDialogBuilder, InputDialogBuilder.IFrom>,
        InputDialogBuilder.IFrom
    {
        public new interface IFrom : IBuild
        {
            IBuild PasswordStyle();
        }
        
        public interface IBuild
        {
            InputDialog Build();
        }
        
        
        private bool _isPassword = false;
        
        public override IFrom Continue()
        {
            return this;
        }
        
        public IBuild PasswordStyle()
        {
            _isPassword = true;
            return this;
        }
        
        public override InputDialog Build()
        {
            var dialog = new InputDialog(Caption, Message, _isPassword, LeftButton, RightButton);
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