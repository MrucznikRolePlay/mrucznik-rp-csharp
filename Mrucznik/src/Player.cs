using System;
using mrucznik;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using static mrucznik.PlayerHelpers;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        private RealTime _realTime;
        public AntiSpawn _antiSpawn;
        public string Nick;
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
            SetupClientOnConnect(this);
        }
        public void OnPlayerChoosedCharacter(object? sender, System.EventArgs e)
        {
            LoggedIn = true;
            Name = Nick;
            Nick = Name;
            Color = Color.White;
            SetSpawnInfo(0, Skin, new Vector3(1759.0189f, -1898.1260f, 13.5622f), 266.4503f);
            ToggleControllable(true);
            ToggleSpectating(false);
            Spawn();
            VirtualWorld = 0;
            
            ApplyAnimation("CARRY", "crry_prtial", 1.0f, false, false, false, false, 0);
            ClearAnimations();
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
        private void _KickTimer_Executed(object sender, EventArgs e)
        {
            base.Kick();
        }
    }
}