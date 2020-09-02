using System;
using SampSharp.GameMode.Controllers;

namespace Mrucznik.Controllers
{
    public class PlayerController : BasePlayerController, IEventListener
    {
        public override void RegisterTypes()
        {
            Player.Register<Player>();
        }
    }
}