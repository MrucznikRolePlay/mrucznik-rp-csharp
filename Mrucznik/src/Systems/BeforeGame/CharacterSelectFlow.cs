using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;

namespace Mrucznik.Systems.BeforeGame
{
    public class CharacterSelectFlow
    {
        private Player _player;
        private TablistDialog _dialog;
        public event EventHandler ChoosedCharacter;


        
        public CharacterSelectFlow(Player player)
        {
            _player = player;
            _player.SendClientMessage(Color.GreenYellow, "Wybierz swoją postać aby rozpocząć rozgrywkę.");
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
                _player.Kick();
            }
            else
            {
                ChoosedCharacter?.Invoke(_player, EventArgs.Empty);
            }
        }

        public void OnPlayerChoosedCharacter(object? sender, System.EventArgs e)
        {
            _player.LoggedIn = true;
            Tutorial tutorial = new Tutorial(_player);
            tutorial.Start();
            _player.Name = _player.Nick;
            _player.Nick = _player.Name;
            _player.Color = Color.White;
            _player.PlaySound(0);
        }
        public void Start()
        {
            _dialog.Show(_player);
        }
    }
}