using System.Collections.Generic;
using System.Linq;
using Mrucznik.Systems;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using SampSharp.Streamer;
using SampSharp.Streamer.Definitions;
using SampSharp.Streamer.World;

namespace Mrucznik.Commands
{
    public class ObjectStreamerCommands
    {
        [Command("objects")]
        private static void StreamerObjects(BasePlayer sender)
        {
            sender.SendClientMessage($"Streamer objects: {SampSharp.Streamer.World.DynamicObject.All.Count()}");
        }
        
        [Command("objectslist")]
        private static void StreamerObjectsList(BasePlayer sender, int page=0)
        {
            var objects = DynamicObject.All.Skip(50*page).Take(50);
            var tablistDialog = new TablistDialog($"Obiekty - strona {page}", 
                new []{"ID", "Model", "VW", "INT"}, 
                "Teleport", "Następny");
            bool ok = false;
            foreach (var o in objects)
            {
                ok = true;
                tablistDialog.Add(o.Id.ToString(), o.ModelId.ToString(), o.World.ToString(), o.Interior.ToString());
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
                    sender.SetPositionFindZ(DynamicObject.All.ElementAt(page * 50 + args.ListItem).Position);
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
            var o = SampSharp.Streamer.World.DynamicObject.Find(objectId);
            if(o != null)
            {
                sender.SendClientMessage($"Zostałeś teleportowany do obiektu o ID {objectId}, pozycja: {o.Position}.");
            }
            else
            {
                sender.SendClientMessage($"Nie znaleziono obiektu o ID {objectId}.");
            }
        }

        [Command("selectobject")]
        private static void SelectObject(BasePlayer sender)
        {
            GlobalObject.Select(sender);
        }
    }
}