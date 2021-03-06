﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Atlassian.Jira;
    using Atlassian.Jira.Remote;
    using Catel;
    using Catel.Logging;
    using Models;

    public class VersionService : IVersionService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IMergeVersionService _mergeVersionService;
        private readonly IVersionInfoService _versionInfoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionService" /> class.
        /// </summary>
        /// <param name="mergeVersionService">The merge version service.</param>
        /// <param name="versionInfoService">The version information service.</param>
        public VersionService(IMergeVersionService mergeVersionService, IVersionInfoService versionInfoService)
        {
            Argument.IsNotNull(() => mergeVersionService);
            Argument.IsNotNull(() => versionInfoService);

            _mergeVersionService = mergeVersionService;
            _versionInfoService = versionInfoService;
        }

        public async Task<string[]> AssignVersionToIssuesAsync(IJiraRestClient jiraRestClient, string projectKey, string version, string[] issues)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);
            Argument.IsNotNullOrEmptyArray(() => issues);

            Log.Debug("Checking if version already exists");

            var existingVersion = GetProjectVersionAsync(jiraRestClient, projectKey, version);
            if (existingVersion == null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Version '{version}' does not exist / ensure you have created this version first");
            }

            Log.Info($"Version '{version}' exists, going to set this as FixVersion for '{issues.Length}' issues");

            // update issues
            var issueUpdate = new JiraIssueUpdate();
            issueUpdate.AddFieldValue("fixVersions", new { name = version });

            var issuesIds = string.Join(",", issues);
            var jql = "project = " + projectKey + " AND key in (" + issuesIds + ") ORDER BY priority DESC, updated DESC";

            var assignedVersions = new List<string>();

            var retrievedIssues = await jiraRestClient.GetIssuesAsync(jql);
            foreach (var item in retrievedIssues)
            {
                Log.Info("Updating issue '{0}'", item.Key);
                // if the item has a parent (i.e like a subtask)
                if (item.Fields != null)
                {
                    if (item.Fields.IssueType != null)
                    {
                        if (item.Fields.IssueType.SubTask)
                        {
                            if (item.Fields.Parent == null)
                            {
                                Log.Error("Could not retrieve parent task for subtask: {0}", item.Key);
                                continue;          // do not attempt to apply to subtask.             
                            }

                            // get parent and assign that instead.
                            Log.Info("Issue '{0}' is a subtask. Will assign version to the parent task: '{1}' instead.", item.Key, item.Fields.Parent.ProxyKey);
                            await jiraRestClient.UpdateIssueAsync(item.Fields.Parent.ProxyKey, issueUpdate);
                            assignedVersions.Add(item.Fields.Parent.ProxyKey);
                            continue;
                        }

                        // queries cannot be assigned to versions.
                        if (item.Fields.IssueType.Name.ToLowerInvariant() == "query")
                        {
                            Log.Info("Issue '{0}' is a query. It will be skipped as we cannot assing queries to a release.", item.Key);
                            continue;
                        }
                    }
                }

                await jiraRestClient.UpdateIssueAsync(item.ProxyKey, issueUpdate);
                assignedVersions.Add(item.ProxyKey);
                Log.Info("Issue updated.");

                //if (assignParentOfSubtasks)
                //{
                //  //  var issuesIds = string.Join(",", issues);


                //}

            }

            return assignedVersions.ToArray();

        }

        public async Task CreateVersionAsync(IJiraRestClient jiraRestClient, string projectKey, string version)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Creating version '{0}'", version);

            Log.Debug("Checking if version already exists");

            var existingVersion = await GetProjectVersionAsync(jiraRestClient, projectKey, version);
            if (existingVersion != null)
            {
                Log.Info("Version '{0}' already exists", version);

                if (existingVersion.Released)
                {
                    throw Log.ErrorAndCreateException<InvalidOperationException>($"Version '{version}' is already released, are you re-releasing an existing version?");
                }

                return;
            }

            Log.Debug("Version does not yet exist, creating version");

            var project = await GetProjectAsync(jiraRestClient, projectKey);
            if (project == null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Project '{projectKey}' cannot be found or current user does not have access to the project");
            }

            await jiraRestClient.CreateProjectVersionAsync(new JiraProjectVersion
            {
                Project = project.Key,
                Name = version,
            });

            Log.Info("Created version '{0}'", version);
        }

        public async Task ReleaseVersionAsync(IJiraRestClient jiraRestClient, string projectKey, string version)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Releasing version '{0}'", version);

            var projectVersion = await GetProjectVersionAsync(jiraRestClient, projectKey, version);
            if (projectVersion == null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Version '{version}' does not exist / ensure you have created this version first");
            }

            if (projectVersion.Released)
            {
                Log.Info("Version was already released on {0}", projectVersion.ReleaseDate);
                return;
            }

            projectVersion.UserReleaseDate = null;
            projectVersion.ReleaseDate = DateTime.Now;

            //if (string.IsNullOrWhiteSpace(projectVersion.UserReleaseDate))
            //{

            //}
            //else
            //{

            //}

            projectVersion.Released = true;

            await jiraRestClient.UpdateProjectVersionAsync(projectVersion);

            Log.Info("Released version '{0}'", version);
        }

        public async Task MergeVersionsAsync(IJiraRestClient jiraRestClient, string projectKey, string version)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Merging all prerelease versions into '{0}'", version);

            if (!_versionInfoService.IsReleaseVersion(version))
            {
                Log.Info("Version '{0}' is not a stable version, versions will not be merged", version);
                return;
            }

            var allVersions = await jiraRestClient.GetProjectVersionsAsync(projectKey);

            var newVersion = allVersions.First(x => string.Equals(x.Name, version));
            var versionsToMerge = new List<JiraProjectVersion>();

            foreach (var remoteVersion in allVersions)
            {
                if (_mergeVersionService.ShouldBeMerged(version, remoteVersion.Name))
                {
                    versionsToMerge.Add(remoteVersion);
                }
            }

            foreach (var versionToMerge in versionsToMerge)
            {
                if (string.Equals(versionToMerge.Id, newVersion.Id))
                {
                    continue;
                }

                Log.Debug("Deleting version '{0}' and moving issues to '{1}'", versionToMerge, newVersion);

                await jiraRestClient.DeleteProjectVersionAsync(versionToMerge, newVersion, newVersion);
            }

            Log.Info("Merged all prerelease versions into '{0}'", version);
        }

        private async Task<JiraProject> GetProjectAsync(IJiraRestClient jiraRestClient, string projectKey)
        {
            var existingProject = (from x in await jiraRestClient.GetProjectsAsync()
                                   where string.Equals(x.Key, projectKey, StringComparison.OrdinalIgnoreCase)
                                   select x).FirstOrDefault();

            return existingProject;
        }

        private async Task<JiraProjectVersion> GetProjectVersionAsync(IJiraRestClient jiraRestClient, string project, string version)
        {
            var existingVersion = (from x in await jiraRestClient.GetProjectVersionsAsync(project)
                                   where string.Equals(x.Name, version, StringComparison.OrdinalIgnoreCase)
                                   select x).FirstOrDefault();

            return existingVersion;
        }


    }
}
