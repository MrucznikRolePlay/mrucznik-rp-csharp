using System;
using Grpc.Core;
using mrucznik;
using Mruv;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;
using SampSharp.GameMode.SAMP;

namespace Mrucznik
{
    public class LoginFlow
    {
        private readonly Player _player;
        private readonly InputDialog _loginDialog;
        private int _playerBadPassword = 0;

        public LoginFlow(Player player)
        {
            _player = player;
            _loginDialog = new InputDialog(
                "Logowanie",
                $"Witaj {player.Name}. Twoje konto jest zarejestrowane\nZaloguj się wpisując w okienko poniżej hasło.\nJeżli nie znasz hasła do tego konta, wejdź pod innym nickiem.",
                true,
                "Zaloguj się", "Wyjdź"
            );
            _player.ToggleSpectating(true);
            _loginDialog.Response += LoginDialogOnResponse;
        }

        private void LoginDialogOnResponse(object? sender, DialogResponseEventArgs e)
        {
            var logInRequest = new LogInRequest() {Login = _player.Name, Password = e.InputText};
            try
            {
                LogInResponse response = MruV.Accounts.LogIn(logInRequest);
                if (response.Success)
                {
                    _player.SendClientMessage("Zalogowano!");
                    CharacterSelectFlow charsf = new CharacterSelectFlow(_player);
                    charsf.ChoosedCharacter += _player._antiSpawn.OnPlayerChoosedCharacter;
                    charsf.Start();
                }
                else
                {
                    _playerBadPassword++;
                    
                    _player.SendClientMessage(Color.OrangeRed,"Złe hasło!");
                    if (_playerBadPassword == 3)
                    {
                        _player.SendClientMessage(Color.Red, "Zostałeś wyrzucony z serwera za potrójne wpisanie złego hasła.");
                        _player.Kick();
                    }
                    _loginDialog.Show(_player);
                }
            }
            catch (RpcException err)
            {
                _player.SendClientMessage($"Nie udało się zalogować, błąd: {err.Status.Detail}");
            }
        }
        
        public void Start()
        {
            _loginDialog.Show(_player);
        }
    }
}