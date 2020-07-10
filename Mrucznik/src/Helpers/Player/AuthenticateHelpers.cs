using Mrucznik;
using Mruv;

namespace mrucznik
{
    public static class AuthenticateHelpers
    {
        public static void ShowAuthFlow(Player _player)
        {
            var check = MruV.Accounts.IsAccountExists(new IsAccountExistsRequest() { Login = _player.Name });
            _player.Name = $"Niezalogowany_{_player.Id}";
            if (check.Exists)
            {
                var loginFlow = new LoginFlow(_player);
                loginFlow.Start();
            }
            else
            {
                var registrationFlow = new RegistrationFlow(_player);
                registrationFlow.Start();
            }
        }
    }
}
