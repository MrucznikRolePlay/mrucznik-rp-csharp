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
        private event EventHandler ChoosedCharacter;
        
        public CharacterSelectFlow(Player player)
        {
            _player = player;
            _player.SendClientMessage(Color.GreenYellow, "Wybierz swoją postać aby rozpocząć rozgrywkę.");
            _dialog = new TablistDialog("Wybór postaci", new []
            {
                "Nick", "Level"
            }, "Wybierz", "Wyjdź");
            _dialog.Response += DialogOnResponse;
            ChoosedCharacter += OnPlayerChoosedCharacter;

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

        private void OnPlayerChoosedCharacter(object? sender, System.EventArgs e)
        {
            _player.LoggedIn = true;
            _player.Name = _player.Nick;
            _player.Nick = _player.Name;
            _player.Color = Color.White;
            _player.PlaySound(0);
            _player.SetSpawnInfo(0, 0, new Vector3(180.0f, 150.0f, 8.0f), 90.0f);
            _player.Spawn();
            _player.Position = new Vector3(150.0f, 130.0f, 8.0f);
        }
        public void Start()
        {
            _dialog.Show(_player);
        }
    }
}