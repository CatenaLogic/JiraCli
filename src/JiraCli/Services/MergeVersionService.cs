// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using Catel;
    using Catel.Logging;

    public class MergeVersionService : IMergeVersionService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IVersionInfoService _versionInfoService;

        public MergeVersionService(IVersionInfoService versionInfoService)
        {
            Argument.IsNotNull(() => versionInfoService);

            _versionInfoService = versionInfoService;
        }

        public bool ShouldBeMerged(string versionBeingReleased, string versionToCheck)
        {
            Argument.IsNotNull(() => versionBeingReleased);
            Argument.IsNotNull(() => versionToCheck);

            // Only non pre-release versions can be merged in to.
            if (!_versionInfoService.IsReleaseVersion(versionBeingReleased))
            {
                return false;
            }

            // Only pre-release versions with an "unstable" label prefix are valid to merge.
            // semVer.Prerelease.ToLowerInvariant().StartsWith(labelPrefix.ToLowerInvariant())

            if (!_versionInfoService.IsPreRelease(versionToCheck, (label) =>
            {
                return label.ToLowerInvariant().StartsWith("unstable") ||
                       label.ToLowerInvariant().StartsWith("alpha");
            }))
            {
                return false;
            }

            // version must be less than the version being released.
            var versionComparison = _versionInfoService.CompareVersions(versionToCheck, versionBeingReleased);
            switch (versionComparison)
            {
                case VersionComparisonResult.LessThan:
                    return true;

                case VersionComparisonResult.Unknown:
                    Log.Warning("Unable to compare version '{0}' with version '{1}'. Version will not be included in the Merge.", versionToCheck, versionBeingReleased);
                    return false;

                default:
                    return false;
            }

        }
    }
}