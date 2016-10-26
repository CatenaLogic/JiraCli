// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVersionInfoService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;

namespace JiraCli.Services
{
    public interface IVersionInfoService
    {
        bool IsPreRelease(string version, Predicate<string> labelChecker);
        bool IsPreRelease(string version);
        bool IsPreReleaseWithLabelPrefix(string version, string labelPrefix);
        bool IsReleaseVersion(string version);
        VersionComparisonResult CompareVersions(string versionToCheck, string versionBeingReleased);

    }
}