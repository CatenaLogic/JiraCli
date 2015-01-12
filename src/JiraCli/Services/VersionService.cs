// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using System.Linq;
    using Atlassian.Jira;
    using Atlassian.Jira.Remote;
    using Catel;
    using Catel.Logging;
    using Catel.Services;

    public class VersionService : IVersionService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void CreateVersion(Jira jira, string project, string version)
        {
            Argument.IsNotNull(() => jira);
            Argument.IsNotNullOrWhitespace(() => project);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Creating version {0}", version);

            Log.Debug("Checking if version already exists");

            var existingVersion = GetProjectVersion(jira, project, version);
            if (existingVersion != null)
            {
                Log.Info("Version {0} already exists", version);

                if (existingVersion.IsReleased)
                {
                    string error = string.Format("Version {0} is already released, are you re-releasing an existing version?", version);
                    Log.Error(error);
                    throw new InvalidOperationException(error);
                }

                return;
            }

            Log.Debug("Version does not yet exist, creating version");

            var token = jira.GetToken();
            var jiraService = jira.GetJiraService();

            var nextSequence = 0L;
            var remoteVersions = jiraService.GetVersions(token, project).OrderBy(x => x.name);
            foreach (var remoteVersion in remoteVersions)
            {
                Log.Debug("  {0} => {1}", remoteVersion.name, ObjectToStringHelper.ToString(remoteVersion.sequence));

                if (string.Compare(remoteVersion.name, version, StringComparison.OrdinalIgnoreCase) > 0 && (nextSequence == 0L))
                {
                    nextSequence = remoteVersion.sequence.Value;
                }
            }

            jiraService.AddVersion(token, project, new RemoteVersion
            {
                name = version,
                archived = false,
                sequence = nextSequence
            });

            Log.Info("Created version {0}", version);
        }

        public void ReleaseVersion(Jira jira, string project, string version)
        {
            Argument.IsNotNull(() => jira);
            Argument.IsNotNullOrWhitespace(() => project);
            Argument.IsNotNullOrWhitespace(() => version);

            Log.Info("Releasing version {0}", version);

            var existingVersion = GetProjectVersion(jira, project, version);
            if (existingVersion == null)
            {
                string error = string.Format("Version {0} does not exist, make sure to create it first", version);
                Log.Error(error);
                throw new InvalidOperationException(error);
            }

            if (existingVersion.IsReleased)
            {
                Log.Info("Version was already released on {0}", existingVersion.ReleasedDate);
                return;
            }

            var token = jira.GetToken();
            var jiraService = jira.GetJiraService();

            var remoteVersion = GetVersion(jiraService, token, project, version);

            remoteVersion.releaseDate = DateTime.Now;
            remoteVersion.released = true;

            jiraService.ReleaseVersion(token, project, remoteVersion);

            Log.Info("Released version {0}", version);
        }

        private ProjectVersion GetProjectVersion(Jira jira, string project, string version)
        {
            var existingVersion = (from x in jira.GetProjectVersions(project)
                                   where string.Equals(x.Name, version, StringComparison.OrdinalIgnoreCase)
                                   select x).FirstOrDefault();

            return existingVersion;
        }

        private RemoteVersion GetVersion(IJiraRemoteService jiraService, string token, string project, string version)
        {
            var existingVersion = (from x in jiraService.GetVersions(token, project)
                                   where string.Equals(x.name, version, StringComparison.OrdinalIgnoreCase)
                                   select x).FirstOrDefault();

            return existingVersion;
        }
    }
}