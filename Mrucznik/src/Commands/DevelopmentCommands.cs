using Mrucznik.Systems;
using Mruv;
using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;

namespace Mrucznik.Commands
{
    public class DevelopmentCommands
    {
        [Command("getvw")]
        private static void GetVirtualWorld(BasePlayer sender)
        {
            sender.SendClientMessage($"Twój wirtual world to: {sender.VirtualWorld}");
        }

        [Command("setvw")]
        private static void SetVirtualWorld(BasePlayer sender, int vw, BasePlayer giveplayer = null)
        {
            if(giveplayer == null)
            {
                sender.SendClientMessage($"Ustawiłeś swój virtual world na: {vw}");
                sender.VirtualWorld = vw;
            }
            else
            {
                sender.SendClientMessage($"Ustawiłeś virtual world {giveplayer} na: {vw}");
                giveplayer.SendClientMessage($"{sender} ustawił Twój virtual world na: {vw}");
                giveplayer.VirtualWorld = vw;
            }
        }
        
        [Command("getint")]
        private static void GetInterior(BasePlayer sender)
        {
            sender.SendClientMessage($"Twój interior to: {sender.Interior}");
        }

        [Command("setint")]
        private static void SetInterior(BasePlayer sender, int interior, BasePlayer giveplayer = null)
        {
            if(giveplayer == null)
            {
                sender.SendClientMessage($"Ustawiłeś swój interior na: {interior}");
                sender.VirtualWorld = interior;
            }
            else
            {
                sender.SendClientMessage($"Ustawiłeś interior {giveplayer} na: {interior}");
                giveplayer.SendClientMessage($"{sender} ustawił Twój interior na: {interior}");
                giveplayer.VirtualWorld = interior;
            }
        }
        
        [Command("gotopos")]
        private static void GotoPos(BasePlayer sender, float x, float y, float z)
        {
            sender.SendClientMessage($"Teleportowałeś się do pozycji: {x}, {y}, {z}");
            sender.Position = new Vector3(x, y, z);
        }
        
        [Command("gotopos")]
        private static void GotoPos(BasePlayer sender, string pos)
        {
            double x = 0.0, y = 0.0, z = 0.0;
            sender.SendClientMessage($"Teleportowałeś się do pozycji: {pos}");
            var values = pos.Split(",");
            if(values.Length > 0)
            {
                if (!double.TryParse(values[0], out x)
                || !double.TryParse(values[1], out y) 
                || !double.TryParse(values[2], out z))
                {
                    sender.SendClientMessage("Złe argumenty!");
                    return;
                }
            }
            sender.Position = new Vector3(x, y, z);
        }
        
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