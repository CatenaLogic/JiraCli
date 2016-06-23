// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVersionInfoService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    public interface IVersionInfoService
    {
        bool IsPreReleaseWithLabelPrefix(string version, string labelPrefix);
        bool IsReleaseVersion(string version);
        VersionComparisonResult CompareVersions(string versionToCheck, string versionBeingReleased);

    }
}