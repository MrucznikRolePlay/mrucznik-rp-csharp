using SampSharp.GameMode.Controllers;

namespace Mrucznik.Controllers
{
    public class PlayerController : BasePlayerController
    {
        public override void RegisterTypes()
        {
            Player.Register<Player>();
        }
    }
}