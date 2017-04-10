// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Collections.Generic;
    using Atlassian.Jira;
    using Catel;
    using Models;
    using Newtonsoft.Json;
    using RestSharp;

    public static partial class JiraExtensions
    {
        public static List<JiraIssue> GetIssues(this IJiraRestClient jiraRestClient, string jql, int startAt = 0, int maxResults = 200, string[] fields = null)
        {
            Argument.IsNotNull(() => jiraRestClient);

            var issues = new List<JiraIssue>();

            var searchRequest = new JiraSearchRequest
            {
                Jql = jql,
                StartAt = startAt,
                MaxResults = maxResults,
            };

            if (fields != null)
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
            var responseJson = jiraRestClient.ExecuteRequestRaw(Method.POST, "rest/api/2/search", requestJson);

            SearchResponse response = JsonConvert.DeserializeObject<SearchResponse>(responseJson.ToString());
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