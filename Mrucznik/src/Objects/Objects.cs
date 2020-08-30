
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Mruv.Objects;
using NLog.Fluent;
using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.Streamer.World;

namespace Mrucznik.Objects
{
    public class Objects
    {
        public readonly Dictionary<int, DynamicObject> ObjectsIDs = new Dictionary<int, DynamicObject>();
        
        public Objects()
        {
            LoadObjects();
            new DynamicObject(347, new Vector3(2226.0696, -1718.3290, 13.5182),
                new Vector3(-90.0000000, 0.0000000, 0.0000000), 0, 0);
            new DynamicObject(347, new Vector3(2226.0696, -1718.3290, 13.5182),
                new Vector3(-90.0000000, 0.0000000, 0.0000000), 0, 0);
        }

        public void LoadObjects()
        {
            Console.Write("Loading objects..");
            var request = new FetchAllRequest{ChunkSize = 10000};
            var call = MruV.Objects.FetchAll(request);
            {
                while(true)
                {
                    var next = call.ResponseStream.MoveNext();
                    next.Wait();
                    if (next.Result == false)
                    {
                        break;
                    }
                    Console.Write(".");
                    foreach (var currentObject in call.ResponseStream.Current.Objects)
                    {
                        var o = currentObject.Value;
                        var dynamicObject = new DynamicObject((int) o.Model,
                            new Vector3(o.X, o.Y, o.Z),
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
                }
            }
            call.Dispose();
            Console.WriteLine("\nObjects loaded.");
        }
        
    }
}