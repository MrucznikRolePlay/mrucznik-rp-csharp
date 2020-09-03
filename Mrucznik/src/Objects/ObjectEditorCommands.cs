using System;
using System.Globalization;
using System.Linq;
using Mrucznik.Objects;
using Mruv.Objects;
using SampSharp.Core.Natives;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
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
                sender.ObjectEditor.ObjectEditorState = ObjectEditorState.Edit;
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

                sender.ObjectEditor.ObjectEditorState = ObjectEditorState.Edit;
                o.Edit(sender);
            }
        }

        [Command("multiselect", Shortcut = "msel")]
        private static void MultiSelect(Player sender, params int[] objectIds)
        {
            if (objectIds.Length > 0)
            {
                foreach (var objectId in objectIds)
                {
                    var o = DynamicObject.Find(objectId);
                    if (o == null || !o.IsValid)
                    {
                        sender.SendClientMessage($"Nie znaleziono obiektu o id {objectId}.");
                        return;
                    }
                }
            }
            else
            {
                sender.SendClientMessage(
                    "Wybierz obiekty, które chcesz edytować, lub wpisz komendę /msel jeszcze raz aby anulować.");
                sender.ObjectEditor.ObjectEditorState = ObjectEditorState.MultiSelect;
                GlobalObject.Select(sender);
            }
        }

        [Command("deleteobject", "odel", "odelete", "deleteo", Shortcut = "delo")]
        private static void DeleteObject(Player sender, int objectId = -1)
        {
            if (objectId == -1)
            {
                sender.ObjectEditor.ObjectEditorState = ObjectEditorState.Delete;
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
        private static void CreateObject(Player sender, int modelId = -1)
        {
            if (modelId != -1)
            {
            }
            else // Select model from dialog
            {
                sender.ObjectEditor.GetModelListDialog(0, "Stwórz", (o, args) =>
                {
                    
                });
            }
        }

        [Command("edittexture")]
        private static void EditTexture(BasePlayer sender, int objectId, int textureIndex = 0)
        {
            if (textureIndex < 0 || textureIndex > 15)
            {
                sender.SendClientMessage("Index textury powinien zawierać się w przedziale od 0 do 15.");
                return;
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