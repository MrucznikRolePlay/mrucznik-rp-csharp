using System;
using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using SampSharp.Streamer.Events;
using SampSharp.Streamer.World;

namespace Mrucznik
{
    public class MruDynamicObject : DynamicObject
    {
        public int ApiId;
        public DynamicTextLabel DynamicTextLabel;

        private bool _editingMode = false;
        private Vector3 _lastPosition;
        
        public MruDynamicObject(int apiID, int modelid, Vector3 position, Vector3 rotation = new Vector3(), int worldid = -1, int interiorid = -1, BasePlayer player = null, float streamdistance = 200, float drawdistance = 0, DynamicArea area = null, int priority = 0) : base(modelid, position, rotation, worldid, interiorid, player, streamdistance, drawdistance, area, priority)
        {
            ApiId = apiID;
            Object3DText(modelid, position);
        }

        public MruDynamicObject(int apiID, int modelid, Vector3 position, Vector3 rotation, float streamdistance, int[] worlds = null, int[] interiors = null, BasePlayer[] players = null, float drawdistance = 0, DynamicArea[] areas = null, int priority = 0) : base(modelid, position, rotation, streamdistance, worlds, interiors, players, drawdistance, areas, priority)
        {
            ApiId = apiID;
            Object3DText(modelid, position);
        }

        private void Object3DText(int modelid, Vector3 position)
        {
            DynamicTextLabel = new DynamicTextLabel($"{GetModelName(modelid)}\nID: {Id} | Model: {modelid}", Color.Chocolate, position, 25.0f);
        }

        public override void OnSelected(PlayerSelectEventArgs e)
        {
            base.OnSelected(e);
            
            e.Player.SendClientMessage($"Wybrałeś obiekt: {GetModelName(ModelId)}");
            Edit(e.Player);
        }

        public override void OnEdited(PlayerEditEventArgs e)
        {
            base.OnEdited(e);

            if (_editingMode)
            {
                if (e.Response == EditObjectResponse.Update)
                {
                    e.Player.SendClientMessage($"Edytujesz obiekt: {GetModelName(ModelId)}");
                    DynamicTextLabel.Position = e.Position;
                }
                else if (e.Response == EditObjectResponse.Final)
                {
                    e.Player.SendClientMessage($"Edytowałeś obiekt: {GetModelName(ModelId)}");
                    DynamicTextLabel.Position = e.Position;
                    _editingMode = false;
                }
                else if (e.Response == EditObjectResponse.Cancel)
                {
                    e.Player.SendClientMessage($"Anulowałeś edycję obiektu: {GetModelName(ModelId)}");
                    Position = _lastPosition;
                    DynamicTextLabel.Position = _lastPosition;
                    _editingMode = false;
                }
            }
            else
            {
                e.Player.SendClientMessage($"Wchodzisz w tryb edycji obiektu: {GetModelName(ModelId)}");
                _editingMode = true;
                _lastPosition = Position;
                OnEdited(e);
            }
        }

        /// <summary>
        /// Get model name or return model id as string
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetModelName(int model)
        {
            if (Objects.Objects.ObjectModels.ContainsKey(model))
            {
                return Objects.Objects.ObjectModels[model].Name;
            }
            return model.ToString();
        }
    }
}