using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grpc.Core;
using Mruv;
using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        public override void OnText(TextEventArgs e)
        {
            base.OnText(e);
            SendClientMessage("Wpisales {0}", e);
        }
    }
}