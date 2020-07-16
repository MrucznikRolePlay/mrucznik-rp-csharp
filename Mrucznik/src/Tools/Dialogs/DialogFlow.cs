using System;
using System.Collections;
using System.Collections.Generic;
using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class DialogFlow : IDialogFlow
    {
        ArrayList dialogs = new ArrayList();

        public DialogFlow()
        {
            dialogs.Add(new MessageDialog("caption", "message", "button1", "button2"));
            var d = TablistDialogBuilder.Create()
                .WithCaption("kox")
                .WithMessage("elo")
                .WithLeftButton("Siema")
                .WithAction(((sender, args) => { Console.WriteLine("Elo"); }))
                .WithRightButton("PL")
                .WithAction(((sender, args) => { Console.WriteLine("ENG"); }))
                .WithResponseAction((sender, args) => { })
                .Continue()
                .WithHeaders("kek", "123")
                .WithRow("123", "123")
                .Build();
        }

        public void Start()
        {
        }

        public void Next()
        {
        }

        public void Previous()
        {
            throw new System.NotImplementedException();
        }

        public void AddDialog(Dialog dialog)
        {
            throw new System.NotImplementedException();
        }
    }
}