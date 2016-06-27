// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionInfoServiceFacts.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Tests.Services
{
    using JiraCli.Services;
    using NUnit.Framework;

    [TestFixture]
    public class VersionInfoServiceFacts
    {
        [TestCase("0.1.0", true)]
        [TestCase("1.0.0", true)]
        [TestCase("0.1.0.0", true)]
        [TestCase("1.0.0.0", true)]
        [TestCase("1.0.15.12", true)]
        [TestCase("1.0.15-unstable0016", false)]
        [TestCase("1.0.15-unstable.1", false)]
        [TestCase("1.0.15-beta0016", false)]
        [TestCase("1.0.15-prerelease0016", false)]
        [TestCase("1.0.15.0-unstable0016", false)]
        [TestCase("1.0.15.0-beta0016", false)]
        [TestCase("1.0.15.0-prerelease0016", false)]
        public void TheIsStableVersionMethod(string version, bool expectedValue)
        {
            var versionInfoService = new VersionInfoService();

            var actualValue = versionInfoService.IsReleaseVersion(version);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCase("0.1.0", "1.0.0", VersionComparisonResult.LessThan)]
        [TestCase("0.1.0-unstable.1", "0.1.0", VersionComparisonResult.LessThan)]
        [TestCase("0.1.0-unstable.1", "1.0.0", VersionComparisonResult.LessThan)]
        [TestCase("0.1.0-unstable.1", "0.1.0-unstable.2", VersionComparisonResult.LessThan)]
        [TestCase("0.1.0-unstable.1", "0.1.0-beta.1", VersionComparisonResult.GreaterThan)]
        [TestCase("0.1.0", "0.1.0", VersionComparisonResult.Equal)]
        [TestCase("0.1.0.0", "1.0.0.0", VersionComparisonResult.LessThan)]
        public void TheCompareVersionMethod(string versionToCheck, string compareToVersion, VersionComparisonResult expectedValue)
        {
            var versionInfoService = new VersionInfoService();
            var actualValue = versionInfoService.CompareVersions(versionToCheck, compareToVersion);
            Assert.AreEqual(expectedValue, actualValue);

            if (expectedValue == VersionComparisonResult.Equal)
            {
                return;
            }

            // check the inverse comparison is also true.
            if (expectedValue == VersionComparisonResult.LessThan)
            {
                expectedValue = VersionComparisonResult.GreaterThan;
            }
            else if (expectedValue == VersionComparisonResult.GreaterThan)
            {
                expectedValue = VersionComparisonResult.LessThan;
            }

            actualValue = versionInfoService.CompareVersions(compareToVersion, versionToCheck);
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}