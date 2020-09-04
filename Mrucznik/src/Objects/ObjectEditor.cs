using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mruv.Gates;
using Mruv.Objects;
using Mruv.Spots;
using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;
using SampSharp.Streamer.Events;
using SampSharp.Streamer.World;

namespace Mrucznik.Objects
{
    public class ObjectEditor
    {
        private ObjectEditorState _objectEditorState;
        private HashSet<MruDynamicObject> _selectedObjects = new HashSet<MruDynamicObject>();
        private readonly Dictionary<MruDynamicObject, Vector3> _selectedObjectsPositions =
            new Dictionary<MruDynamicObject, Vector3>();

        private readonly Player _player;

        public ObjectEditor(Player player)
        {
            _player = player;
        }

        public void SelectObjectMode()
        {
            _objectEditorState = ObjectEditorState.Edit;
            GlobalObject.Select(_player);
        }

        public void CloneObjectMode()
        {
            _objectEditorState = ObjectEditorState.Clone;
            GlobalObject.Select(_player);
        }

        public void MultiSelectMode()
        {
            _objectEditorState = ObjectEditorState.MultiSelect;
            GlobalObject.Select(_player);
        }

        public void DeleteObjectMode()
        {
            _objectEditorState = ObjectEditorState.Delete;
            GlobalObject.Select(_player);
        }

        public void CreateObjectMode(MruDynamicObject o)
        {
            _objectEditorState = ObjectEditorState.Create;
            o.Edit(_player);
        }

        public void EditObjectMode(MruDynamicObject o)
        {
            _objectEditorState = ObjectEditorState.Edit;
            o.Edit(_player);
        }

        public void CreateGateMode(MruDynamicObject o)
        {
            _objectEditorState = ObjectEditorState.CreateGate;
            o.Edit(_player);
        }

        public void CancelSelection()
        {
            foreach (var o in _selectedObjects)
            {
                o.UnMark();
            }

            _selectedObjects.Clear();
        }

        public void SelectObject(MruDynamicObject o)
        {
            _selectedObjects.Add(o);
            o.Mark();
        }

        public void UnSelectObject(MruDynamicObject o)
        {
            _selectedObjects.Remove(o);
            o.UnMark();
        }

        #region Events

        public void OnSelected(MruDynamicObject o, PlayerSelectEventArgs e)
        {
            if (_objectEditorState != ObjectEditorState.MultiSelect && _selectedObjects.Contains(o))
            {
                _selectedObjects.Remove(o);
            }

            switch (_objectEditorState)
            {
                case ObjectEditorState.Edit:
                    _player.SendClientMessage($"Wybrałeś obiekt: {o}");
                    foreach (var selectedObject in _selectedObjects)
                    {
                        _selectedObjectsPositions[selectedObject] = selectedObject.Position;
                    }
                    
                    o.Edit(_player);
                    o.Mark();
                    break;
                case ObjectEditorState.Delete:
                    _player.SendClientMessage($"Usunąłeś obiekt {o}");
                    foreach (var selectedObject in _selectedObjects)
                    {
                        selectedObject.ApiDelete();
                    }

                    _selectedObjects.Clear();
                    o.ApiDelete();
                    _objectEditorState = ObjectEditorState.Delete;
                    break;
                case ObjectEditorState.Clone:
                    _player.SendClientMessage($"Sklonowałeś obiekt: {o}");
                    var oldSelected = _selectedObjects;
                    _selectedObjects = new HashSet<MruDynamicObject>();
                    foreach (var selectedObject in oldSelected)
                    {
                        selectedObject.UnMark();
                        var c = new MruDynamicObject(selectedObject);
                        _selectedObjectsPositions[c] = c.Position;
                        SelectObject(c);
                    }

                    var clone = new MruDynamicObject(o);
                    _objectEditorState = ObjectEditorState.Edit;
                    clone.Edit(e.Player);
                    break;
                case ObjectEditorState.MultiSelect:
                    if (_selectedObjects.Contains(o))
                    {
                        _player.SendClientMessage($"Usunąłeś obiekt {o} z zaznaczeń");
                        UnSelectObject(o);
                    }
                    else
                    {
                        _player.SendClientMessage($"Dodałeś obiekt {o} do zaznaczeń");
                        SelectObject(o);
                    }

                    MultiSelectMode();
                    break;
            }
        }

        public void OnEdited(MruDynamicObject o, PlayerEditEventArgs e)
        {
            if (_objectEditorState == ObjectEditorState.Edit)
            {
                if (e.Response == EditObjectResponse.Update)
                {
                    foreach (var selectedObject in _selectedObjects)
                    {
                        selectedObject.Position = _selectedObjectsPositions[selectedObject] + (e.Position - o.Position);
                    }
                    return;
                }

                if (e.Response == EditObjectResponse.Final)
                {
                    e.Player.SendClientMessage($"Edytowałeś obiekt: {o}");
                    foreach (var selectedObject in _selectedObjects)
                    {
                        selectedObject.Position = _selectedObjectsPositions[selectedObject] + (e.Position - o.Position);
                        selectedObject.ApiSave();
                    }

                    o.Position = e.Position;
                    o.ApiSave();
                }
                else if (e.Response == EditObjectResponse.Cancel)
                {
                    e.Player.SendClientMessage($"Anulowałeś edycję obiektu: {o}");
                    foreach (var selectedObject in _selectedObjects)
                    {
                        selectedObject.Position = _selectedObjectsPositions[selectedObject];
                    }
                    o.UnMark();
                }

                _objectEditorState = ObjectEditorState.None;
                _selectedObjectsPositions.Clear();
                o.UnMark();
            }
            else if (_objectEditorState == ObjectEditorState.CreateGate)
            {
                switch (e.Response)
                {
                    case EditObjectResponse.Update:
                        break;
                    case EditObjectResponse.Final:
                        //2nd step - set opened position
                        o.Position = e.Position;
                        _player.SendClientMessage("Ustaw pozycję otwartej bramy.");

                        break;
                    case EditObjectResponse.Cancel:
                        //destroy object
                        e.Player.SendClientMessage("Anulowałeś tworzenie bramy.");
                        o.ApiDelete();
                        break;
                }
            }
            else if (_objectEditorState == ObjectEditorState.CreateGate2NdState)
            {
                switch (e.Response)
                {
                    case EditObjectResponse.Update:
                        break;
                    case EditObjectResponse.Final:
                        //3nd step - gate params & save gate to API
                        MruV.Gates.CreateGateAsync(new CreateGateRequest()
                        {
                            Name = "Gate",
                            Spot = new Spot()
                            {
                                Name = "Gate Spot",
                                Message = "",
                                Icon = 0,
                                Marker = 0,
                                Int = _player.Interior,
                                Vw = _player.VirtualWorld,
                                X = _player.Position.X,
                                Y = _player.Position.Y,
                                Z = _player.Position.Z,
                            },
                            GateObjects =
                            {
                                new MovableObject()
                                {
                                    Object = o,
                                    States =
                                    {
                                        new State()
                                        {
                                            Name = "Closed",
                                            X = o.Position.X,
                                            Y = o.Position.Y,
                                            Z = o.Position.Z,
                                            Rx = o.Rotation.X,
                                            Ry = o.Rotation.Y,
                                            Rz = o.Rotation.Z,
                                            TransitionSpeed = 2.5f
                                        },
                                        new State()
                                        {
                                            Name = "Opened",
                                            X = e.Position.X,
                                            Y = e.Position.Y,
                                            Z = e.Position.Z,
                                            Rx = e.Rotation.X,
                                            Ry = e.Rotation.Y,
                                            Rz = e.Rotation.Z,
                                            TransitionSpeed = 2.5f
                                        }
                                    }
                                }
                            }
                        });
                        o.Position = o.Position;
                        break;
                    case EditObjectResponse.Cancel:
                        e.Player.SendClientMessage("Anulowałeś tworzenie bramy.");
                        o.ApiDelete();
                        break;
                }
            }
        }

        #endregion

        #region Dialogs

        public (Dialog, bool) GetObjectListDialog(int page, string leftButtonText,
            EventHandler<DialogResponseEventArgs> action)
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

        #endregion
    }
}