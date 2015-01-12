// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Reflection;
    using Atlassian.Jira;
    using Atlassian.Jira.Remote;
    using Catel;
    using Catel.Reflection;

    public static class JiraExtensions
    {
        public static string Authenticate(this JiraSoapServiceClient client, Context context)
        {
            Argument.IsNotNull(() => client);
            Argument.IsNotNull(() => context);

            return client.login(context.UserName, context.Password);
        }

        public static string GetToken(this Jira jira)
        {
            Argument.IsNotNull("jira", jira);

            var field = jira.GetType().GetField("_token", BindingFlags.Instance | BindingFlags.NonPublic);
            return (string) field.GetValue(jira);
        }

        public static IJiraRemoteService GetJiraService(this Jira jira)
        {
            Argument.IsNotNull("jira", jira);

            return PropertyHelper.GetPropertyValue<IJiraRemoteService>(jira, "RemoteService");
        }
    }
}