using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;

namespace Mrucznik
{
    public class CharacterSelectFlow
    {
        private BasePlayer _player;
        private TablistDialog _dialog;
        public event EventHandler ChoosedCharacter;


        
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
            if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Right)
            {
                _player.SendClientMessage(Color.LightCoral, "Wyszedłeś z wyboru postaci. Zapraszamy ponownie!");
                _player.Spawn();
            }
            else
            {
                ChoosedCharacter?.Invoke(_player, EventArgs.Empty);
            }
        }

        public void Start()
        {
            _dialog.Show(_player);
        }
    }
}