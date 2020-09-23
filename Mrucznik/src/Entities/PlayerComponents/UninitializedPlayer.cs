using System;
using System.Threading.Tasks;
using Mrucznik.Helpers;
using Mrucznik.PlayerComponents;
using Mrucznik.Systems.BeforeGame;
using Mruv;
using SampSharp.GameMode;
using SampSharp.GameMode.SAMP;

namespace Mrucznik.PlayerStates
{
    /// <summary>
    /// This class represents a player that isn't in a game world yet.
    /// This player didn't log on on an account and didn't pick a character.
    /// </summary>
    public class UninitializedPlayer : IPlayerComponent
    {
        public void RegisterComponent(Player player)
        {
            player.Connected += OnConnected;
        }

        public void UnregisterComponent(Player player)
        {
            player.Connected -= OnConnected;
        }

        private void OnConnected(object? sender, EventArgs e)
        {
            if (sender is Player player)
            {
                player.ClearChat();
                player.SendClientMessage(Color.White, "SERWER: Witaj {0} na serwerze Mrucznik RolePlay!", player.Name);
                if (PlayerHelpers.IsNameFormatCorrect(player.Name) == false)
                {
                    player.SendClientMessage(Color.Red, "SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
                    player.Kick();
                }
                player.ToggleClock(true);
                var sounds = new[] { 1187, 171, 176, 1076, 1187, 157, 162, 169, 178, 180, 181, 147, 140 };
                    player.PlaySound(sounds[new Random().Next(sounds.Length)]);
                Task.Delay(1000).ContinueWith(t =>
                {
                    if (!player.IsConnected) return;

                    player.Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
                    player.Angle = 326.0f;
                    player.CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
                    player.SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
                });
                ShowAuthFlow(player);
            }
        }
        
        private void ShowAuthFlow(Player player)
        {
            var check = MruV.Accounts.IsAccountExist(new IsAccountExistRequest() { Login = player.Name });
            player.Name = $"Niezalogowany_{player.Id}";
            if (check.Exists)
            {
                var loginFlow = new LoginFlow(player);
                loginFlow.Show();
            }
            else
            {
                var registrationFlow = new RegistrationFlow(player);
                registrationFlow.Start();
            }
        }
    }
}