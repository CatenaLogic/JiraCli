// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpWriter.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Text;
    using Catel;
    using Catel.Reflection;
    using Catel.Text;

    public class HelpWriter : IHelpWriter
    {
        private readonly IActionManager _actionManager;

        public HelpWriter(IActionManager actionManager)
        {
            Argument.IsNotNull(() => actionManager);

            _actionManager = actionManager;
        }

        public void WriteAppHeader(Action<string> writer)
        {
            var assembly = typeof (HelpWriter).Assembly;

            writer(string.Format("{0} v{1}", assembly.Title(), assembly.Version()));
            writer("================");
            writer(string.Empty);
        }

        public void WriteHelp(Action<string> writer)
        {
            var stringBuilder = new StringBuilder(@"Automate JIRA via the command line interface (CLI). The following parameters are always required:

    JiraCli -user [user] -pw [password] -url [url] -action [action]

The [action] can be replaced by any of the following actions:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            foreach (var action in _actionManager.Actions)
            {
                stringBuilder.AppendLine("    {0} {1}", action.Name, action.ArgumentsUsage);
            }

            writer(stringBuilder.ToString());
        }
    }
}