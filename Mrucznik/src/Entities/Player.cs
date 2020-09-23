using System;
using System.Threading.Tasks;
using Mrucznik.Helpers;
using Mrucznik.Objects;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using Mrucznik.Systems.AntiCheat;
using Mrucznik.Systems.BeforeGame;
using Mruv;
using Mruv.Economy;
using SampSharp.GameMode;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        public Character PlayerCharacter;
        public bool LoggedIn;
        public readonly ObjectEditor ObjectEditor;

        private RealTime _realTime;
        private AntiSpawn _antiSpawn;
        private AntiWeapon _antiWeapon;

        public Player()
        {
            // Init player components
            _realTime = new RealTime(this);
            //_antiSpawn = new AntiSpawn(this);
            _antiWeapon = new AntiWeapon(this);
            ObjectEditor = new ObjectEditor(this);
        }

        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);
            //SetupClientOnConnect(this);

            var response = MruV.Punishments.IsPlayerBanned(new IsPlayerBannedRequest
            {
                Ip = this.IP,
                Player = 0
            });

            if (response.Banned)
            {
                SendClientMessage("Masz bana ziomeg.");
                Kick();
            }

            SetupClientOnConnect();
        }

        public void InstaKick()
        {
            base.Kick();
        }

        public override void Kick()
        {
            var timer = new Timer(10, false);
            timer.Tick += (sender, args) =>
            {
                if (IsConnected) base.Kick();
            };
        }

        public override void SendPlayerMessageToAll(string message)
        {
            if (LoggedIn == true) return;
            base.SendPlayerMessageToAll(message);
        }


        private void SetupClientOnConnect()
        {
            this.ClearChat();
            SendClientMessage(Color.White, "SERWER: Witaj {0} na serwerze Mrucznik RolePlay!", Name);
            if (PlayerHelpers.IsNameFormatCorrect(Name) == false)
            {
                SendClientMessage(Color.Red, "SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
                Kick();
            }
            ToggleClock(true);
            var sounds = new[] { 1187, 171, 176, 1076, 1187, 157, 162, 169, 178, 180, 181, 147, 140 };
            PlaySound(sounds[new Random().Next(sounds.Length)]);
            Task.Delay(1000).ContinueWith(t =>
            {
                if (!IsConnected) return;

                Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
                Angle = 326.0f;
                CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
                SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
            });
            ShowAuthFlow();
        }

        private void ShowAuthFlow()
        {
            var check = MruV.Accounts.IsAccountExist(new IsAccountExistRequest() { Login = Name });
            Name = $"Niezalogowany_{Id}";
            if (check.Exists)
            {
                var loginFlow = new LoginFlow(this);
                loginFlow.Show();
            }
            else
            {
                var registrationFlow = new RegistrationFlow(this);
                registrationFlow.Start();
            }
        }
    }
}