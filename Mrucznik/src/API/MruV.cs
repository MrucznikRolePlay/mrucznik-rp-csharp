using System;
using System.Threading.Tasks;
using Grpc.Core;
using Mruv;
using Mruv.Business;
using Mruv.Devtools;
using Mruv.Economy;
using Mruv.Elevators;
using Mruv.Entrances;
using Mruv.Estates;
using Mruv.Gates;
using Mruv.Houses;
using Mruv.Jobs;
using Mruv.Objects;
using Mruv.Offers;
using Mruv.Organizations;
using Mruv.Plots;
using Mruv.Server;
using Mruv.Spots;
using Mruv.Vehicles;

namespace Mrucznik
{
    /// <summary>
    /// MruV gRPC API.
    /// </summary>
    public static class MruV
    {
        private static Channel _channel;

        public static MruVAccountsService.MruVAccountsServiceClient Accounts { private set; get; }
        public static MruVBusinessService.MruVBusinessServiceClient Business { private set; get; }
        public static MruVCharactersService.MruVCharactersServiceClient Characters { private set; get; }
        public static MruVDevToolsService.MruVDevToolsServiceClient DevTools { private set; get; }
        public static MruVEconomyService.MruVEconomyServiceClient Economy { private set; get; }
        public static MruVElevatorsService.MruVElevatorsServiceClient Elevators { private set; get; }
        public static MruVEntrancesService.MruVEntrancesServiceClient Entrances { private set; get; }
        public static MruVEstateService.MruVEstateServiceClient Estates { private set; get; }
        public static MruVGatesService.MruVGatesServiceClient Gates { private set; get; }
        public static MruVGatesService.MruVGatesServiceClient Groups { private set; get; }
        public static MruVHousesService.MruVHousesServiceClient Houses { private set; get; }
        public static MruVItemService.MruVItemServiceClient Items { private set; get; }
        public static MruVJobsService.MruVJobsServiceClient Jobs { private set; get; }
        public static MruVObjectModelsService.MruVObjectModelsServiceClient Models { private set; get; }
        public static MruVObjectsService.MruVObjectsServiceClient Objects { private set; get; }
        public static MruVMovableObjectsService.MruVMovableObjectsServiceClient MovableObjects { private get; set; }
        public static MruVOffersService.MruVOffersServiceClient Offers { private set; get; }
        public static MruVOrganizationsService.MruVOrganizationsServiceClient Organizations { private set; get; }
        public static MruVPlotsService.MruVPlotsServiceClient Plots { private set; get; }
        public static MruVPunishmentsService.MruVPunishmentsServiceClient Punishments { private set; get; }
        public static MruVServerService.MruVServerServiceClient Server { private set; get; }
        public static MruVSpotsService.MruVSpotsServiceClient Spots { private set; get; }
        public static MruVVehiclesService.MruVVehiclesServiceClient Vehicles { private set; get; }

        public static bool Connect()
        {
            var host = "127.0.0.1:3001"; //TODO: Get hostname, port and credentials from config file
            var options = new ChannelOption("MaxReceiveMessageSize", "16MB");
            _channel = new Channel(host, ChannelCredentials.Insecure, new []{options});
            try
            {
                _channel.ConnectAsync(DateTime.UtcNow.AddSeconds(5)).Wait();
                if(_channel.State == ChannelState.Ready)
                {
                    Console.WriteLine("Connected to MruV API, channel ready.");
                }
            }
            catch (TaskCanceledException e)
            {
                throw new Exception($"Could not connect to MruV API using url: {host}", e);
            }

            Accounts = new MruVAccountsService.MruVAccountsServiceClient(_channel);
            Business = new MruVBusinessService.MruVBusinessServiceClient(_channel);
            Characters = new MruVCharactersService.MruVCharactersServiceClient(_channel);
            DevTools = new MruVDevToolsService.MruVDevToolsServiceClient(_channel);
            Economy = new MruVEconomyService.MruVEconomyServiceClient(_channel);
            Elevators = new MruVElevatorsService.MruVElevatorsServiceClient(_channel);
            Estates = new MruVEstateService.MruVEstateServiceClient(_channel);
            Entrances = new MruVEntrancesService.MruVEntrancesServiceClient(_channel);
            Gates = new MruVGatesService.MruVGatesServiceClient(_channel);
            Groups = new MruVGatesService.MruVGatesServiceClient(_channel);
            Houses = new MruVHousesService.MruVHousesServiceClient(_channel);
            Items = new MruVItemService.MruVItemServiceClient(_channel);
            Jobs = new MruVJobsService.MruVJobsServiceClient(_channel);
            Models = new MruVObjectModelsService.MruVObjectModelsServiceClient(_channel);
            Objects = new MruVObjectsService.MruVObjectsServiceClient(_channel);
            MovableObjects = new MruVMovableObjectsService.MruVMovableObjectsServiceClient(_channel);
            Offers = new MruVOffersService.MruVOffersServiceClient(_channel);
            Organizations = new MruVOrganizationsService.MruVOrganizationsServiceClient(_channel);
            Plots = new MruVPlotsService.MruVPlotsServiceClient(_channel);
            Punishments = new MruVPunishmentsService.MruVPunishmentsServiceClient(_channel);
            Server = new MruVServerService.MruVServerServiceClient(_channel);
            Spots = new MruVSpotsService.MruVSpotsServiceClient(_channel);
            Vehicles = new MruVVehiclesService.MruVVehiclesServiceClient(_channel);
            return true;
        }

        public static void Disconnect()
        {
            _channel.ShutdownAsync().Wait(30000); //30s wait
        }
    }
}