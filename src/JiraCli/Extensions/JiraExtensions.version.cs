// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Atlassian.Jira.Remote;
    using Catel;
    using Models;
    using Newtonsoft.Json;
    using RestSharp;

    public static partial class JiraExtensions
    {
        public static async Task CreateProjectVersionAsync(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => projectVersion);

            var requestJson = JsonConvert.SerializeObject(projectVersion, GetJsonSettings());

            var resource = "rest/api/2/version";
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.POST, resource, requestJson);
        }


        public static async Task UpdateIssueAsync(this IJiraRestClient jiraRestClient, string issueNumber, JiraIssueUpdate updateIssue)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => updateIssue);

            var requestJson = JsonConvert.SerializeObject(updateIssue, GetJsonSettings());

            var resource = $"rest/api/2/issue/{issueNumber}";
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.PUT, resource, requestJson);
        }

        public static async Task UpdateProjectVersionAsync(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => projectVersion);

            var requestJson = JsonConvert.SerializeObject(projectVersion, GetJsonSettings());

            var resource = string.Format("rest/api/2/version/{0}", projectVersion.Id);
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.PUT, resource, requestJson);
        }

        public static async Task DeleteProjectVersionAsync(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion, JiraProjectVersion projectToMoveFixIssuesTo = null,
            JiraProjectVersion projectToMoveAffectedIssuesTo = null)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNull(() => projectVersion);

            var resource = string.Format("rest/api/2/version/{0}", projectVersion.Id);

            if (projectToMoveFixIssuesTo != null)
            {
                resource += resource.Contains("?") ? "&" : "?";
                resource += string.Format("moveFixIssuesTo={0}", projectToMoveFixIssuesTo.Id);
            }

            if (projectToMoveAffectedIssuesTo != null)
            {
                resource += resource.Contains("?") ? "&" : "?";
                resource += string.Format("moveAffectedIssuesTo={0}", projectToMoveAffectedIssuesTo.Id);
            }

            var responseJson = await jiraRestClient.ExecuteRequestAsync(Method.DELETE, resource);
        }

        public static async Task<List<JiraProjectVersion>> GetProjectVersionsAsync(this IJiraRestClient jiraRestClient, string projectKey)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);

            var projectVersions = new List<JiraProjectVersion>();

            var resource = string.Format("rest/api/2/project/{0}/versions", projectKey);
            var responseJson = await jiraRestClient.ExecuteRequestAsync(Method.GET, resource);          

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
