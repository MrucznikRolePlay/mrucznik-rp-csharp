using System;
using SampSharp.GameMode;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using SampSharp.Streamer.Events;
using SampSharp.Streamer.World;

namespace Mrucznik
{
    public class MruDynamicObject : DynamicObject
    {
        public DynamicTextLabel DynamicTextLabel;
        
        public MruDynamicObject(int modelid, Vector3 position, Vector3 rotation = new Vector3(), int worldid = -1, int interiorid = -1, BasePlayer player = null, float streamdistance = 200, float drawdistance = 0, DynamicArea area = null, int priority = 0) : base(modelid, position, rotation, worldid, interiorid, player, streamdistance, drawdistance, area, priority)
        {
            Object3DText(modelid, position);
        }

        public MruDynamicObject(int modelid, Vector3 position, Vector3 rotation, float streamdistance, int[] worlds = null, int[] interiors = null, BasePlayer[] players = null, float drawdistance = 0, DynamicArea[] areas = null, int priority = 0) : base(modelid, position, rotation, streamdistance, worlds, interiors, players, drawdistance, areas, priority)
        {
            Object3DText(modelid, position);
        }

        private void Object3DText(int modelid, Vector3 position)
        {
            if (Objects.Objects.ObjectModels.ContainsKey(modelid))
            {
                var model = Objects.Objects.ObjectModels[modelid];
                DynamicTextLabel = new DynamicTextLabel($"{model.Name}\nID: {Id} | Model: {modelid}", Color.Chocolate, position, 25.0f);
            }
            else
            {
                DynamicTextLabel = new DynamicTextLabel($"ID: {Id} | Model: {modelid}", Color.Chocolate, position, 25.0f);
            }
        }

        public override void OnSelected(PlayerSelectEventArgs e)
        {
            base.OnSelected(e);
            
            
        }
    }
}