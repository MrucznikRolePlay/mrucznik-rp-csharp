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

        private readonly Dictionary<int, DialogResponseEventArgs> _responses =
            new Dictionary<int, DialogResponseEventArgs>();

        private bool _canExit;
        private readonly DialogButton _nextButton;
        private readonly DialogButton _previousButton;

        public DialogFlow() : this(true, DialogButton.Left, DialogButton.Right)
        {
        }

        public DialogFlow(bool canExit, DialogButton nextButton = DialogButton.Left,
            DialogButton previousButton = DialogButton.Right)
        {
            _canExit = canExit;
            _nextButton = nextButton;
            _previousButton = previousButton;
        }

        public void Show(BasePlayer player)
        {
            _startDialog.Show(player);
            _currentDialog = 0;
        }

        public DialogFlow AddDialog(Dialog dialog)
        {
            _startDialog ??= dialog;
            AddPreviousButtonAction(dialog);
            AddNextButtonAction(_previousDialog, dialog);
            _previousDialog = dialog;
            return this;
        }

        public DialogFlow AddDialogs(List<Dialog> dialogs)
        {
            foreach (var dialog in dialogs)
            {
                AddDialog(dialog);
            }

            return this;
        }

        private void AddPreviousButtonAction(Dialog dialog)
        {
            Dialog previous = _previousDialog ?? _startDialog;
            dialog.Response += (sender, args) =>
            {
                if (args.DialogButton == _previousButton)
                {
                    if (dialog == previous && _canExit) return;
                    previous.Show(args.Player);
                    _currentDialog--;
                }
            };
        }

        private void AddNextButtonAction(Dialog dialog, Dialog nextDialog)
        {
            if (dialog == null) return;

            _previousDialog.Response += (sender, args) =>
            {
                if (args.DialogButton == _nextButton)
                {
                    nextDialog.Show(args.Player);
                    _responses[_currentDialog] = args;
                    _currentDialog++;
                }
            };
        }

        public IDialog End(EventHandler<Dictionary<int, DialogResponseEventArgs>> endAction)
        {
            _previousDialog.Response += (sender, args) =>
            {
                if (args.DialogButton == _nextButton)
                {
                    _responses[_currentDialog] = args;
                    endAction.Invoke(sender, _responses);
                }
            };
            return this;
        }
    }
}