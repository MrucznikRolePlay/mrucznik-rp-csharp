using System;
using System.Globalization;
using System.Linq;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.Streamer.Events;
using SampSharp.Streamer.World;

namespace Mrucznik.Objects
{
    public class ObjectEditor
    {        
        public ObjectEditorState ObjectEditorState;
        public bool EditMode { set; get; }

        private readonly Player _player;

        public ObjectEditor(Player player)
        {
            _player = player;
        }

        public void SelectObject()
        {
            
        }
        
        public void AddMultiSelect()
        {
            
        }
        
        #region Events

        public void OnSelected(MruDynamicObject o, PlayerSelectEventArgs e)
        {
            switch (((Player)e.Player).ObjectEditor.ObjectEditorState)
            {
                case ObjectEditorState.Edit:
                    e.Player.SendClientMessage($"Wybrałeś obiekt: {this}");
                    o.Edit(e.Player);
                    break;
                case ObjectEditorState.Delete:
                    e.Player.SendClientMessage($"Usunąłeś obiekt {this}");
                    o.ApiDelete();
                    break;
            }
        }

        public void OnEdited(MruDynamicObject o, PlayerEditEventArgs e)
        {
            if (EditMode)
            {
                if (e.Response == EditObjectResponse.Update)
                {
                    e.Player.SendClientMessage($"Edytujesz obiekt: {this}");
                    o.DynamicTextLabel.Position = e.Position;
                }
                else if (e.Response == EditObjectResponse.Final)
                {
                    e.Player.SendClientMessage($"Edytowałeś obiekt: {this}");
                    o.DynamicTextLabel.Position = e.Position;
                    o.Position = e.Position;
                    EditMode = false;
                    o.ApiSave();
                }
                else if (e.Response == EditObjectResponse.Cancel)
                {
                    e.Player.SendClientMessage($"Anulowałeś edycję obiektu: {this}");
                    o.DynamicTextLabel.Position = o.Position;
                    o.Position = o.Position;
                    EditMode = false;
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