using System;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using static Mrucznik.Helpers.PlayerHelpers;
using Mrucznik.Systems.AntiCheat;
using Mrucznik.Systems.BeforeGame;
using Mruv;
using SampSharp.GameMode.Definitions;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        public Account PlayerAccount;
        public Character PlayerCharacter;
        public bool LoggedIn;
        
        private RealTime _realTime;
        private AntiSpawn _antiSpawn;
        private AntiWeapon _antiWeapon;
        
        public Player()
        {
            // Init player components
            _realTime = new RealTime(this);
            _antiSpawn = new AntiSpawn(this);
            _antiWeapon = new AntiWeapon(this);
        }
        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);
            //SetupClientOnConnect(this);
        }

        public override void Kick()
        {
            var timer = new Timer(10, false);
            timer.Tick += (sender, args) =>
            {
                if(IsConnected) base.Kick();
            };
        }
        public override void SendPlayerMessageToAll(string message)
        {
            if (LoggedIn == true) return;
            base.SendPlayerMessageToAll(message);
        }
    }
}