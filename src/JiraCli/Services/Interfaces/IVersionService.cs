// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System.Threading.Tasks;
    using Atlassian.Jira.Remote;

    public interface IVersionService
    {
        Task CreateVersionAsync(IJiraRestClient jiraRestClient, string projectKey, string version);
        Task ReleaseVersionAsync(IJiraRestClient jiraRestClient, string projectKey, string version);
        Task MergeVersionsAsync(IJiraRestClient jiraRestClient, string projectKey, string version);
        Task<string[]> AssignVersionToIssuesAsync(IJiraRestClient jiraRestClient, string projectKey, string version, string[] issues);
    }
}
