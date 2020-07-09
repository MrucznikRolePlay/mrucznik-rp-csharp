using System.Collections.Generic;
using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class Flow : IDialogFlow
    {
        Queue<Dialog> _dialogs = new Queue<Dialog>();
        
        public void Start()
        {
            Dialog dialog;
            if (_dialogs.TryPeek(out dialog))
            {
                //dialog.Show();
            }
        }

        public void Next()
        {
        }

        public void Previous()
        {
            throw new System.NotImplementedException();
        }

        public void AddDialog(Dialog dialog)
        {
            throw new System.NotImplementedException();
        }
    }
}