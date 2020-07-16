using System;
using System.Collections;
using System.Collections.Generic;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;

namespace Mrucznik.Systems
{
    public class DialogFlow : IDialog
    {
        private Dialog _startDialog;
        private Dialog _previousDialog;
        private int _currentDialog;
        private readonly Dictionary<int, DialogResponseEventArgs> _responses = new Dictionary<int, DialogResponseEventArgs>();
        
        public void Show(BasePlayer player)
        {
            _startDialog.Show(player);
            _currentDialog = 0;
        }

        public DialogFlow AddDialog(Dialog dialog)
        {
            _startDialog ??= dialog;

            if (dialog.Button2 != "")
            {
                // Add previous dialog button
                Dialog previous = _previousDialog ?? _startDialog;
                dialog.Response += (sender, args) =>
                {
                    if (args.DialogButton == DialogButton.Left)
                    {
                        previous.Show(args.Player);
                        _currentDialog--;
                    }
                };
            }

            if (_previousDialog != null)
            {
                // Add next dialog button
                DialogButton button = _previousDialog.Button2 == "" ? DialogButton.Left : DialogButton.Right;
                _previousDialog.Response += (sender, args) =>
                {
                    if (args.DialogButton == button)
                    {
                        dialog.Show(args.Player);
                        _responses[_currentDialog] = args;
                        _currentDialog++;
                    }
                };
            }

            _previousDialog = dialog;
            return this;
        }

        public IDialog End(EventHandler<Dictionary<int, DialogResponseEventArgs>> endAction)
        {
            DialogButton button = _previousDialog.Button2 == "" ? DialogButton.Left : DialogButton.Right;
            _previousDialog.Response += (sender, args) =>
            {
                if (args.DialogButton == button)
                {
                    endAction.Invoke(sender, _responses);
                }
            };
            return this;
        }
    }
}