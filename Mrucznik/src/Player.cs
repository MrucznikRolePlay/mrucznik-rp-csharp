using System;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using static Mrucznik.Helpers.PlayerHelpers;
using Mrucznik.Systems.Anticheat;
using Mrucznik.Systems.BeforeGame;
using SampSharp.GameMode.Definitions;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        private RealTime _realTime;
        public AntiSpawn _antiSpawn;
        public string Nick;
        //zmienne
        public bool LoggedIn;
        public bool InTutorial;
        public Player()
        {
            _realTime = new RealTime(this);
            _antiSpawn = new AntiSpawn(this);  
        }
        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);
            SetupClientOnConnect(this);
        }
        public void OnPlayerChoosedCharacter(object? sender, System.EventArgs e)
        {
            LoggedIn = true;
            Tutorial tutorial = new Tutorial(this);
            tutorial.Start();
            Name = Nick;
            Nick = Name;
            Color = Color.White;
            PlaySound(0);
        }
        public override void OnDisconnected(DisconnectEventArgs e)
        {
            base.OnDisconnected(e);
        }

        private static Timer _KickTimer;
        public override void Kick()
        {
            _KickTimer = new Timer(100, false);
            _KickTimer.Tick += _KickTimer_Executed;
        }
        public override void SendPlayerMessageToAll(string message)
        {
            if (InTutorial == true) return;
            base.SendPlayerMessageToAll(message);
        }
        private void _KickTimer_Executed(object sender, EventArgs e)
        {
            if(IsConnected) base.Kick();
            _KickTimer.Dispose();
        }
    }
}