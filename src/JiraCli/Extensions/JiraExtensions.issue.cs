namespace JiraCli
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Atlassian.Jira.Remote;
    using Catel;
    using Models;
    using Newtonsoft.Json;
    using RestSharp;

    public static partial class JiraExtensions
    {
        public static async Task<List<JiraIssue>> GetIssuesAsync(this IJiraRestClient jiraRestClient, string jql, int startAt = 0, int maxResults = 200, string[] fields = null)
        {
            ArgumentNullException.ThrowIfNull(jiraRestClient);

            var issues = new List<JiraIssue>();

            var searchRequest = new JiraSearchRequest
            {
                Jql = jql,
                StartAt = startAt,
                MaxResults = maxResults,
            };

            if (fields is not null)
            {
                searchRequest.Fields.AddRange(fields);
            }
            else
            {
                searchRequest.Fields.Add("id");
                searchRequest.Fields.Add("key");
                searchRequest.Fields.Add("parent");
                searchRequest.Fields.Add("status");
                searchRequest.Fields.Add("issuetype");
            }

            var requestJson = JsonConvert.SerializeObject(searchRequest, GetJsonSettings());
            var responseJson = await jiraRestClient.ExecuteRequestRawAsync(Method.POST, "rest/api/2/search", requestJson);

            var response = JsonConvert.DeserializeObject<SearchResponse>(responseJson.ToString());
            return response.IssueDescriptions;
            //foreach (var jsonElement in responseJson.Children())
            //{
            //    var issue = JsonConvert.DeserializeObject<JiraIssue>(jsonElement.ToString());

            //    issues.Add(issue);
            //}

           // return issues;
        }
    }
}
