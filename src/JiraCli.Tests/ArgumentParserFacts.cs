// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentParserFacts.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Test
{
    using Catel.Test;
    using JiraCli;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentParserFacts
    {
        [TestCase]
        public void ThrowsExceptionForEmptyParameters()
        {
            ExceptionTester.CallMethodAndExpectException<JiraCliException>(() => ArgumentParser.ParseArguments(string.Empty));
        }

        [TestCase]
        public void CorrectlyParsesCredentials()
        {
            var context = ArgumentParser.ParseArguments("-url http://myjira.atlassian.net -user username -pw password");

            Assert.AreEqual("http://myjira.atlassian.net", context.JiraUrl);
            Assert.AreEqual("username", context.UserName);
            Assert.AreEqual("password", context.Password);
        }

        [TestCase]
        public void CorrectlyParsesHelp()
        {
            var context = ArgumentParser.ParseArguments("-h");

            Assert.IsTrue(context.IsHelp);
        }

        //[TestCase]
        //public void CorrectlyParsesVersion()
        //{
        //    var context = ArgumentParser.ParseArguments("-v 1.0.0-unstable001");

        //    Assert.AreEqual("1.0.0-unstable001", context.Version);
        //}

        //[TestCase]
        //public void CorrectlyParsesCi()
        //{
        //    var context = ArgumentParser.ParseArguments("-ci true");

        //    Assert.IsTrue(context.IsCi);
        //}

        //[TestCase]
        //public void CorrectlyParsesLogFilePath()
        //{
        //    var context = ArgumentParser.ParseArguments("-l logFilePath");

        //    Assert.AreEqual("logFilePath", context.LogFile);
        //}


        //[TestCase]
        //public void CorrectlyParsesBranchNameAndVersion()
        //{
        //    var context = ArgumentParser.ParseArguments("-b develop -v 1.0.0-unstable001");

        //    Assert.AreEqual("develop", context.BranchName);
        //    Assert.AreEqual("1.0.0-unstable001", context.Version);
        //}

        [TestCase]
        public void ThrowsExceptionForInvalidNumberOfArguments()
        {
            ExceptionTester.CallMethodAndExpectException<JiraCliException>(() => ArgumentParser.ParseArguments("-l logFilePath extraArg"));
        }

        [TestCase]
        public void ThrowsExceptionForUnknownArgument()
        {
            ExceptionTester.CallMethodAndExpectException<JiraCliException>(() => ArgumentParser.ParseArguments("solutionDirectory -x logFilePath"));
        }
    }
}