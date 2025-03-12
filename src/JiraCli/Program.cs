namespace JiraCli
{
    using System;
    using System.Diagnostics;
    using Catel.IoC;
    using Catel.Logging;
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

            var exitCode = 0;

            try
            {
                var jiraAutomation = TypeFactory.Default.CreateInstance<JiraAutomation>();
                var task = jiraAutomation.RunAsync(args, WaitForKeyPress);
                task.Wait();

                exitCode = task.Result;
            }
            catch (JiraCliException)
            {
                // Known exception
                exitCode = -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unknown exception occurred");
                exitCode = -1;
            }

#if DEBUG
            if (Debugger.IsAttached)
            {
                WaitForKeyPress();
            }
#endif

            return exitCode;
        }

        private static void WaitForKeyPress()
        {
            Log.Info(string.Empty);
            Log.Info("Press any key to continue");

            Console.ReadKey();
        }
    }
}
