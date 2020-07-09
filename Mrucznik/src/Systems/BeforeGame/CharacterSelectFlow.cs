using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class CharacterSelectFlow
    {
        private BasePlayer _player;
        private TablistDialog _dialog;

        public CharacterSelectFlow(BasePlayer player)
        {
            _player = player;
            _dialog = new TablistDialog("Wybór postaci", new []
            {
                "Nick", "Level"
            }, "Wybierz", "Wyjdź");
            _dialog.Response += DialogOnResponse;
            
        }

        private void DialogOnResponse(object? sender, DialogResponseEventArgs e)
        {
            
        }

        public void Start()
        {
            _dialog.Show(_player);
        }
    }
}