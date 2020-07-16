using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public class FlowComponent
    {
        public FlowComponent next;
        public FlowComponent previous;
    }
    
    public abstract class DialogFlowBuilder
    {
        private FlowComponent _component;
        
        protected DialogFlowBuilder()
        {
            _component.previous = _component;
        }

        public DialogFlowBuilder Next(DialogBuilder builder)
        {
            _component.next = new FlowComponent();
            _component.next.previous = _component;
            _component = _component.next;
            return this;
        }
    }
}