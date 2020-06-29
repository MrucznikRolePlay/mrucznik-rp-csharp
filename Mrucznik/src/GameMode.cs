using System;
using System.Text.RegularExpressions;
using Mrucznik.Controllers;
using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using Timer = System.Timers.Timer;

namespace Mrucznik
{
    public class GameMode : BaseMode
    {
        #region Overrides of BaseMode
		
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var version = GetType().Assembly.GetName().Version?.ToString();

            Console.WriteLine("\n----------------------------------");
            Console.WriteLine("M | --- Mrucznik Role Play --- | M");
            Console.WriteLine("R | ---        ****        --- | R");
            Console.WriteLine("U | ---        v3.0        --- | U");
            Console.WriteLine("C | ---        ****        --- | C");
            Console.WriteLine("Z | ---    by Mrucznik     --- | Z");
            Console.WriteLine("N | ---                    --- | N");
            Console.WriteLine("I | ---       /\\_/\\        --- | I");
            Console.WriteLine("K | ---   ===( *.* )===    --- | K");
            Console.WriteLine("  | ---       \\_^_/        --- |  ");
            Console.WriteLine("R | ---         |          --- | R");
            Console.WriteLine("P | ---         O          --- | P");
            Console.WriteLine("----------------------------------");
            
            //Ustawienia SAMP'a
            SetGameModeText($"Mrucznik-RP v{version}");
            AllowInteriorWeapons(true); 
            ShowPlayerMarkers(PlayerMarkersMode.Off);
            DisableInteriorEnterExits();
            EnableStuntBonusForAll(false);
            ManualVehicleEngineAndLights();
            ShowNameTags(true);
            SetNameTagDrawDistance(70.0f);

            Server.SetWeather(2);
    
            // Classes
            for(int i=0; i<311; i++)
                AddPlayerClass(i, new Vector3(1759.0189f, -1898.1260f, 13.5622f), 266.4503f);
        }
        
        protected override void OnExited(EventArgs e)
        {
            base.OnExited(e);

            Console.WriteLine("----------------------------------");
            Console.WriteLine("---------- GAMEMODE OFF ----------");
            Console.WriteLine("----------------------------------");
        }
        
        protected override void OnPlayerConnected(BasePlayer player, EventArgs e)
        {
            base.OnPlayerConnected(player, e);

            if (!Regex.IsMatch(player.Name, "^[A-Z][a-z]+(_[A-Z][a-z]+([A-HJ-Z][a-z]+)?){1,2}$"))
            {
                player.SendClientMessage("SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
                player.Kick();
                return;
            }
            
            // Set time to real world time
            player.ToggleClock(true);
        }
        
        protected override void LoadControllers(ControllerCollection controllers)
        {
            base.LoadControllers(controllers);

            controllers.Override(new PlayerController());
        }

        #endregion
    }
}