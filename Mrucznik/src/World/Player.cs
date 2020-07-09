using SampSharp.GameMode;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        public void SetSpawnPlayer()
        {
            Position = new Vector3(1759.0189f, -1898.1260f, 13.5622f);
            Health = 100;
            Skin = 13;
            Color = Color.White;
            SendClientMessage(Color.Gray, "Zosta³eœ przywrócony do miejsca spawnu.");
            ToggleControllable(true);
            ApplyAnimation("CARRY", "crry_prtial", 1, false, false, false, false, 0);
            ClearAnimations();
        }

        
    }
}