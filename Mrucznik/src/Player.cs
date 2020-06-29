using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using Timer = System.Timers.Timer;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        private Timer _realWorldTimeTimer;

        public Player()
        {
            _realWorldTimeTimer = new Timer(1000);
            _realWorldTimeTimer.Elapsed += (sender, args) =>
            {
                SetTime(DateTime.Now.Hour, DateTime.Now.Minute);
            };
            _realWorldTimeTimer.AutoReset = true;
        }

        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);
            
            _realWorldTimeTimer.Start();
            SendClientMessage(Color.White, "SERVER: Witaj {0}", Name);
            
            if (!Regex.IsMatch(Name, "^[A-Z][a-z]+(_[A-Z][a-z]+([A-HJ-Z][a-z]+)?){1,2}$"))
            {
                SendClientMessage(
                    "SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
                Kick();
                return;
            }

            // Set time to real world time
            ToggleClock(true);

            Color = Color.LightGray;
            VirtualWorld = Id;
            ToggleControllable(false);

            var sounds = new [] {1187, 171, 176, 1076, 1187, 157, 162, 169, 178, 180, 181, 147, 140};
            PlaySound(sounds[new Random().Next(sounds.Length)]);
            
            Task.Delay(1000).ContinueWith(t =>
            {
                if (!IsConnected) return;
                
                Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
                Angle = 326.0f;
                CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
                SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
                ApplyAnimation("ON_LOOKERS", "wave_loop", 3.5f, true, false, false, false, 0, false);
            });
        }

        public override void OnDisconnected(DisconnectEventArgs e)
        {
            base.OnDisconnected(e);
            
            _realWorldTimeTimer.Stop();
        }

        public override void OnRequestClass(RequestClassEventArgs e)
        {
            base.OnRequestClass(e);

            ApplyAnimation("ON_LOOKERS", "wave_loop", 3.5f, true, false, false, false, 0, false);
        }
    }
}