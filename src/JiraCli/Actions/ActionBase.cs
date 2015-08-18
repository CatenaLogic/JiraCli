// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionBase.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Atlassian.Jira;
    using Atlassian.Jira.Remote;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;

    public abstract class ActionBase : IAction
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public string Name { get; set; }

        public string Description { get; set; }

        public string ArgumentsUsage { get; set; }

        public void Validate(Context context)
        {
            context.ValidateContext();

            ValidateContext(context);
        }

        protected abstract void ValidateContext(Context context);

        public async Task<bool> Execute(Context context)
        {
            Argument.IsNotNull(() => context);

            try
            {
                await Task.Factory.StartNew(() => ExecuteWithContext(context));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        protected abstract void ExecuteWithContext(Context context);

        protected IJiraRestClient CreateJira(Context context)
        {
            Argument.IsNotNull(() => context);

            var type = TypeCache.GetType("Atlassian.Jira.Remote.JiraRestServiceClient");
            var constructor = type.GetConstructorsEx().First();

            var jiraRestClient = (IJiraRestClient) constructor.Invoke(new object[]
            {
                context.JiraUrl,
                context.UserName,
                context.Password,
                new JiraRestClientSettings
                {
                    EnableRequestTrace = Debugger.IsAttached
                } 
            });

            return jiraRestClient;
        }
    }
}