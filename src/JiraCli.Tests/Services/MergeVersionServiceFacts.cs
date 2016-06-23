// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeVersionServiceFacts.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Tests.Services
{
    using JiraCli.Services;
    using NUnit.Framework;

    [TestFixture]
    public class MergeVersionServiceFacts
    {
        [TestCase("1.0.0", "1.1.0", false)]
        [TestCase("1.0.0-unstable0016", "1.1.0", false)]
        [TestCase("1.0.0", "1.1.0-unstable0016", false)]
        [TestCase("1.0.0", "1.0.0-beta0016", false)]
        [TestCase("1.0.0", "1.0.0-unstable0016", true)]
        [TestCase("1.0.0", "0.0.1-unstable0016", true)]
        public void TheShouldBeMergedMethod(string versionBeingReleased, string versionToCheck, bool expectedValue)
        {
            var mergeVersionService = new MergeVersionService(new VersionInfoService());

            var shouldBeMerged = mergeVersionService.ShouldBeMerged(versionBeingReleased, versionToCheck);

            Assert.AreEqual(expectedValue, shouldBeMerged);
        }
       
    }
}