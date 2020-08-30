using System;
using System.Linq;
using Grpc.Core;
using Mrucznik.Controllers;
using Mruv.Server;
using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using Server = SampSharp.GameMode.SAMP.Server;

namespace Mrucznik
{
    public class GameMode : BaseMode
    {
        private Objects.Objects _objects;

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

            // temp skin
            AddPlayerClass(0, new Vector3(2226.0696, -1718.3290, 13.5182), 0.0f);
            AddPlayerClass(13, new Vector3(-2571.25, 2316.0, 5.2), 0.0f);

            //Ustawienia SAMP'a
            SetGameModeText($"Mrucznik-RP v{version}");
            AllowInteriorWeapons(true);
            ShowPlayerMarkers(PlayerMarkersMode.Off);
            DisableInteriorEnterExits();
            EnableStuntBonusForAll(false);
            ManualVehicleEngineAndLights();
            ShowNameTags(true);
            SetNameTagDrawDistance(70.0f);
            //UsePlayerPedAnimations(); //CJ run on all skins when enabled
            Server.SetWeather(2);

            // Connect to MruV API
            Console.WriteLine("Connecting to MruV API...");
            try
            {
                MruV.Connect();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                Exit();
                return;
            }

            Console.WriteLine("Connected, registering server in API.");
            try
            {
                var serverId = MruV.Server.RegisterServer(new ServerInfo()
                {
                    Host = "127.0.0.1",
                    Port = "7777",
                    Name = "!PL! Mrucznik Role Play 3.0. !PL!",
                    Platform = "SA-MP",
                    Players = 0
                });
                Console.WriteLine($"Registered server in MruV API, server id: {serverId}");
            }
            catch (RpcException err)
            {
                Console.WriteLine($"MruV API Error[{err.Status.StatusCode}]: {err.Status.Detail}");
            }

            // Create objects
            _objects = new Objects.Objects();
        }

        protected override void OnExited(EventArgs e)
        {
            base.OnExited(e);

            try
            {
                MruV.Disconnect();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }

            Console.WriteLine("----------------------------------");
            Console.WriteLine("---------- GAMEMODE OFF ----------");
            Console.WriteLine("----------------------------------");
        }

        protected override void LoadControllers(ControllerCollection controllers)
        {
            base.LoadControllers(controllers);

            controllers.Override(new PlayerController());
        }

        protected override void OnPlayerConnected(BasePlayer player, EventArgs e)
        {
            base.OnPlayerConnected(player, e);

            _objects.RemoveBuildingsForPlayer(player);
        }

        #endregion
    }
}