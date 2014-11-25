// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpWriter.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using Catel.Reflection;

    public static class HelpWriter
    {
        public static void WriteAppHeader(Action<string> writer)
        {
            var assembly = typeof (HelpWriter).Assembly;

            writer(string.Format("{0} v{1}", assembly.Title(), assembly.Version()));
            writer("=====================");
            writer(string.Empty);
        }

        public static void WriteHelp(Action<string> writer)
        {
            var message =
                @"Initializes several variables in Continua CI to create a single point of responsibility in all configurations.

ContinuaInit -b [branchName] -v [version]

    -b [branchName]      Name of the branch to use on the remote repository.
    -v [version]         The version that is currently being built, best determined with GitVersion.
    -ci [true|false]     true if this is a ci build, otherwise false. The default value is false.
    -l [file]        
";
            writer(message);
        }
    }
}