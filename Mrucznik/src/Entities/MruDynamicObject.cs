using System;
using Mrucznik.Objects;
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

        public MruDynamicObject(uint apiId, int modelid, Vector3 position, Vector3 rotation = new Vector3(), int worldid = -1, int interiorid = -1, BasePlayer player = null, float streamdistance = 200, float drawdistance = 0, DynamicArea area = null, int priority = 0, uint estateId = 0) : base(modelid, position, rotation, worldid, interiorid, player, streamdistance, drawdistance, area, priority)
        {
            ApiId = apiId;
            Object3DText(modelid, position);
        }

        public MruDynamicObject(uint apiId, int modelid, Vector3 position, Vector3 rotation, float streamdistance, int[] worlds = null, int[] interiors = null, BasePlayer[] players = null, float drawdistance = 0, DynamicArea[] areas = null, int priority = 0, uint estateId = 0) : base(modelid, position, rotation, streamdistance, worlds, interiors, players, drawdistance, areas, priority)
        {
            ApiId = apiId;
            Object3DText(modelid, position);
        }

        private void Object3DText(int modelid, Vector3 position)
        {
            DynamicTextLabel = new DynamicTextLabel($"{this}\nAPI ID: {ApiId} | ID: {Id} | Model: {modelid}", Color.Chocolate, position, 25.0f);
        }

        public override void OnSelected(PlayerSelectEventArgs e)
        {
            base.OnSelected(e);

            ((Player)e.Player).ObjectEditor.OnSelected(this, e);
        }
        
        public override void OnEdited(PlayerEditEventArgs e)
        {
            base.OnEdited(e);

            ((Player)e.Player).ObjectEditor.OnEdited(this, e);
        }

        public override void Edit(BasePlayer player)
        {
            base.Edit(player);
            ((Player)Player).ObjectEditor.EditMode = true;
        }

        public void ApiSave()
        {
            Console.WriteLine($"Saving object {ApiId}");
            MruV.Objects.UpdateObjectAsync(new UpdateObjectRequest()
            {
                Id = ApiId,
                Object = this
            });
        }

        public void ApiDelete()
        {
            Console.WriteLine($"Deleting object {ApiId}");
            MruV.Objects.DeleteObjectAsync(new DeleteObjectRequest()
            {
                Id = ApiId,
            });
            Dispose();
        }


        public static implicit operator Object(MruDynamicObject o) => new Object()
        {
            Model = o.ModelId,
            X = o.Position.X,
            Y = o.Position.Y,
            Z = o.Position.Z,
            Rx = o.Rotation.X,
            Ry = o.Rotation.Y,
            Rz = o.Rotation.Z,
            WorldId = o.World,
            InteriorId = o.Interior,
            PlayerId = -1,
            AreaId = -1,
            StreamDistance = o.StreamDistance,
            DrawDistance = o.DrawDistance,
            Priority = o.Priority,
            EstateId = o.EstateId
        };

        public override string ToString()
        {
            return this.GetName();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            DynamicTextLabel.Dispose();
        }
    }
}