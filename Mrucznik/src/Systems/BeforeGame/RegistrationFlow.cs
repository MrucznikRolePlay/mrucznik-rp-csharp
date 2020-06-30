using Grpc.Core;
using Mruv;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class RegistrationFlow
    {
        private readonly BasePlayer _player;
        private readonly InputDialog _registerDialog;

        public RegistrationFlow(BasePlayer player)
        {
            _player = player;
            _registerDialog = new InputDialog("Rejestracja konta", "Witaj. Aby zacząć grę na serwerze musisz się zarejestrować.\nAby to zrobić wpisz w okienko poniżej hasło które chcesz używać w swoim koncie.\nZapamiętaj je gdyż będziesz musiał go używać za każdym razem kiedy wejdziesz na serwer", true, "Zarejestruj się", "Wyjdź");
            _registerDialog.Response += RegisterDialogOnResponse;
        }

        private void RegisterDialogOnResponse(object? sender, DialogResponseEventArgs e)
        {
            var registerAccountRequest = new RegisterAccountRequest();
            registerAccountRequest.Account = new Account()
            {
                Email = "mrucznix@gmail.com",
                Login = _player.Name,
                Nick = _player.Name
            };
            registerAccountRequest.Password = e.InputText;

            try
            {
                var response = MruV.Accounts.RegisterAccount(registerAccountRequest);
                if (response.Success)
                {
                    _player.SendClientMessage("Zarejestrowano!");
                    new LoginFlow(_player).Start();
                }
                else
                {
                    _player.SendClientMessage("Nie udało się zarejestrować konta.");
                    _registerDialog.Show(_player);
                }
            }
            catch(RpcException err)
            {
                _player.SendClientMessage($"Nie udało się zarejestrować, błąd: {err.Status.Detail}");
            }
        }

        public void Start()
        {
            _registerDialog.Show(_player);
        }
    }
}