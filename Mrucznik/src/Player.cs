using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using mrucznik;
using Mruv;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        private RealTime _realTime;
        public AntiSpawn _antiSpawn;
        private static Timer _KickTimer;
        //zmienne
        public bool LoggedIn;
        public Player()
        {
            _realTime = new RealTime(this);
            _antiSpawn = new AntiSpawn(this);
            
        }
        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);

            SendClientMessage(Color.White, "SERVER: Witaj {0}", Name);
            SendClientMessage(Color.Yellow, "Zweryfikuj swoje konto aby kontynuować.");
            if (!Regex.IsMatch(Name, "^[A-Z][a-z]+(_[A-Z][a-z]+([A-HJ-Z][a-z]+)?){1,2}$"))
            {
                SendClientMessage(
                    "SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
                Kick();
                return;
            }

            // Set time to real world time
            ToggleClock(true);


            var sounds = new[] {1187, 171, 176, 1076, 1187, 157, 162, 169, 178, 180, 181, 147, 140};
            PlaySound(sounds[new Random().Next(sounds.Length)]);

            Task.Delay(1000).ContinueWith(t =>
            {
                if (!IsConnected) return;

                Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
                Angle = 326.0f;
                CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
                SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
            });

            // Login/Registration
            var check = MruV.Accounts.IsAccountExists(new IsAccountExistsRequest() {Login = Name});
            if (check.Exists)
            {
                var loginFlow = new LoginFlow(this);
                loginFlow.Start();
            }
            else
            {
                var registrationFlow = new RegistrationFlow(this);
                registrationFlow.Start();
            }
        }

        public override void OnDisconnected(DisconnectEventArgs e)
        {
            base.OnDisconnected(e);
        }

        public override void Kick()
        {
            _KickTimer = new Timer(100, false);
            _KickTimer.Tick += _KickTimer_Executed;
        }

        private void _KickTimer_Executed(object sender, EventArgs e)
        {
            base.Kick();
        }
    }
}