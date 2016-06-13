// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentParser.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Catel.Logging;

    public static class ArgumentParser
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static Context ParseArguments(string commandLineArguments)
        {
            return ParseArguments(commandLineArguments.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        public static Context ParseArguments(params string[] commandLineArguments)
        {
            return ParseArguments(commandLineArguments.ToList());
        }

        public static Context ParseArguments(List<string> commandLineArguments)
        {
            var context = new Context();

            if (commandLineArguments.Count == 0)
            {
                Log.ErrorAndThrowException<JiraCliException>("Invalid number of arguments");
            }

            var firstArgument = commandLineArguments.First();
            if (IsHelp(firstArgument))
            {
                context.IsHelp = true;
                return context;
            }

            if (commandLineArguments.Count < 2)
            {
                Log.ErrorAndThrowException<JiraCliException>("Invalid number of arguments");
            }

            var namedArguments = commandLineArguments.ToList();

            EnsureArgumentsEvenCount(commandLineArguments, namedArguments);

            for (var index = 0; index < namedArguments.Count; index = index + 2)
            {
                var name = namedArguments[index];
                var value = namedArguments[index + 1];

                if (IsSwitch("l", name))
                {
                    context.LogFile = value;
                    continue;
                }

                if (IsSwitch("user", name))
                {
                    context.UserName = value;
                    continue;
                }

                if (IsSwitch("pw", name))
                {
                    context.Password = value;
                    continue;
                }

                if (IsSwitch("url", name))
                {
                    context.JiraUrl = value;
                    continue;
                }

                if (IsSwitch("action", name))
                {
                    context.Action = value;
                    continue;
                }

                if (IsSwitch("project", name))
                {
                    context.Project = value;
                    continue;
                }

                if (IsSwitch("version", name))
                {
                    context.Version = value;
                    continue;
                }

                if (IsSwitch("merge", name))
                {
                    bool mergeVersions;
                    bool.TryParse(value, out mergeVersions);
                    context.MergeVersions = mergeVersions;
                    continue;
                }

                if (IsSwitch("issues", name))
                {
                    context.Issues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    continue;
                }

                //if (IsSwitch("ci", name))
                //{
                //    bool isCi;
                //    bool.TryParse(value, out isCi);
                //    context.IsCi = isCi;
                //    continue;
                //}

                Log.ErrorAndThrowException<JiraCliException>("Could not parse command line parameter '{0}'.", name);
            }

            return context;
        }

        private static bool IsSwitch(string switchName, string value)
        {
            if (value.StartsWith("-"))
            {
                value = value.Remove(0, 1);
            }

            if (value.StartsWith("/"))
            {
                value = value.Remove(0, 1);
            }

            return (string.Equals(switchName, value));
        }

        private static void EnsureArgumentsEvenCount(IEnumerable<string> commandLineArguments, List<string> namedArguments)
        {
            if (namedArguments.Count.IsOdd())
            {
                Log.ErrorAndThrowException<JiraCliException>("Could not parse arguments: '{0}'.", string.Join(" ", commandLineArguments));
            }
        }

        private static bool IsHelp(string singleArgument)
        {
            return (singleArgument == "?") ||
                   IsSwitch("h", singleArgument) ||
                   IsSwitch("help", singleArgument) ||
                   IsSwitch("?", singleArgument);
        }
    }
}