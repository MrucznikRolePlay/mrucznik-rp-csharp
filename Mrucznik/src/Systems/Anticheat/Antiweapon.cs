using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;

namespace Mrucznik.Systems.Anticheat
{
    public class Antiweapon
    {
        private Player _player;
        private int securitylevel;
        private Timer _AntiweaponTimer;
        public Antiweapon(Player player)
        {
            _player = player;
            _AntiweaponTimer = new Timer(30, true);
            _AntiweaponTimer.Tick += _AntiweaponTimer_Tick;
            SetSecurityLevel(1);
        }

        private void _AntiweaponTimer_Tick(object sender, System.EventArgs e)
        {
            var weapon = _player.Weapon;
            if((weapon == Weapon.ThermalGoggles ||
               weapon == Weapon.RocketLauncher ||
               weapon == Weapon.Sawedoff ||
               weapon == Weapon.Minigun) && securitylevel > 0)
            {
                _player.SetArmedWeapon(Weapon.Unarmed);
                _player.SendClientMessage(Color.Red, "Zostałeś wyrzucony z serwera za nielegalną broń.");
                _player.Kick();
            }
        }

        public void SetSecurityLevel(int level)
        {
            //level: 0 - wylaczone, 1 - wszystkie bronie oprocz zakazanych, 2 - wszystkie bronie oprocz bc, 3 - żadne bronie
            securitylevel = level;
        }
    }
}
