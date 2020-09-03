using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Google.Type;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;
using SampSharp.Streamer.Events;
using SampSharp.Streamer.World;
using Color = SampSharp.GameMode.SAMP.Color;

namespace Mrucznik.Objects
{
    public class ObjectEditor
    {        
        private ObjectEditorState ObjectEditorState;
        private HashSet<MruDynamicObject> selectedObjects = new HashSet<MruDynamicObject>();

        private readonly Player _player;

        public ObjectEditor(Player player)
        {
            _player = player;
        }

        public void SelectObjectMode()
        {
            ObjectEditorState = ObjectEditorState.Edit;
            GlobalObject.Select(_player);
        }
        public void CloneObjectMode()
        {
            ObjectEditorState = ObjectEditorState.Clone;
            GlobalObject.Select(_player);
        }
        
        public void MultiSelectMode()
        {
            ObjectEditorState = ObjectEditorState.MultiSelect;
            GlobalObject.Select(_player);
        }

        public void DeleteObjectMode()
        {
            ObjectEditorState = ObjectEditorState.Delete;
            GlobalObject.Select(_player);
        }

        public void CreateObjectMode(MruDynamicObject o)
        {
            ObjectEditorState = ObjectEditorState.Create;
            o.Edit(_player);
        }

        public void EditObjectMode(MruDynamicObject o)
        {
            ObjectEditorState = ObjectEditorState.Edit;
            o.Edit(_player);
        }

        public void CancelSelection()
        {
            foreach (var o in selectedObjects)
            {
                UnSelectObject(o);
            }
        }

        public void SelectObject(MruDynamicObject o)
        {
            selectedObjects.Add(o);
            o.Mark();
        }

        public void UnSelectObject(MruDynamicObject o)
        {
            selectedObjects.Remove(o);
            o.UnMark();
        }
        
        #region Events
        public void OnSelected(MruDynamicObject o, PlayerSelectEventArgs e)
        {
            switch (ObjectEditorState)
            {
                case ObjectEditorState.Edit:
                    _player.SendClientMessage($"Wybrałeś obiekt: {this}");
                    o.Edit(_player);
                    o.Mark();
                    break;
                case ObjectEditorState.Delete:
                    _player.SendClientMessage($"Usunąłeś obiekt {this}");
                    foreach (var selectedObject in selectedObjects)
                    {
                        selectedObject.ApiDelete();
                    }
                    o.ApiDelete();
                    ObjectEditorState = ObjectEditorState.None;
                    break;
                case ObjectEditorState.Clone:
                    _player.SendClientMessage($"Sklonowałeś obiekt: {this}");
                    foreach (var selectedObject in selectedObjects)
                    {
                        new MruDynamicObject(selectedObject);
                    }
                    var clone = new MruDynamicObject(o);
                    clone.Edit(e.Player);
                    break;
                case ObjectEditorState.MultiSelect:
                    if (selectedObjects.Contains(o))
                    {
                        _player.SendClientMessage($"Usunąłęś obiekt {this} z zaznaczeń");
                        UnSelectObject(o);
                    }
                    else
                    {
                        _player.SendClientMessage($"Dodałeś obiekt {this} do zaznaczeń");
                        SelectObject(o);
                    }
                    MultiSelectMode();
                    break;
            }
        }

        public void OnEdited(MruDynamicObject o, PlayerEditEventArgs e)
        {
            if (ObjectEditorState == ObjectEditorState.Edit)
            {
                if (e.Response == EditObjectResponse.Update)
                {
                    e.Player.SendClientMessage($"Edytujesz obiekt: {this}");

                    foreach (var selectedObject in selectedObjects)
                    {
                        selectedObject.Position += o.Position - e.Position;
                    }
                }
                else if (e.Response == EditObjectResponse.Final)
                {
                    e.Player.SendClientMessage($"Edytowałeś obiekt: {this}");
                    ObjectEditorState = ObjectEditorState.None;
                    o.Position = e.Position;
                    foreach (var selectedObject in selectedObjects)
                    {
                        selectedObject.Position += o.Position - e.Position;
                        selectedObject.UnMark();
                        selectedObject.ApiSave();
                    }
                    o.UnMark();
                    o.ApiSave();
                }
                else if (e.Response == EditObjectResponse.Cancel)
                {
                    e.Player.SendClientMessage($"Anulowałeś edycję obiektu: {this}");
                    ObjectEditorState = ObjectEditorState.None;
                    foreach (var selectedObject in selectedObjects)
                    {
                        selectedObject.Position -= o.Position - e.Position;
                        selectedObject.UnMark();
                    }
                    o.Position = o.Position;
                    o.UnMark();
                }
            }
        }
        #endregion

        #region Dialogs
        public (Dialog, bool) GetObjectListDialog(int page, string leftButtonText, EventHandler<DialogResponseEventArgs> action)
        {
            if (page < 0)
                return (null, false);
            
            var objects = DynamicObject.All.Skip(50 * page).Take(50);
            var tablistDialog = new TablistDialog($"Obiekty - strona {page.ToString()}",
                new[] {"ID", "Model", "Nazwa", "X", "Y", "Z", "VW", "INT"},
                leftButtonText, "Następny");
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

            return (tablistDialog, ok);
        }
        
        public (Dialog, bool) GetModelListDialog(int page, string leftButtonText, EventHandler<DialogResponseEventArgs> action)
        {
            var tablistDialog = new TablistDialog($"Modele - strona {page.ToString()}",
                new[] {"Model", "Nazwa", "Kategoria", "Tagi"},
                leftButtonText, "Następny");
            bool ok = false;
            foreach (var objectModel in Objects.ObjectModels.Skip(50 * page).Take(50))
            {
                var o = objectModel.Value;
                ok = true;
                tablistDialog.Add(o.Model.ToString(), o.Name, 
                    o.Category, String.Join( ", ", o.Tags));
            }
            tablistDialog.Response += action;
            return (tablistDialog, ok);
        }
        #endregion
    }
}