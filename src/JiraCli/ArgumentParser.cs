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
    using System.Text.RegularExpressions;
    using Catel.Logging;

    public static class ArgumentParser
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static Context ParseArguments(string commandLineArguments)
        {
            var args = SplitCommandLine(commandLineArguments).ToArray();
            return ParseArguments(args);
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
                throw Log.ErrorAndCreateException<JiraCliException>("Invalid number of arguments");
            }

            var firstArgument = commandLineArguments.First();
            if (IsHelp(firstArgument))
            {
                context.IsHelp = true;
                return context;
            }

            if (commandLineArguments.Count < 2)
            {
                throw Log.ErrorAndCreateException<JiraCliException>("Invalid number of arguments");
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
                    context.Issues = SplitCsv(value);
                    continue;
                }

                //if (IsSwitch("ci", name))
                //{
                //    bool isCi;
                //    bool.TryParse(value, out isCi);
                //    context.IsCi = isCi;
                //    continue;
                //}

                throw Log.ErrorAndCreateException<JiraCliException>("Could not parse command line parameter '{0}'.", name);
            }

            return context;
        }

        private static string[] SplitCsv(string value)
        {
            var splitAndTrimmed = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();
            return splitAndTrimmed;
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
                throw Log.ErrorAndCreateException<JiraCliException>("Could not parse arguments: '{0}'.", string.Join(" ", commandLineArguments));
            }
        }

        private static bool IsHelp(string singleArgument)
        {
            return (singleArgument == "?") ||
                   IsSwitch("h", singleArgument) ||
                   IsSwitch("help", singleArgument) ||
                   IsSwitch("?", singleArgument);
        }

        /// <summary>
        /// Shamelessly taken from stackoverflow: http://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp/298990#298990
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            bool inQuotes = false;

            return Split(commandLine, c =>
            {
                if (c == '\"')
                    inQuotes = !inQuotes;

                return !inQuotes && c == ' ';
            })
            .Select(arg => TrimMatchingQuotes(arg.Trim(), '\"'))
            .Where(arg => !string.IsNullOrEmpty(arg));
        }

        private static IEnumerable<string> Split(string inputString, Predicate<char> shouldSplit)
        {
            int nextPiece = 0;

            for (int c = 0; c < inputString.Length; c++)
            {
                if (shouldSplit(inputString[c]))
                {
                    yield return inputString.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return inputString.Substring(nextPiece);
        }

        private static string TrimMatchingQuotes(string inputString, char quote)
        {
            if ((inputString.Length >= 2) &&
                (inputString[0] == quote) && (inputString[inputString.Length - 1] == quote))
                return inputString.Substring(1, inputString.Length - 2);

            return inputString;
        }
    }
}
