using Mrucznik.Systems;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;

namespace Mrucznik.Commands
{
    public class DevelopmentCommands
    {
        [Command("testdialogflow")]
        private static void TestDialogFlow(BasePlayer sender)
        {
            sender.SendClientMessage("Test dialog flow started");
            var first = new ListDialog("Dialog 1", "Dalej", "Wyjdź");
            first.Items.Add("Pierwsza opcja");
            first.Items.Add("Druga opcja");
            first.Items.Add("Trzecia opcja");
            IDialog dialog = new DialogFlow(false)
                .AddDialog(first)
                .AddDialog(new InputDialog("Dialog 2", "Drugi dialog", false, "Dalej", "Cofnij"))
                .AddDialog(new MessageDialog("Dialog 3", "Ostatni dialog", "Gotowe", "Cofnij"))
                .End((o, args) =>
                {
                    foreach (var arg in args)
                    {
                        sender.SendClientMessage(
                            $"Output {arg.Key}: input: {arg.Value.InputText} | listitem: {arg.Value.ListItem}");
                    }
                });
            dialog.Show(sender);
        }

        [Command("testdialogflow2")]
        private static void TestDialogFlow2(BasePlayer sender)
        {
            sender.SendClientMessage("Test dialog flow started");
            var dialog = new DialogFlow(false)
                .AddDialog(ListDialogBuilder.Create()
                    .WithCaption("Dialog 1")
                    .WithMessage("Pierwszy dialog")
                    .WithLeftButton("Dalej")
                    .WithRightButton("Wyjdź")
                    .Continue()
                    .WithRow("Pierwsza opcja")
                    .WithRow("Druga opcja")
                    .WithRow("Trzecia opocja")
                    .Build())
                .AddDialog(InputDialogBuilder.Create()
                    .WithCaption("Dialog 2")
                    .WithMessage("Dialog drugi")
                    .WithLeftButton("Dalej")
                    .WithRightButton("Cofnij")
                    .Continue().PasswordStyle()
                    .Build())
                .AddDialog(MessageDialogBuilder.Create()
                    .WithCaption("Dialog 3")
                    .WithMessage("Trzeci dialog")
                    .WithLeftButton("Gotowe")
                    .WithRightButton("Cofnij")
                    .Build())
                .End((o, args) =>
                {
                    foreach (var arg in args)
                    {
                        sender.SendClientMessage(
                            $"Output {arg.Key}: input: {arg.Value.InputText} | listitem: {arg.Value.ListItem}");
                    }
                });
            dialog.Show(sender);
        }
    }
}