using System;
using System.Threading.Tasks;
using Mrucznik.Helpers;
using Mrucznik.Objects;
using Mrucznik.Systems.AntiCheat;
using Mrucznik.Systems.BeforeGame;
using Mruv;
using Mruv.Economy;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;

namespace Mrucznik.PlayerStates
{
    public class OldPlayer : Player
    {
        public uint AccountID { get; private set; }
        public Character PlayerCharacter;
        public bool LoggedIn;
        public readonly ObjectEditor ObjectEditor;

        private RealTime _realTime;
        private AntiSpawn _antiSpawn;
        private AntiWeapon _antiWeapon;

        public OldPlayer()
        {
            // Init player components
            _realTime = new RealTime(this);
            //_antiSpawn = new AntiSpawn(this);
            _antiWeapon = new AntiWeapon(this);
            ObjectEditor = new ObjectEditor(this);
        }

        #region callbacks

        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);

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
        }

        public override void OnRequestSpawn(RequestSpawnEventArgs e)
        {
            base.OnRequestSpawn(e);
        }

        public override void OnSpawned(SpawnEventArgs e)
        {
            base.OnSpawned(e);
        }

        public void OnPlayerRegister()
        {   
        }
        
        public void OnPlayerLogin(uint accountId)
        {
            
        }

        public void OnPlayerCreateCharacter(Character playerCharacter)
        {
            
        }

        public void OnPlayerSelectCharacter(Character playerCharacter)
        {
            
        }

        #endregion

        #region overrides

        public override void Kick()
        {
            var timer = new Timer(10, false);
            timer.Tick += (sender, args) =>
            {
                if (IsConnected) base.Kick();
            };
        }

        public override void Ban()
        {
            Ban("SYSTEM BAN");
            Kick();
        }

        #endregion

        public void RconBan()
        {
            base.Ban();
        }

        public void Ban(string reason)
        {
            MruV.Punishments.BanAsync(new BanRequest
            {
                Admin = 0,
                Character = PlayerCharacter.Id,
                Ip = IP,
                Player = AccountID,
                Reason = reason,
                Time = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            });
            Kick();
        }

        public void Ban(OldPlayer admin, string reason)
        {
            MruV.Punishments.BanAsync(new BanRequest
            {
                Admin = admin.AccountID,
                Character = PlayerCharacter.Id,
                Ip = IP,
                Player = AccountID,
                Reason = reason,
                Time = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            });
            Kick();
        }

        public void Block(string reason)
        {
            MruV.Punishments.BlockAsync(new BlockRequest
            {
                Admin = 0,
                Character = PlayerCharacter.Id,
                Reason = reason
            });
            Kick();
        }

        public void Block(OldPlayer admin, string reason)
        {
            MruV.Punishments.BlockAsync(new BlockRequest
            {
                Admin = admin.AccountID,
                Character = PlayerCharacter.Id,
                Reason = reason
            });
            Kick();
        }

        public void InstaKick()
        {
            base.Kick();
        }

        public void Kick(string reason)
        {
            SendClientMessage($"Zostałeś skickowany za: {reason}");
            Kick();
        }


        
    }
}