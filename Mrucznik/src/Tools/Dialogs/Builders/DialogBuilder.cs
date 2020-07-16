using System;
using System.Collections;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;

namespace Mrucznik.Systems
{
    public abstract class DialogBuilder<TDialog, TBuilder, TFrom> :
        DialogBuilder<TDialog, TBuilder, TFrom>.IFrom, 
        DialogBuilder<TDialog, TBuilder, TFrom>.IMessage, 
        DialogBuilder<TDialog, TBuilder, TFrom>.ILeftButton, 
        DialogBuilder<TDialog, TBuilder, TFrom>.IWithLeftButtonAction, 
        DialogBuilder<TDialog, TBuilder, TFrom>.IWithRightButtonAction
        where TDialog : Dialog
        where TBuilder : DialogBuilder<TDialog, TBuilder, TFrom>, new()
    {
        public interface IFrom
        {
            IMessage WithCaption(string caption);
        }
    
        public interface IMessage
        {
            ILeftButton WithMessage(string message);
        }

        public interface ILeftButton
        {
            IWithLeftButtonAction WithLeftButton(string button);
        }

        public interface IWithLeftButtonAction : IRightButton
        {
            IRightButton WithAction(EventHandler<DialogResponseEventArgs> action);
        }
    
        public interface IRightButton
        {
            IWithRightButtonAction WithRightButton(string button);
        }
    
        public interface IWithRightButtonAction : IOthers, ILast
        {
            IOthers WithAction(EventHandler<DialogResponseEventArgs> action);
        }

        public interface IOthers : ILast
        {
            ILast WithResponseAction(EventHandler<DialogResponseEventArgs> action);
        }

        public interface ILast
        {
            TFrom Continue();
            TDialog Build();
        }
        
        protected string Caption;
        protected string Message;
        protected string LeftButton;
        protected EventHandler<DialogResponseEventArgs> LeftButtonAction;
        protected string RightButton;
        protected EventHandler<DialogResponseEventArgs> RightButtonAction;
        protected EventHandler<DialogResponseEventArgs> Action;

        protected DialogBuilder() {}
        
        public static IFrom Create()
        {
            return new TBuilder();
        }
        
        IMessage IFrom.WithCaption(string caption)
        {
            Caption = caption;
            return this;
        }

        ILeftButton IMessage.WithMessage(string message)
        {
            Message = message;
            return this;
        }

        IWithLeftButtonAction ILeftButton.WithLeftButton(string button)
        {
            LeftButton = button;
            return this;
        }

        IRightButton IWithLeftButtonAction.WithAction(EventHandler<DialogResponseEventArgs> action)
        {
            LeftButtonAction = action;
            return this;
        }

        IWithRightButtonAction IRightButton.WithRightButton(string button)
        {
            RightButton = button;
            return this;
        }

        IOthers IWithRightButtonAction.WithAction(EventHandler<DialogResponseEventArgs> action)
        {
            RightButtonAction = action;
            return this;
        }

        ILast IOthers.WithResponseAction(EventHandler<DialogResponseEventArgs> action)
        {
            Action = action;
            return this;
        }

        public abstract TFrom Continue();

        public abstract TDialog Build();
    }
}