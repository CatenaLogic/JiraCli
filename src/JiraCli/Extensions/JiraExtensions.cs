// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Atlassian.Jira;
    using Atlassian.Jira.Remote;
    using Catel;
    using Newtonsoft.Json;
    using RestSharp;

    public static class JiraExtensions
    {
        private static readonly ConstructorInfo _projectVersionConstructorInfo = typeof (ProjectVersion).GetConstructors(BindingFlags.Instance| BindingFlags.NonPublic | BindingFlags.Public).First();

        public static List<ProjectVersion> GetProjectVersions(this IJiraRestClient jiraRestClient, string projectId)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectId);

            var projectVersions = new List<ProjectVersion>();

            var resource = string.Format("rest/api/2/project/{0}/versions", projectId);
            var json = jiraRestClient.ExecuteRequest(Method.GET, resource);

            foreach (var jsonElement in json.Children())
            {
                var remoteVersion = JsonConvert.DeserializeObject<RemoteVersion>(jsonElement.ToString());

                var projectVersion = (ProjectVersion)_projectVersionConstructorInfo.Invoke(new object[] { remoteVersion });
                projectVersions.Add(projectVersion);
            }

            return projectVersions;
        } 
    }
}