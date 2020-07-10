using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.World;
using System;
using System.Security.Cryptography.X509Certificates;

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