using SampSharp.GameMode.Controllers;

namespace Mrucznik.Controllers
{
    class LoginControllers : IController, ITypeProvider
    {
        public virtual void RegisterTypes()
        {
            Logowanie.Register<Logowanie>();
        }
    }
}
