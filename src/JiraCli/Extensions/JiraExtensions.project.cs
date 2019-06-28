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
    using Newtonsoft.Json.Linq;
    using RestSharp;

    public static partial class JiraExtensions
    {
        public static async Task<List<JiraProject>> GetProjectsAsync(this IJiraRestClient jiraRestClient)
        {
            Argument.IsNotNull(() => jiraRestClient);

            var projects = new List<JiraProject>();

            var responseJson = await jiraRestClient.ExecuteRequestAsync(Method.GET, "rest/api/2/project");

            foreach (var jsonElement in responseJson.Children())
            {
                var project = JsonConvert.DeserializeObject<JiraProject>(jsonElement.ToString());

                projects.Add(project);
            }

            return projects;
        }
    }
}
