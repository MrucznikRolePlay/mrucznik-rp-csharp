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
        public Antispawn _antispawn;
        public Antiweapon _antiweapon;
        public string Nick;
        //zmienne
        public bool LoggedIn;
        public bool InTutorial;
        
        public Player()
        {
            _realTime = new RealTime(this);
            _antispawn = new Antispawn(this);
            _antiweapon = new Antiweapon(this);
        }
        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);
            SetupClientOnConnect(this);
        }
        
        public override void OnDisconnected(DisconnectEventArgs e)
        {
            base.OnDisconnected(e);
        }

        private static Timer _KickTimer;
        public override void Kick()
        {
            _KickTimer = new Timer(10, false);
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