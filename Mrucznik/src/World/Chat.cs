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
	public class Chat : Player
	{
		public void ClearPlayerChat()
        {
			for (var i = 0; i < 15; i++)
				SendClientMessage(" ");
        }
		public void ClearAllPlayerChat()
        {
			for (var i = 0; i < 50; i++)
				SendClientMessageToAll(" ");
			SendClientMessageToAll(Color.Red, "Czat został wyczyszczony przez admina.");
        }
	}
}
