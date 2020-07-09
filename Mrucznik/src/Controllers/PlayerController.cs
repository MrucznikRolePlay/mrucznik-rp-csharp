using SampSharp.GameMode.Controllers;

namespace Mrucznik.Controllers
{
    public class PlayerController : BasePlayerController
    {
        public virtual void RegisterTypes()
        {
            Player.Register<Player>();
        }
    }
}