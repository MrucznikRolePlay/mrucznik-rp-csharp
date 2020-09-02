using System;
using Mrucznik.Objects;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using Mrucznik.Systems.AntiCheat;
using Mruv;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        public Character PlayerCharacter;
        public bool LoggedIn;
        public ObjectEditorState ObjectEditorState;
        
        private RealTime _realTime;
        private AntiSpawn _antiSpawn;
        private AntiWeapon _antiWeapon;
        
        public Player()
        {
            // Init player components
            _realTime = new RealTime(this);
            //_antiSpawn = new AntiSpawn(this);
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