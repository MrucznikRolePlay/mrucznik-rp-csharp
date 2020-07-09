using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grpc.Core;
using Mrucznik.Controllers;
using Mruv;
using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using Timer = System.Timers.Timer;

namespace Mrucznik
{
	public class Chat
	{
		public static void ClearPlayerChat(Player player)
        {
			for (var i = 0; i < 15; i++)
				player.SendClientMessage(" ");
        }
	}
}
