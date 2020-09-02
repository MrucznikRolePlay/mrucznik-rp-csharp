using SampSharp.Streamer.World;

namespace Mrucznik
{
    public static class DynamicObjectExtensions
    {
        /// <summary>
        /// Get model name or return model id as string
        /// </summary>
        /// <returns></returns>
        public static string GetName(this DynamicObject o)
        {
            if (Objects.Objects.ObjectModels.ContainsKey(o.ModelId))
            {
                return Objects.Objects.ObjectModels[o.ModelId].Name;
            }
            return o.ModelId.ToString();
        }
    }
}