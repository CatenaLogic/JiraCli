// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAction.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Threading.Tasks;

    public interface IAction
    {
        string Name { get; set; }

        string Description { get; set; }

        string ArgumentsUsage { get; set; }

        void Validate(Context context);

        Task<bool> Execute(Context context);
    }
}