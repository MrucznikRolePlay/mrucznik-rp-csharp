using System;
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
            
            Task.Delay(1000).ContinueWith(t => 
            {
                Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
                Angle = 326.0f;
                CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
                SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
                PlaySound(1062, new Vector3(-2818.0f, 1100.0f, 0.0f));
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