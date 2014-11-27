// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using Atlassian.Jira;

    public interface IVersionService
    {
        void CreateVersion(Jira jira, string project, string version);
        void ReleaseVersion(Jira jira, string project, string version);
    }
}