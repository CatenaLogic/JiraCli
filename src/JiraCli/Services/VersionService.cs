// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atlassian.Jira;
    using Atlassian.Jira.Remote;
    using Catel;
    using Catel.Logging;
    using Models;
    using RestSharp;

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

        public void CreateVersion(IJiraRestClient jiraRestClient, string projectKey, string version)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Creating version '{0}'", version);

            Log.Debug("Checking if version already exists");

            var existingVersion = GetProjectVersion(jiraRestClient, projectKey, version);
            if (existingVersion != null)
            {
                Log.Info("Version '{0}' already exists", version);

                if (existingVersion.Released)
                {
                    var error = string.Format("Version '{0}' is already released, are you re-releasing an existing version?", version);
                    Log.Error(error);
                    throw new InvalidOperationException(error);
                }

                return;
            }

            Log.Debug("Version does not yet exist, creating version");

            var project = GetProject(jiraRestClient, projectKey);
            if (project == null)
            {
                var error = string.Format("Project '{0}' cannot be found or current user does not have access to the project", projectKey);
                Log.Error(error);
                throw new InvalidOperationException(error);
            }

            jiraRestClient.CreateProjectVersion(new JiraProjectVersion
            {
                Project = project.Key,
                Name = version,
            });

            Log.Info("Created version '{0}'", version);
        }

        public void ReleaseVersion(IJiraRestClient jiraRestClient, string projectKey, string version)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Releasing version '{0}'", version);

            var projectVersion = GetProjectVersion(jiraRestClient, projectKey, version);
            if (projectVersion == null)
            {
                var error = string.Format("Version {0} does not exist, make sure to create it first", version);
                Log.Error(error);
                throw new InvalidOperationException(error);
            }

            if (projectVersion.Released)
            {
                Log.Info("Version was already released on {0}", projectVersion.ReleaseDate);
                return;
            }

            projectVersion.ReleaseDate = DateTime.Now;
            projectVersion.Released = true;

            jiraRestClient.UpdateProjectVersion(projectVersion);

            Log.Info("Released version '{0}'", version);
        }

        public void MergeVersions(IJiraRestClient jiraRestClient, string projectKey, string version)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => projectKey);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Merging all prerelease versions into '{0}'", version);

            if (!_versionInfoService.IsStableVersion(version))
            {
                Log.Info("Version '{0}' is not a stable version, versions will not be merged", version);
                return;
            }

            var allVersions = jiraRestClient.GetProjectVersions(projectKey);

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

                jiraRestClient.DeleteProjectVersion(versionToMerge, newVersion, newVersion);
            }

            Log.Info("Merged all prerelease versions into '{0}'", version);
        }

        private JiraProject GetProject(IJiraRestClient jiraRestClient, string projectKey)
        {
            var existingProject = (from x in jiraRestClient.GetProjects()
                                   where string.Equals(x.Key, projectKey, StringComparison.OrdinalIgnoreCase)
                                   select x).FirstOrDefault();

            return existingProject;
        }

        private JiraProjectVersion GetProjectVersion(IJiraRestClient jiraRestClient, string project, string version)
        {
            var existingVersion = (from x in jiraRestClient.GetProjectVersions(project)
                                   where string.Equals(x.Name, version, StringComparison.OrdinalIgnoreCase)
                                   select x).FirstOrDefault();

            return existingVersion;
        }
    }
}