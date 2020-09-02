using System.Globalization;
using System.Linq;
using Mrucznik.Objects;
using SampSharp.Core.Natives;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using SampSharp.Streamer;
using SampSharp.Streamer.World;

namespace Mrucznik.Commands
{
    public class ObjectStreamerCommands
    {
        [Command("objects")]
        private static void StreamerObjects(BasePlayer sender)
        {
            sender.SendClientMessage($"Streamer objects: {DynamicObject.All.Count().ToString()}");
        }

        [Command("objectslist")]
        private static void StreamerObjectsList(BasePlayer sender, int page = 0)
        {
            var objects = DynamicObject.All.Skip(50 * page).Take(50);
            var tablistDialog = new TablistDialog($"Obiekty - strona {page.ToString()}",
                new[] {"ID", "Model", "Name", "X", "Y", "Z", "VW", "INT"},
                "Teleport", "Następny");
            bool ok = false;
            foreach (var o in objects)
            {
                ok = true;
                tablistDialog.Add(o.Id.ToString(), o.ModelId.ToString(), o.ToString(), 
                    o.Position.X.ToString(CultureInfo.CurrentCulture), 
                    o.Position.Y.ToString(CultureInfo.CurrentCulture), 
                    o.Position.Z.ToString(CultureInfo.CurrentCulture),
                    o.World.ToString(), o.Interior.ToString());
            }

            if (!ok)
            {
                sender.SendClientMessage("Brak obiektów na tej stronie.");
                return;
            }

            tablistDialog.Response += (o, args) =>
            {
                if (args.DialogButton == DialogButton.Left)
                {
                    sender.SendClientMessage($"Zostałeś teleportowany do obiektu: {args.InputText}");
                    // sender.SetPositionFindZ(DynamicObject.All.ElementAt(page * 50 + args.ListItem).Position);
                    sender.Position = DynamicObject.All.ElementAt(page * 50 + args.ListItem).Position;
                }
                else
                {
                    StreamerObjectsList(sender, page + 1);
                }
            };

            tablistDialog.Show(sender);
        }

        [Command("gotoobject")]
        private static void GotoObject(BasePlayer sender, int objectId)
        {
            var o = DynamicObject.Find(objectId);
            if (o != null)
            {
                sender.SendClientMessage($"Zostałeś teleportowany do obiektu o ID {objectId}, pozycja: {o.Position}.");
                sender.Position = o.Position;
            }
            else
            {
                sender.SendClientMessage($"Nie znaleziono obiektu o ID {objectId}.");
            }
        }

        [Command("selectobject", Shortcut = "sel")]
        private static void SelectObject(Player sender, int objectId = -1)
        {
            if (objectId == -1)
            {
                sender.ObjectEditorState = ObjectEditorState.Edit;
                GlobalObject.Select(sender);
            }
            else
            {
                var o = DynamicObject.Find(objectId);
                if (o == null || !o.IsValid)
                {
                    sender.SendClientMessage($"Nie znaleziono obiektu o id {objectId}.");
                    return;
                }
                
                sender.ObjectEditorState = ObjectEditorState.Edit;
                o.Edit(sender);
            }
        }

        [Command("deleteobject", "odel", "odelete", "deleteo", Shortcut = "delo")]
        private static void DeleteObject(Player sender, int objectId=-1)
        {
            if (objectId == -1)
            {
                sender.ObjectEditorState = ObjectEditorState.Delete;
                GlobalObject.Select(sender);
            }
            else
            {
                var o = DynamicObject.Find(objectId);
                if (o == null || !o.IsValid)
                {
                    sender.SendClientMessage($"Nie znaleziono obiektu o id {objectId}.");
                    return;
                }
                
                sender.SendClientMessage($"Usunąłeś obiekt {o}");
                o.Dispose();
            }
        }

        [Command("createobject", Shortcut = "cobject")]
        private static void CreateObject(BasePlayer sender, int modelId=-1)
        {
            sender.SendClientMessage("Not implemented");
        }
    }
}