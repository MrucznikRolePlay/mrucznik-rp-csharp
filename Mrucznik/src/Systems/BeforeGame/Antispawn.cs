using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using SampSharp.GameMode.Events;
using System.ComponentModel;
using Mrucznik;
using System;

namespace mrucznik
{
    public class AntiSpawn 
    {
        private bool CanSpawn;
        private Player _player;
        public AntiSpawn(Player player)
        {
            this._player = player;
            _player.Spawned += PlayerOnSpawned;
            _player.Connected += _player_Connected;
            _player.CommandText += _player_CommandText;
            _player.Text += _player_Text;
        }

        private void _player_Connected(object sender, System.EventArgs e)
        {
            _player.LoggedIn = false;
            _player.Color = Color.Gray;
            _player.VirtualWorld = _player.Id + 300;
            _player.ToggleControllable(false);
            _player.ToggleSpectating(true);
        }
        
        private void PlayerOnSpawned(object? sender, SpawnEventArgs e)
        {
            if(_player.LoggedIn == false)
            {
                _player.SendClientMessage(Color.Red, "Zostałeś wyrzucony z serwera za nielegalny spawn.");
                _player.Kick();
            }
        }
        private void _player_CommandText(object sendet, EventArgs e)
        {
            if (_player.LoggedIn == false)
            {
                return;
            }
        }

        private void _player_Text(object sendet, EventArgs e)
        {
            if (_player.LoggedIn == false)
            {
                return;
            }
        }
        public void OnPlayerChoosedCharacter(object? sender, System.EventArgs e)
        {
            _player.LoggedIn = true;
            _player.Spawn();
            _player.Color = Color.White;
            _player.VirtualWorld = 0;
            _player.Position = new Vector3(1759.0189f, -1898.1260f, 13.5622f);
            _player.Rotation = new Vector3(266.4503f);
            _player.ToggleControllable(true);
            _player.ToggleSpectating(false);
            _player.ApplyAnimation("CARRY", "crry_prtial", 1.0f, false, false, false, false, 0);
            _player.ClearAnimations();
        }
    }
}
