using System.Collections.Generic;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class TablistDialogBuilder : DialogBuilder<TablistDialog, TablistDialogBuilder, TablistDialogBuilder.IFrom>,
        TablistDialogBuilder.IFrom,
        TablistDialogBuilder.IRow
    {
        private string[] _headers;
        private readonly LinkedList<string[]> _list = new LinkedList<string[]>();
        
        public new interface IFrom : IRow
        {
            IRow WithHeaders(params string[] headers);
        }
        
        public interface IRow
        {
            IRow WithRow(params string[] row);
            TablistDialog Build();
        }

        public override IFrom Continue()
        {
            return this;
        }

        public IRow WithHeaders(params string[] headers)
        {
            _headers = headers;
            return this;
        }

        public IRow WithRow(params string[] row)
        {
            _list.AddLast(row);
            return this;
        }

        public override TablistDialog Build()
        {
            var dialog = new TablistDialog(Caption, _headers, LeftButton, RightButton);
            foreach (var row in _list)
            {
                dialog.Add(row);
            }
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