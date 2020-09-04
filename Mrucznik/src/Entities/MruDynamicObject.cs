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
        public uint ApiId { get; private set; }
        public uint EstateId;
        
        private DynamicTextLabel _dynamicTextLabel;

        public MruDynamicObject(DynamicObject o) : base(o.ModelId, o.Position, o.Rotation, o.World, o.Interior,
            o.Player, o.StreamDistance, o.DrawDistance, o.Area, o.Priority)
        {
            ApiId = ApiCreate();
            Object3DText(o.ModelId, o.Position);
        }

        public MruDynamicObject(int modelid, Vector3 position, Vector3 rotation = new Vector3(), int worldid = -1,
            int interiorid = -1, BasePlayer player = null, float streamdistance = 200, float drawdistance = 0,
            DynamicArea area = null, int priority = 0, uint estateId = 0) : base(modelid, position, rotation, worldid,
            interiorid, player, streamdistance, drawdistance, area, priority)
        {
            ApiId = ApiCreate();
            Object3DText(modelid, position);
        }

        public MruDynamicObject(uint apiId, int modelid, Vector3 position, Vector3 rotation = new Vector3(),
            int worldid = -1, int interiorid = -1, BasePlayer player = null, float streamdistance = 200,
            float drawdistance = 0, DynamicArea area = null, int priority = 0, uint estateId = 0) : base(modelid,
            position, rotation, worldid, interiorid, player, streamdistance, drawdistance, area, priority)
        {
            ApiId = apiId;
            Object3DText(modelid, position);
        }

        public MruDynamicObject(uint apiId, int modelid, Vector3 position, Vector3 rotation, float streamdistance,
            int[] worlds = null, int[] interiors = null, BasePlayer[] players = null, float drawdistance = 0,
            DynamicArea[] areas = null, int priority = 0, uint estateId = 0) : base(modelid, position, rotation,
            streamdistance, worlds, interiors, players, drawdistance, areas, priority)
        {
            ApiId = apiId;
            Object3DText(modelid, position);
        }

        private void Object3DText(int modelid, Vector3 position)
        {
            _dynamicTextLabel = new DynamicTextLabel($"{this}\nAPI ID: {ApiId} | ID: {Id} | Model: {modelid}",
                Color.Chocolate, position, 25.0f);
        }

        public override void OnSelected(PlayerSelectEventArgs e)
        {
            base.OnSelected(e);

            ((Player) e.Player).ObjectEditor.OnSelected(this, e);
        }

        public override void OnEdited(PlayerEditEventArgs e)
        {
            base.OnEdited(e);

            if (e.Response == EditObjectResponse.Cancel)
            {
                _dynamicTextLabel.Position = Position;
            }
            else
            {
                _dynamicTextLabel.Position = e.Position;
            }
            ((Player) e.Player).ObjectEditor.OnEdited(this, e);
        }

        internal void ApiSave()
        {
            Console.WriteLine($"Saving object {ApiId}");
            MruV.Objects.UpdateObjectAsync(new UpdateObjectRequest()
            {
                Id = ApiId,
                Object = this
            });
        }

        internal void ApiDelete()
        {
            Console.WriteLine($"Deleting object {ApiId}");
            MruV.Objects.DeleteObjectAsync(new DeleteObjectRequest()
            {
                Id = ApiId,
            });
            Dispose();
        }

        internal uint ApiCreate()
        {
            var response = MruV.Objects.CreateObject(new CreateObjectRequest()
            {
                Object = this
            });
            ApiId = response.Id;
            Objects.Objects.ObjectsIDs[(int) ApiId] = this;
            Console.WriteLine($"Created object with id {ApiId}");
            return ApiId;
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
            _dynamicTextLabel?.Dispose();
            
            base.Dispose(disposing);
        }

        protected bool Equals(MruDynamicObject other)
        {
            return ApiId == other.ApiId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MruDynamicObject) obj);
        }

        public override int GetHashCode()
        {
            return (int) ApiId;
        }

        public override Vector3 Position
        {
            get => base.Position;
            set
            {
                _dynamicTextLabel.Position = value;
                base.Position = value;
            }
        }

        public void Mark()
        {
            _dynamicTextLabel.Color = Color.GreenYellow;
        }

        public void UnMark()
        {
            _dynamicTextLabel.Color = Color.Chocolate;
        }
    }
}