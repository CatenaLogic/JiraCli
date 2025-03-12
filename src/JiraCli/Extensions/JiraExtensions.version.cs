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
            ArgumentNullException.ThrowIfNull(jiraRestClient);
            ArgumentNullException.ThrowIfNull(projectVersion);

            var requestJson = JsonConvert.SerializeObject(projectVersion, GetJsonSettings());

            var resource = "rest/api/2/version";
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.POST, resource, requestJson);
        }


        public static async Task UpdateIssueAsync(this IJiraRestClient jiraRestClient, string issueNumber, JiraIssueUpdate updateIssue)
        {
            ArgumentNullException.ThrowIfNull(jiraRestClient);
            ArgumentNullException.ThrowIfNull(updateIssue);

            var requestJson = JsonConvert.SerializeObject(updateIssue, GetJsonSettings());

            var resource = $"rest/api/2/issue/{issueNumber}";
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.PUT, resource, requestJson);
        }

        public static async Task UpdateProjectVersionAsync(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion)
        {
            ArgumentNullException.ThrowIfNull(jiraRestClient);
            ArgumentNullException.ThrowIfNull(projectVersion);

            var requestJson = JsonConvert.SerializeObject(projectVersion, GetJsonSettings());

            var resource = string.Format("rest/api/2/version/{0}", projectVersion.Id);
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.PUT, resource, requestJson);
        }

        public static async Task DeleteProjectVersionAsync(this IJiraRestClient jiraRestClient, JiraProjectVersion projectVersion, JiraProjectVersion projectToMoveFixIssuesTo = null,
            JiraProjectVersion projectToMoveAffectedIssuesTo = null)
        {
            ArgumentNullException.ThrowIfNull(jiraRestClient);
            ArgumentNullException.ThrowIfNull(projectVersion);

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
            ArgumentNullException.ThrowIfNull(jiraRestClient);
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
