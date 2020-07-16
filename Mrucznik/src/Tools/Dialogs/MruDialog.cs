using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;

namespace Mrucznik.Systems
{
    public class MruDialog
    {
        private readonly Dialog _dialog;

        public MruDialog(Dialog dialog)
        {
            _dialog = dialog;
            dialog.Response += DialogOnResponse;
        }

        private void DialogOnResponse(object? sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton == DialogButton.Left)
            {
                
            }
            else
            {
                
            }
        }
    }
}