namespace Mrucznik
{
    /// <summary>
    /// Tutaj znajdziesz metody rozszerzające klasę Player
    /// </summary>
    public static class PlayerExtensions
    {
        public static void ClearChat(this Player _player)
        {
            for(var i=0; i<15; i++)
                _player.SendClientMessage("");
        }
    }
}