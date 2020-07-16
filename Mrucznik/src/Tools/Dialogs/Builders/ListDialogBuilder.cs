using System.Collections.Generic;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class ListDialogBuilder : DialogBuilder<ListDialog, ListDialogBuilder, ListDialogBuilder.IFrom>,
        ListDialogBuilder.IFrom
    {
        private readonly IList<string> _items = new List<string>();

        public new interface IFrom : IBuild, IRow
        {
            
        }
        
        public interface IRow
        {
            IFrom WithRow(string row);
        }
        
        public interface IBuild
        {
            ListDialog Build();
        }
        
        public override IFrom Continue()
        {
            return this;
        }

        public IFrom WithRow(string row)
        {
            _items.Add(row);
            return this;
        }

        public override ListDialog Build()
        {
            var dialog = new ListDialog(Caption, LeftButton, RightButton);
            dialog.AddItems(_items);
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