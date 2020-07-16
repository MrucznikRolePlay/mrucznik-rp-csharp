using Mrucznik.Systems;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.SAMP.Commands.PermissionCheckers;
using SampSharp.GameMode.World;

namespace Mrucznik.Commands
{
    public class DevelopmentCommands
    {
        
        [Command("testdialogflow")]
        private static void LocalChatCommand(BasePlayer sender)
        {
            sender.SendClientMessage("Test dialog flow started");
            var dialog = new DialogFlow(false)
                .AddDialog(new MessageDialog("Dialog 1", "Pierwszy dialog", "Dalej", "Wyjdź"))
                .AddDialog(new InputDialog("Dialog 2", "Drugi dialog", false, "Dalej", "Cofnij"))
                .AddDialog(new MessageDialog("Dialog 3", "Ostatni dialog", "Gotowe", "Cofnij"))
                .End((o, args) =>
            {
                foreach (var arg in args)
                {
                    sender.SendClientMessage($"Output {arg.Key}: input: {arg.Value.InputText} | listitem: {arg.Value.ListItem}");
                }
            });
            dialog.Show(sender);
        }
    }
}