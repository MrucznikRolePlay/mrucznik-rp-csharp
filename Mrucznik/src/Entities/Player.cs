using System;
using System.Collections.Generic;
using Mrucznik.PlayerComponents;
using Mrucznik.PlayerStates;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class Player : BasePlayer
    {
        protected List<IPlayerComponent> Behaviours { get; private set; }
        
        public Player()
        {
            Behaviours = new List<IPlayerComponent>();
        }

        public void RegisterBehaviour(IPlayerComponent component)
        {
            Behaviours.Add(component);
            component.RegisterComponent(this);
        }

        public void UnregisterBehaviour(IPlayerComponent component)
        {
            Behaviours.Remove(component);
            component.UnregisterComponent(this);
        }

        public void ClearBehavoiurs()
        {
            foreach (var behaviour in Behaviours)
            {
                behaviour.UnregisterComponent(this);
            }
            Behaviours.Clear();
        }

        public override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);

            RegisterBehaviour(new UninitializedPlayer());
        }
    }
}