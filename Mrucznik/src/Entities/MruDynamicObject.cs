using SampSharp.GameMode;
using SampSharp.GameMode.World;
using SampSharp.Streamer.World;

namespace Mrucznik
{
    public class MruDynamicObject : DynamicObject
    {
        public uint APIID;
        
        public MruDynamicObject(uint apiId, int modelid, Vector3 position, Vector3 rotation = new Vector3(), int worldid = -1, int interiorid = -1, BasePlayer player = null, float streamdistance = 200, float drawdistance = 0, DynamicArea area = null, int priority = 0) : base(modelid, position, rotation, worldid, interiorid, player, streamdistance, drawdistance, area, priority)
        {
            APIID = apiId;
        }

        public MruDynamicObject(uint apiId, int modelid, Vector3 position, Vector3 rotation, float streamdistance, int[] worlds = null, int[] interiors = null, BasePlayer[] players = null, float drawdistance = 0, DynamicArea[] areas = null, int priority = 0) : base(modelid, position, rotation, streamdistance, worlds, interiors, players, drawdistance, areas, priority)
        {
            APIID = apiId;
        }
    }
}