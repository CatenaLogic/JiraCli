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
        [TestCase("1.0.15-beta0016", false)]
        [TestCase("1.0.15-prerelease0016", false)]
        [TestCase("1.0.15.0-unstable0016", false)]
        [TestCase("1.0.15.0-beta0016", false)]
        [TestCase("1.0.15.0-prerelease0016", false)]
        public void TheIsStableVersionMethod(string version, bool expectedValue)
        {
            var versionInfoService = new VersionInfoService();

            var actualValue = versionInfoService.IsStableVersion(version);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}