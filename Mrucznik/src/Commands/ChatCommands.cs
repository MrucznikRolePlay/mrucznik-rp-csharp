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
        
        [Command("okal")]
        private static void LocalChatCommand2(BasePlayer sender, string text)
        {
            sender.SendClientMessage("Local chat");
        }
        
        [Command("pokal")]
        private static void LocalChatCommand3(BasePlayer sender, string text)
        {
            sender.SendClientMessage("Local chat");
        }
        
        [Command("dm2")]
        private static void LocalChatCommand4(BasePlayer sender, string text)
        {
            sender.SendClientMessage("Local chat");
        }
    }
}