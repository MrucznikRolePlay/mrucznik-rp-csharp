using Grpc.Core;
using Mruv;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class LoginFlow
    {
        private readonly BasePlayer _player;
        private readonly InputDialog _loginDialog;

        public LoginFlow(BasePlayer player)
        {
            _player = player;
            _loginDialog = new InputDialog(
                "Logowanie",
                $"Witaj {player.Name}. Twoje konto jest zarejestrowane\nZaloguj się wpisując w okienko poniżej hasło.\nJeżli nie znasz hasła do tego konta, wejdź pod innym nickiem.",
                true,
                "Zaloguj się", "Wyjdź"
            );
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
                    new CharacterSelectFlow(_player).Start();
                }
                else
                {
                    _player.SendClientMessage("Złe hasło!");
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