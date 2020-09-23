namespace Mrucznik.PlayerComponents
{
    public interface IPlayerComponent
    {
        public void RegisterComponent(Player player);
        public void UnregisterComponent(Player player);
    }
}