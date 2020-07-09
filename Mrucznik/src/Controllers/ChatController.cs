using SampSharp.GameMode.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mrucznik.Controllers
{
    class ChatController : IController, ITypeProvider
    {
        public virtual void RegisterTypes()
        {
            Chat.Register<Chat>();
        }
    }
}
