// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraCliException.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;

    public class JiraCliException : Exception
    {
        public JiraCliException(string message)
            : base(message)
        {
        }
    }
}