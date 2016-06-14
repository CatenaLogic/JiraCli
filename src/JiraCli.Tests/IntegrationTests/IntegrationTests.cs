// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegrationTests.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Tests.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using NUnit.Framework;

    [TestFixture, Explicit]
    public partial class IntegrationTests
    {
        public IntegrationTests()
        {
            LogManager.AddDebugListener(true);
        }

        private T ResolveService<T>()
        {
            return ServiceLocator.Default.ResolveType<T>();
        }

        private Context CreateContext(string version, string[] issues = null)
        {
            var context = new Context
            {
                JiraUrl = JiraUrl,
                Project = JiraProject,
                UserName = JiraUser,
                Password = JiraPassword,
                Version = version,
                Issues = issues ?? new string[] { }
            };

            return context;
        }

        [TestCase]
        public async Task ReleaseVersions()
        {
            var actionManager = ResolveService<IActionManager>();

            var action = actionManager.GetAction("CreateAndReleaseVersion");

            var createV100Context = CreateContext("1.0.0");
            Assert.IsTrue(await action.Execute(createV100Context));

            var createV110Context = CreateContext("1.1.0");
            Assert.IsTrue(await action.Execute(createV110Context));

            var createV111Context = CreateContext("1.1.1");
            Assert.IsTrue(await action.Execute(createV111Context));
            Assert.IsFalse(await action.Execute(createV111Context));
        }

        [TestCase]
        public async Task ReleaseVersionsThatShouldBeMerged()
        {
            var actionManager = ResolveService<IActionManager>();

            var action = actionManager.GetAction("CreateAndReleaseVersion");

            var createV1Context = CreateContext("2.0.0-unstable42");
            Assert.IsTrue(await action.Execute(createV1Context));

            var createV2Context = CreateContext("2.0.0-unstable48");
            Assert.IsTrue(await action.Execute(createV2Context));

            var createV3Context = CreateContext("2.0.0-unstable56");
            Assert.IsTrue(await action.Execute(createV3Context));

            var createStableVersionContext = CreateContext("2.0.0");
            Assert.IsTrue(await action.Execute(createStableVersionContext));
        }
        
     
        [TestCase("TS-1,TS-2,TS-3")]
        public async Task AssignVersion(string issueNumbers)
        {

            var actionManager = ResolveService<IActionManager>();

            var action = actionManager.GetAction("AssignVersion");
            var issues = issueNumbers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var createV100Context = CreateContext("1.0.0", issues);
            Assert.IsTrue(await action.Execute(createV100Context));

        }
    }
}