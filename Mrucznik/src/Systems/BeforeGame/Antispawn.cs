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
    }
}
