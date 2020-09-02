using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.Events;

namespace Mrucznik.Systems.AntiCheat
{
    public class AntiSpawn
    {
        private readonly Player _player;
        public AntiSpawn(Player player)
        {
            _player = player;
            _player.Spawned += PlayerOnSpawned;
            _player.Connected += _player_Connected;
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
    }
}
