using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Mrucznik.Objects;
using Mruv.Objects;
using SampSharp.Core.Natives;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.SAMP.Commands.Parameters;
using SampSharp.GameMode.World;
using SampSharp.Streamer;
using SampSharp.Streamer.World;

namespace Mrucznik.Commands
{
    public class ObjectStreamerCommands
    {
        [CommandGroup("object", "o")]
        class ObjectCommands
        {
            [Command("count")]
            private static void StreamerObjects(BasePlayer sender)
            {
                sender.SendClientMessage($"Streamer objects: {DynamicObject.All.Count().ToString()}");
            }

            [Command("list", "l")]
            private static void StreamerObjectsList(Player sender, int page = 0)
            {
                var (tablistDialog, ok) = sender.ObjectEditor.GetObjectListDialog(page, "Teleport", (o, args) =>
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
                });
                if (!ok)
                {
                    sender.SendClientMessage("Brak obiektów na tej stronie.");
                    return;
                }

                tablistDialog.Show(sender);
            }

            [Command("goto", "tp")]
            private static void GotoObject(BasePlayer sender, int objectId)
            {
                var o = DynamicObject.Find(objectId);
                if (o != null)
                {
                    sender.SendClientMessage(
                        $"Zostałeś teleportowany do obiektu o ID {objectId}, pozycja: {o.Position}.");
                    sender.Position = o.Position;
                }
                else
                {
                    sender.SendClientMessage($"Nie znaleziono obiektu o ID {objectId}.");
                }
            }

            [Command("select", "sel", "s", "edit", "e")]
            private static void SelectObject(Player sender, int objectId = -1)
            {
                if (objectId == -1)
                {
                    sender.ObjectEditor.SelectObjectMode();
                }
                else
                {
                    var o = DynamicObject.Find(objectId);
                    if (o == null || !o.IsValid)
                    {
                        sender.SendClientMessage($"Nie znaleziono obiektu o id {objectId}.");
                        return;
                    }

                    sender.ObjectEditor.EditObjectMode((MruDynamicObject)o);
                }
            }

            [Command("clone")]
            private static void CloneObject(Player sender, int objectId = -1)
            {
                if (objectId == -1)
                {
                    sender.ObjectEditor.CloneObjectMode();
                }
                else
                {
                    var o = DynamicObject.Find(objectId);
                    if (o == null || !o.IsValid)
                    {
                        sender.SendClientMessage($"Nie znaleziono obiektu o id {objectId}.");
                        return;
                    }

                    sender.ObjectEditor.EditObjectMode((MruDynamicObject)o);
                }
            }

            [Command("multiselect", "msel")]
            private static void MultiSelect(Player sender, [Parameter(typeof(ListParamType))]List<int> objectIds = null)
            {
                if (objectIds?.Count > 0)
                {
                    sender.SendClientMessage($"Wybrałeś obiekty: {String.Join(", ", objectIds)}");
                    return;
                }
                sender.SendClientMessage(
                    "Wybierz obiekty, które chcesz edytować. Wpisz komendę /o cancel aby anulować wybór obiektów.");
                sender.ObjectEditor.MultiSelectMode();
            }
            
            [Command("cancel")]
            private static void CancelSelect(Player sender, [Parameter(typeof(ListParamType))]List<int> objectIds = null)
            {
                sender.SendClientMessage("Anulowałeś zaznaczenie wszystkich wybranych obiekty.");
                sender.ObjectEditor.CancelSelection();
            }

            [Command("delete", "del", "d")]
            private static void DeleteObject(Player sender, int objectId = -1)
            {
                if (objectId == -1)
                {
                    sender.ObjectEditor.DeleteObjectMode();
                }
                else
                {
                    var o = (MruDynamicObject)DynamicObject.Find(objectId);
                    if (o == null || !o.IsValid)
                    {
                        sender.SendClientMessage($"Nie znaleziono obiektu o id {objectId}.");
                        return;
                    }

                    sender.SendClientMessage($"Usunąłeś obiekt {o}");
                    o.ApiDelete();
                }
            }

            [Command("create", "c")]
            private static void CreateObject(Player sender, int modelId = -1)
            {
                if (modelId != -1)
                {
                }
                else // Select model from dialog
                {
                    sender.ObjectEditor.GetModelListDialog(0, "Stwórz", (o, args) => { });
                }
            }

            [Command("texture", "t")]
            private static void EditTexture(BasePlayer sender, int objectId, int textureIndex = 0)
            {
                if (textureIndex < 0 || textureIndex > 15)
                {
                    sender.SendClientMessage("Index textury powinien zawierać się w przedziale od 0 do 15.");
                    return;
                }
            }

            [CommandGroup("group", "g")]
            class ObjectGroupCommands
            {
                [Command("delete", "d")]
                private static void DeleteGroup(Player sender)
                {
                    sender.SendClientMessage("Usunąłeś grupę obiektów.");
                }
                
                [Command("edit", "e")]
                private static void EditGroup(Player sender)
                {
                    sender.SendClientMessage("Edytujesz grupę obiektów.");
                }
            }

            private static void ModelsDialogCreate(Player player, int page)
            {
                var action = new EventHandler<DialogResponseEventArgs>((o, args) =>
                {
                    if (args.DialogButton == DialogButton.Left)
                    {
                        args.Player.SendClientMessage($"Stworzyłeś obiekt o modelu {args.InputText}.");
                    }
                    else
                    {
                        ModelsDialogCreate(player, page + 1);
                    }
                });
                var (dialog, ok) = player.ObjectEditor.GetModelListDialog(page, "Stwórz", action);
                if (!ok)
                {
                    player.SendClientMessage("Brak modeli na następnej stronie.");
                    (dialog, _) = player.ObjectEditor.GetModelListDialog(page - 1, "Stwórz", action);
                }

                dialog.Show(player);
            }
        }
    }
}