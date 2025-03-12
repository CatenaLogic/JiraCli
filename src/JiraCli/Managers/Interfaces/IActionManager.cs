namespace JiraCli
{
    using System.Collections.Generic;

    public interface IActionManager
    {
        IEnumerable<IAction> Actions { get; }
        IAction GetAction(string name);
    }
}