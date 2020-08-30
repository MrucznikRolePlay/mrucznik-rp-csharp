
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Mruv.Objects;
using NLog.Fluent;
using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.World;
using SampSharp.Streamer.World;

namespace Mrucznik.Objects
{
    public class Objects
    {
        public readonly Dictionary<int, DynamicObject> ObjectsIDs = new Dictionary<int, DynamicObject>();
        
        public Objects()
        {
            new DynamicObject(347, new Vector3(2399.20972, -1983.04883, 33.4963074),
                new Vector3(-90.0000000, 0.0000000, 0.0000000), 31, -1, 
                null, 300.0f, 300.0f, null, 0);
            new DynamicObject(347, new Vector3(2399.20972, -1983.04883, 33.4963074),
                new Vector3(-90.0000000, 0.0000000, 0.0000000), 31, -1, 
                null, 300.0f, 300.0f, null, 0);
            LoadObjects();
        }

        public async void LoadObjects()
        {
            Console.Write("Loading objects..");
            var request = new FetchAllRequest();
            request.ChunkSize = 10000;
            using (var call = MruV.Objects.FetchAll(request))
            {
                List<Task> jobs = new List<Task>();
                while (await call.ResponseStream.MoveNext())
                {
                    Console.Write(".");
                    var data = call.ResponseStream.Current.Objects;
                    var task = new Task(() =>
                    {
                        Console.Write(" started job ");
                        foreach (var currentObject in data)
                        {
                            var o = currentObject.Value;
                            var dynamicObject = new DynamicObject((int) o.Model,
                                new Vector3(o.X, o.Y, o.X),
                                new Vector3(o.Rx, o.Ry, o.Rz),
                                o.WorldId, o.InteriorId, null, o.StreamDistance, o.DrawDistance,
                                null, o.Priority); // TODO: o.PlayerId i o.AreaId

                            foreach (var material in o.Materials)
                            {
                                dynamicObject.SetMaterial((int) material.Key, material.Value.ModelId,
                                    material.Value.TxdName, material.Value.TextureName, material.Value.MaterialColor);
                            }
                            
                            foreach (var materialText in o.MaterialTexts)
                            {
                                var mt = materialText.Value;
                                dynamicObject.SetMaterialText((int) materialText.Key, mt.Text,
                                    (ObjectMaterialSize) mt.MaterialSize,
                                    mt.FontFace, (int) mt.FontSize, mt.Bold, mt.FontColor, mt.BackColor,
                                    (ObjectMaterialTextAlign) mt.TextAlignment);
                            }
                            ObjectsIDs[currentObject.Key] = dynamicObject;
                        }

                        Console.Write(" created ");
                    });
                    jobs.Add(task);
                    task.Start();
                    break;
                }
                Console.Write(" fetching done ");
                Task.WaitAll(jobs.ToArray());
                Console.Write(" creating done ");
            }
            Console.WriteLine("\nLoaded");
        }
        
    }
}