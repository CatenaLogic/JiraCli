// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;
    using Logging;

    internal class Program
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static int Main(string[] args)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            var consoleLogListener = new OutputLogListener();
            LogManager.AddListener(consoleLogListener);

            try
            {
                HelpWriter.WriteAppHeader(s => Log.Write(LogEvent.Info, s));

                Log.Info("Arguments: {0}", string.Join(" ", args));
                Log.Info(string.Empty);

                var context = ArgumentParser.ParseArguments(args);
                if (context.IsHelp)
                {
                    HelpWriter.WriteHelp(s => Log.Write(LogEvent.Info, s));

                    WaitForKeyPress();

                    return 0;
                }

                // TODO: Implement actual logic 

                //var parameters = new List<Parameter>();
                //var typeFactory = TypeFactory.Default;
                //var initTypes = TypeCache.GetTypes(x => typeof (RuleBase).IsAssignableFromEx(x) && !x.IsAbstract);
                //foreach (var initType in initTypes)
                //{
                //    var initializer = (RuleBase)typeFactory.CreateInstance(initType);
                //    parameters.Add(initializer.GetParameter(context));
                //}

                //foreach (var buildServer in BuildServerList.GetApplicableBuildServers(context))
                //{
                //    buildServer.WriteIntegration(parameters, x => Log.Info(x));
                //}

#if DEBUG
                if (Debugger.IsAttached)
                {
                    WaitForKeyPress();
                }
#endif

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static void WaitForKeyPress()
        {
            Log.Info(string.Empty);
            Log.Info("Press any key to continue");

            Console.ReadKey();
        }
    }
}