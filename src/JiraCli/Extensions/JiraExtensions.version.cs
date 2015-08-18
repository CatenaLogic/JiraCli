// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Collections.Generic;
    using Atlassian.Jira;
    using Catel;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using RestSharp.Serializers;

    public static partial class JiraExtensions
    {
        public static void CreateProjectVersion(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => projectVersion);

            var requestJson = JsonConvert.SerializeObject(projectVersion, GetJsonSettings());

            var resource = "rest/api/2/version";
            var responseJson = jiraRestClient.ExecuteRequestRaw(Method.POST, resource, requestJson);
        }

        public static void UpdateProjectVersion(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => projectVersion);

            var requestJson = JsonConvert.SerializeObject(projectVersion, GetJsonSettings());

            var resource = string.Format("rest/api/2/version/{0}", projectVersion.Id);
            var responseJson = jiraRestClient.ExecuteRequestRaw(Method.PUT, resource, requestJson);
        }

        public static void DeleteProjectVersion(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => projectVersion);

            var resource = string.Format("rest/api/2/version/{0}", projectVersion.Id);
            var responseJson = jiraRestClient.ExecuteRequest(Method.DELETE, resource);
        }

        public static List<JiraProjectVersion> GetProjectVersions(this IJiraRestClient jiraRestClient, string projectKey)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);

            var projectVersions = new List<JiraProjectVersion>();

            var resource = string.Format("rest/api/2/project/{0}/versions", projectKey);
            var responseJson = jiraRestClient.ExecuteRequest(Method.GET, resource);

            foreach (var jsonElement in responseJson.Children())
            {
                var projectVersion = JsonConvert.DeserializeObject<JiraProjectVersion>(jsonElement.ToString());
                projectVersion.Project = projectKey;

                projectVersions.Add(projectVersion);
            }

            return projectVersions;
        }
    }
}