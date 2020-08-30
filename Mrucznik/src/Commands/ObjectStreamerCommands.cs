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
        [Command("streamerobjects")]
        private static void StreamerObjects(BasePlayer sender)
        {
            sender.SendClientMessage($"Streamer objects: {SampSharp.Streamer.World.DynamicObject.All.Count()}");
        }
        
        [Command("updatestreamer")]
        private static void UpdateStreamer(BasePlayer sender)
        {
            Streamer.Update(sender);
            DynamicObject.ToggleAllItems(sender, true, new DynamicObject[]{});
            sender.SendClientMessage("Streamer updated.");
        }
        
        [Command("streamerobjectslist")]
        private static void StreamerObjectsList(BasePlayer sender, int page=0)
        {
            var objects = DynamicObject.All.Skip(50*page).Take(50);
            var tablistDialog = new TablistDialog($"Obiekty - strona {page}", 
                new []{"ID", "Model", "VW", "INT"}, 
                "Goto", "Next");
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
                sender.SendClientMessage($"You have been teleported to object {objectId}, position: {o.Position}.");
            }
            else
            {
                sender.SendClientMessage($"Object of id {objectId} not found.");
            }
        }
    }
}