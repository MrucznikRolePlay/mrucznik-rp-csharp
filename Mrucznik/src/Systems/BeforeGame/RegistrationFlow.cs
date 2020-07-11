using Grpc.Core;
using Mruv;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;

namespace Mrucznik.Systems.BeforeGame
{
    public class RegistrationFlow
    {
        private readonly Player _player;
        private readonly InputDialog _registerDialog;

        public RegistrationFlow(Player player)
        {
            _player = player;
            _player.SendClientMessage(Color.Yellow, "Aby rozpocząc rozgrywkę musisz się zarejestrować.");
            _registerDialog = new InputDialog("Rejestracja konta", "Witaj. Aby zacząć grę na serwerze musisz się zarejestrować.\nAby to zrobić wpisz w okienko poniżej hasło które chcesz używać w swoim koncie.\nZapamiętaj je gdyż będziesz musiał go używać za każdym razem kiedy wejdziesz na serwer", true, "Zarejestruj się", "Wyjdź");
            _registerDialog.Response += RegisterDialogOnResponse;
        }

        private void RegisterDialogOnResponse(object? sender, DialogResponseEventArgs e)
        {
            _player.Name = $"Niezarejestrowany_{_player.Id}";
            var registerAccountRequest = new RegisterAccountRequest();
            registerAccountRequest.Account = new Account()
            {
                Email = "mrucznix@gmail.com",
                Login = _player.Nick,
                Nick = _player.Nick
            };
            registerAccountRequest.Password = e.InputText;

            try
            {
                var response = MruV.Accounts.RegisterAccount(registerAccountRequest);
                if (response.Success)
                {
                    
                    new LoginFlow(_player).Start();
                    Tutorial tutorial = new Tutorial(_player);
                    tutorial.RegisterMessage();
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