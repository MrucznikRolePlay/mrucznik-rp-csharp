using SampSharp.GameMode.Display;

namespace Mrucznik.Systems
{
    public interface IDialogFlow
    {
        void Start();
        void Next();
        void Previous();

        void AddDialog(Dialog dialog);
    }
}