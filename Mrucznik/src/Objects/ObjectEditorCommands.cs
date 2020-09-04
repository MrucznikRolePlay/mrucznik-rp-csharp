using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.SAMP.Commands.Parameters;
using SampSharp.GameMode.World;
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
                    foreach (var objectId in objectIds)
                    {
                        sender.ObjectEditor.SelectObject((MruDynamicObject)MruDynamicObject.Find(objectId));
                    }
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
                    // TODO: create + 1 based on camera view angle
                    sender.SendClientMessage($"Stworzyłeś obiekt o modelu {modelId}.");
                    sender.ObjectEditor.CreateObjectMode(new MruDynamicObject(modelId, sender.Position));
                }
                else
                {
                    ModelsDialogCreate(sender);
                }
            }

            [Command("stats", "s")]
            private static void ObjectStats(Player sender, int modelId = -1)
            {
                
            }

            [CommandGroup("texture", "t")]
            class ObjectTexturesCommands
            {
                [Command("edit", "e")]
                private static void EditTexture(BasePlayer sender, int objectId = -1)
                {
                    
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

            [CommandGroup("gate", "gates")]
            class ObjectGateCommands
            {
                [Command("create")]
                private static void CreateGate(Player sender, int modelId)
                {
                    sender.SendClientMessage("Tworzysz bramę.");
                    sender.ObjectEditor.CreateGateMode(new MruDynamicObject(modelId, sender.Position));
                }
            }

            private static void ModelsDialogCreate(Player player)
            {
                var action = new EventHandler<DialogResponseEventArgs>((o, args) =>
                {
                    if (args.DialogButton == DialogButton.Left)
                    {
                        player.SendClientMessage($"Stworzyłeś obiekt o modelu {args.InputText}.");
                        player.ObjectEditor.CreateObjectMode(new MruDynamicObject(Int32.Parse(args.InputText), player.Position));
                    }
                    else
                    {
                        ModelsDialogCreate(player);
                    }
                });
                
                var categoriesDialog = new ListDialog("Kategorie modeli", "Wyjdź", "Wybierz");
                categoriesDialog.AddItems(Objects.Objects.ObjectModelsCategory.Keys);
                categoriesDialog.Response += (sender, args) =>
                {
                    var tablistDialog = new TablistDialog($"Modele - Kategoria {args.InputText}",
                        new[] {"Model", "Nazwa", "Rozmiar", "Tagi"},
                        "Stwórz", "Wróć");
                    foreach (var objectModel in Objects.Objects.ObjectModelsCategory[args.InputText])
                    {
                        var o = objectModel.Value;
                        tablistDialog.Add(o.Model.ToString(), o.Name,
                            o.Size.ToString(CultureInfo.CurrentCulture), String.Join(", ", o.Tags));
                    }

                    tablistDialog.Response += action;
                    tablistDialog.Show(player);
                };
                categoriesDialog.Show(player);
            }
        }
    }
}