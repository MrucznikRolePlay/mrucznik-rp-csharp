using System;
using Mruv.Objects;
using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using SampSharp.Streamer.Events;
using SampSharp.Streamer.World;
using Object = Mruv.Objects.Object;

namespace Mrucznik
{
    public class MruDynamicObject : DynamicObject
    {
        public uint ApiId;
        public uint EstateId;
        public DynamicTextLabel DynamicTextLabel;
        
        public MruDynamicObject(uint apiID, int modelid, Vector3 position, Vector3 rotation = new Vector3(), int worldid = -1, int interiorid = -1, BasePlayer player = null, float streamdistance = 200, float drawdistance = 0, DynamicArea area = null, int priority = 0, uint estateId = 0) : base(modelid, position, rotation, worldid, interiorid, player, streamdistance, drawdistance, area, priority)
        {
            ApiId = apiID;
            Object3DText(modelid, position);
        }

        public MruDynamicObject(uint apiID, int modelid, Vector3 position, Vector3 rotation, float streamdistance, int[] worlds = null, int[] interiors = null, BasePlayer[] players = null, float drawdistance = 0, DynamicArea[] areas = null, int priority = 0, uint estateId = 0) : base(modelid, position, rotation, streamdistance, worlds, interiors, players, drawdistance, areas, priority)
        {
            ApiId = apiID;
            Object3DText(modelid, position);
        }

        private void Object3DText(int modelid, Vector3 position)
        {
            DynamicTextLabel = new DynamicTextLabel($"{GetModelName()}\nID: {Id} | Model: {modelid}", Color.Chocolate, position, 25.0f);
        }

        public override void OnSelected(PlayerSelectEventArgs e)
        {
            base.OnSelected(e);
            
            e.Player.SendClientMessage($"Wybrałeś obiekt: {GetModelName()}");
            Edit(e.Player);
        }

        public override void OnEdited(PlayerEditEventArgs e)
        {
            base.OnEdited(e);

            if (e.Response == EditObjectResponse.Update)
            {
                e.Player.SendClientMessage($"Edytujesz obiekt: {GetModelName()}");
                DynamicTextLabel.Position = e.Position;
            }
            else if (e.Response == EditObjectResponse.Final)
            {
                e.Player.SendClientMessage($"Edytowałeś obiekt: {GetModelName()}");
                DynamicTextLabel.Position = e.Position;
                Position = e.Position;
                
                Save();
            }
            else if (e.Response == EditObjectResponse.Cancel)
            {
                e.Player.SendClientMessage($"Anulowałeś edycję obiektu: {GetModelName()}");
                DynamicTextLabel.Position = Position;
                Position = Position;
            }
        }

        /// <summary>
        /// Get model name or return model id as string
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetModelName()
        {
            if (Objects.Objects.ObjectModels.ContainsKey(ModelId))
            {
                return Objects.Objects.ObjectModels[ModelId].Name;
            }
            return ModelId.ToString();
        }

        private void Save()
        {
            MruV.Objects.UpdateObjectAsync(new UpdateObjectRequest()
            {
                Id = ApiId,
                Object = new Object()
                {
                    Model = ModelId,
                    X = Position.X,
                    Y = Position.Y,
                    Z = Position.Z,
                    Rx = Rotation.X,
                    Ry = Rotation.Y,
                    Rz = Rotation.Z,
                    WorldId = World,
                    InteriorId = Interior,
                    PlayerId = -1,
                    AreaId = -1,
                    StreamDistance = StreamDistance,
                    DrawDistance = DrawDistance,
                    Priority = Priority,
                    EstateId = EstateId
                },
            });
        }
    }
}