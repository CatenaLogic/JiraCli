// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraAutomation.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public class JiraAutomation
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IHelpWriter _helpWriter;
        private readonly IActionManager _actionManager;

        public JiraAutomation(IHelpWriter helpWriter, IActionManager actionManager)
        {
            Argument.IsNotNull(() => helpWriter);
            Argument.IsNotNull(() => actionManager);

            _helpWriter = helpWriter;
            _actionManager = actionManager;
        }

        public async Task<int> Run(string[] arguments, Action waitForInput)
        {
            _helpWriter.WriteAppHeader(s => Log.Write(LogEvent.Info, s));

            // NOTE: for now don't expose, it might contain the password
            //Log.Info("Arguments: {0}", string.Join(" ", arguments));
            //Log.Info(string.Empty);

            var context = ArgumentParser.ParseArguments(arguments);
            if (context.IsHelp)
            {
                _helpWriter.WriteHelp(s => Log.Write(LogEvent.Info, s));

                waitForInput();

                return 0;
            }

            var action = _actionManager.GetAction(context.Action);
            if (action == null)
            {
                Log.ErrorAndThrowException<JiraCliException>("Action '{0}' does not exist, make sure to specify the right action name", context.Action);
            }

            Log.Debug("Validating action");

            action.Validate(context);

            Log.Info("Running action '{0}'", action.Name);

            await action.Execute(context);

            return 0;
        }
    }
}