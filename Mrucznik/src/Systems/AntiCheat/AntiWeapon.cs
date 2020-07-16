using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;

namespace Mrucznik.Systems.AntiCheat
{
    public class AntiWeapon
    {
        private readonly Player _player;
        private int _securityLevel;
        private readonly Timer _antiWeaponTimer;
        
        public AntiWeapon(Player player)
        {
            _player = player;
            _antiWeaponTimer = new Timer(30, true);
            _antiWeaponTimer.Tick += AntiWeaponTimerTick;
            SetSecurityLevel(1);
        }

        private void AntiWeaponTimerTick(object sender, System.EventArgs e)
        {
            var weapon = _player.Weapon;
            if((weapon == Weapon.ThermalGoggles ||
               weapon == Weapon.RocketLauncher ||
               weapon == Weapon.Sawedoff ||
               weapon == Weapon.Minigun) && _securityLevel > 0)
            {
                _player.SetArmedWeapon(Weapon.Unarmed);
                _player.SendClientMessage(Color.Red, "Zostałeś wyrzucony z serwera za nielegalną broń.");
                _player.Kick();
            }
        }

        public void SetSecurityLevel(int level)
        {
            //level: 0 - wylaczone, 1 - wszystkie bronie oprocz zakazanych, 2 - wszystkie bronie oprocz bc, 3 - żadne bronie
            _securityLevel = level;
        }
    }
}
