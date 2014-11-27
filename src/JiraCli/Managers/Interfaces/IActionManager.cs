// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActionManager.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Collections.Generic;

    public interface IActionManager
    {
        IEnumerable<IAction> Actions { get; }
        IAction GetAction(string name);
    }
}