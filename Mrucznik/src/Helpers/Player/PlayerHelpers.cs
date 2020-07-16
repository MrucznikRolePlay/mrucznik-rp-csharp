using Mrucznik;
using SampSharp.GameMode;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SampSharp.GameMode.SAMP;
using static Mrucznik.Helpers.AuthenticateHelpers;
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
        public static void SetupClientOnConnect(Player _player)
        {
            _player.ClearChat();
            _player.SendClientMessage(Color.White, "SERWER: Witaj {0} na serwerze Mrucznik RolePlay!", _player.Name);
            if (IsNameFormatCorrect(_player.Name) == false)
            {
                _player.SendClientMessage(Color.Red, "SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
                _player.Kick();
            }
            _player.Nick = _player.Name;
            _player.ToggleClock(true);
            var sounds = new[] { 1187, 171, 176, 1076, 1187, 157, 162, 169, 178, 180, 181, 147, 140 };
            _player.PlaySound(sounds[new Random().Next(sounds.Length)]);
            Task.Delay(1000).ContinueWith(t =>
            {
                if (!_player.IsConnected) return;

                _player.Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
                _player.Angle = 326.0f;
                _player.CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
                _player.SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
            });
            ShowAuthFlow(_player);
        }
    }
}
