using SampSharp.GameMode;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SampSharp.GameMode.SAMP;
namespace Mrucznik.Helpers
{
    public static class PlayerHelpers
    {
        public static bool IsNameFormatCorrect(string playerName)
        {
            if (!Regex.IsMatch(playerName, "^[A-Z][a-z]+(_[A-Z][a-z]+([A-HJ-Z][a-z]+)?){1,2}$")) return false;
            return true;
        }

        public static void ClearAllChat()
        {
            for (var i = 0; i < 50; i++)
                Player.SendClientMessageToAll("");
            Player.SendClientMessageToAll(Color.Red, "Czat został wyczyszczony przez administratora.");
        }
    }
}
