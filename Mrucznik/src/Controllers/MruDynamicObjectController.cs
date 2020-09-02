using System;
using SampSharp.Streamer.World;
using SampSharp.Streamer.Controllers;

namespace Mrucznik.Controllers
{
    public class MruDynamicObjectController : DynamicObjectController
    {
        public override void RegisterTypes()
        {
            DynamicObject.Register<MruDynamicObject>();
        }
    }
}