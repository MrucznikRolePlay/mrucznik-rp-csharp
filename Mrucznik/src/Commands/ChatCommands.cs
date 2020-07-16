using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;

namespace Mrucznik.Commands
{
    public class ChatCommands
    {
        [Command("local", Shortcut = "l")]
        private static void LocalChatCommand(BasePlayer sender, string text)
        {
            sender.SendClientMessage("Local chat");
        }
    }
}