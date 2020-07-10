
using SampSharp.GameMode;
using SampSharp.Streamer.World;

namespace Mrucznik.Objects
{
    public class Objects
    {
        public Objects()
        {
            //new SampSharp.Streamer.World.DynamicObject({model}, new Vector3({x}, {y}, {z}), new Vector3({rx}, {ry}, {rz}), {vw}, {int}, null, {dd})
            new DynamicObject(2614, new Vector3(1423.5000000, -886.4000244, 53.7000008),
                new Vector3(0.0000000, 0.0000000, 180.0000000), 0, 0, null, 250);
            
        }
    }
}