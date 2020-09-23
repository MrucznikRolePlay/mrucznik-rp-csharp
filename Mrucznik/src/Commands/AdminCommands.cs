using SampSharp.GameMode.SAMP.Commands;

namespace Mrucznik.Commands
{
    public class AdminCommands
    {
        [Command("ban")]
        private static void Ban(Player sender, Player giveplayer, string reason)
        {
            giveplayer.Ban(sender, reason);
        }

        [Command("block")]
        private static void Block(Player sender, Player giveplayer, string reason)
        {
            giveplayer.Block(sender, reason);
        }

        [Command("kick")]
        private static void Kick(Player sender, Player giveplayer, string reason)
        {
            giveplayer.Kick(reason);
        }

        [Command("aj", "adminjail")]
        private static void AdminJail(Player sender, Player giveplayer, string reason)
        {
            
        }
    }
}