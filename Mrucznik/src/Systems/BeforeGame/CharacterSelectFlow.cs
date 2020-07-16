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
        private readonly Player _player;
        private readonly TablistDialog _dialog;
        private event EventHandler ChosenCharacter;
        
        public CharacterSelectFlow(Player player)
        {
            _player = player;
            _player.SendClientMessage(Color.GreenYellow, "Wybierz swoją postać aby rozpocząć rozgrywkę.");
            _dialog = new TablistDialog("Wybór postaci", new []
            {
                "Nick", "Level"
            }, "Wybierz", "Wyjdź");
            _dialog.Response += DialogOnResponse;
            ChosenCharacter += OnPlayerChosenCharacter;

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
                ChosenCharacter?.Invoke(_player, EventArgs.Empty);
            }
        }

        private void OnPlayerChosenCharacter(object? sender, System.EventArgs e)
        {
            _player.LoggedIn = true;
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