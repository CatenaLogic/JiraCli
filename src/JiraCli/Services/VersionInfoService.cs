// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionInfoService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using Catel;
    using Semver;

    public class VersionInfoService : IVersionInfoService
    {
        public bool IsReleaseVersion(string version)
        {
            Argument.IsNotNull(() => version);

            // Attempt semver format.
            SemVersion semVer;
            if (SemVersion.TryParse(version, out semVer))
            {
                if (string.IsNullOrWhiteSpace(semVer.Prerelease))
                {
                    // it's not a pre-release.
                    return true;
                }
            }
            else
            {
                // the version isn't compatible with semver.
                // Assume that if we can parse it, it's a release version
                Version ver;
                if (Version.TryParse(version, out ver))
                {
                    return true;
                }
            }

            return false;
        }

        public VersionComparisonResult CompareVersions(string versionToCheck, string versionToCompareAgainst)
        {

            Argument.IsNotNull(() => versionToCheck);
            Argument.IsNotNull(() => versionToCompareAgainst);

            SemVersion semanticVersionToCheck;
            bool hasParsedVersionToCheck = SemVersion.TryParse(versionToCheck, out semanticVersionToCheck);

            SemVersion semanticVersionToCompareAgainst;
            bool hasParsedVersionToCompareAgainst = SemVersion.TryParse(versionToCompareAgainst, out semanticVersionToCompareAgainst);

            // either both have to be semver or both have to be ordinary to compare.
            if (hasParsedVersionToCheck && hasParsedVersionToCompareAgainst)
            {
                int result = semanticVersionToCheck.CompareByPrecedence(versionToCompareAgainst);
                return ConvertToComparisonResult(result);
            }

            // either one or both version numbers are not sem ver compatible. fallback to attempting non semver version comparison.

            Version verToCheck;
            hasParsedVersionToCheck = Version.TryParse(versionToCheck, out verToCheck);

            Version verToCompareAgainst;
            hasParsedVersionToCompareAgainst = Version.TryParse(versionToCompareAgainst, out verToCompareAgainst);

            if (hasParsedVersionToCheck && hasParsedVersionToCompareAgainst)
            {
                int result = verToCheck.CompareTo(verToCompareAgainst);
                return ConvertToComparisonResult(result);
            }

            // attempting to compare semver with non semver, or unable to parse version numbers.
            return VersionComparisonResult.Unknown;

        }

        private VersionComparisonResult ConvertToComparisonResult(int result)
        {
            if (result < 0)
            {
                return VersionComparisonResult.LessThan;
            }

            if (result > 0)
            {
                return VersionComparisonResult.GreaterThan;
            }

            return VersionComparisonResult.Equal;

        }

        public bool IsPreRelease(string version, Predicate<string> prereleaseLabelChecker)
        {
            Argument.IsNotNull(() => version);

            // Can assume semver format.
            SemVersion semVer;
            if (SemVersion.TryParse(version, out semVer))
            {
                if (!string.IsNullOrWhiteSpace(semVer.Prerelease))
                {
                    return prereleaseLabelChecker(semVer.Prerelease);
                }
            }

            return false;
        }

        public bool IsPreRelease(string version)
        {
            Argument.IsNotNull(() => version);            

            // Can assume semver format.
            SemVersion semVer;
            if (SemVersion.TryParse(version, out semVer))
            {
                if (!string.IsNullOrWhiteSpace(semVer.Prerelease))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsPreReleaseWithLabelPrefix(string version, string labelPrefix)
        {
            Argument.IsNotNull(() => version);
            Argument.IsNotNull(() => labelPrefix);

            // Can assume semver format.
            SemVersion semVer;
            if (SemVersion.TryParse(version, out semVer))
            {
                if (!string.IsNullOrWhiteSpace(semVer.Prerelease))
                {
                    if (semVer.Prerelease.ToLowerInvariant().StartsWith(labelPrefix.ToLowerInvariant()))
                    {
                        // it's a pre-release and the label contains the specified prefix.
                        return true;
                    }
                }
            }
           
            return false;
        }
    }
}