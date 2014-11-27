// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHelpWriter.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;

    public interface IHelpWriter
    {
        void WriteAppHeader(Action<string> writer);
        void WriteHelp(Action<string> writer);
    }
}