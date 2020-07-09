using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using System;

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
