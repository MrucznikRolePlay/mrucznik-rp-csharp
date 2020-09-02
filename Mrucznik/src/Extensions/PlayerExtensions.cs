using SampSharp.Core.Natives;
using SampSharp.GameMode;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    /// <summary>
    /// Tutaj znajdziesz metody rozszerzające klasę Player
    /// </summary>
    public static class PlayerExtensions
    {
        private static INative _cancelEditNative = BaseMode.Instance.Client.NativeLoader.Load(
            "CancelEdit", new []{ NativeParameterInfo.ForType(typeof(int))});
        
        public static void ClearChat(this BasePlayer player)
        {
            for(var i=0; i<15; i++)
                player.SendClientMessage("");
        }

        public static bool CancelEdit(this BasePlayer player)
        {
            return _cancelEditNative.InvokeBool(player.Id);
        }
    }
}