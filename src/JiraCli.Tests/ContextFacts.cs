// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextFacts.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Test
{
    using Catel.Test;
    using NUnit.Framework;
    using Tests;

    public class ContextFacts
    {
        [TestFixture]
        public class TheValidateContextMethod
        {
            [TestCase]
            public void ThrowsExceptionForMissingJiraUrl()
            {
                var context = TestHelper.GetValidContext();
                context.JiraUrl = string.Empty;

                ExceptionTester.CallMethodAndExpectException<JiraCliException>(() => context.ValidateContext());
            }

            [TestCase]
            public void ThrowsExceptionForMissingUserName()
            {
                var context = TestHelper.GetValidContext();
                context.UserName = string.Empty;

                ExceptionTester.CallMethodAndExpectException<JiraCliException>(() => context.ValidateContext());
            }

            [TestCase]
            public void ThrowsExceptionForMissingPassword()
            {
                var context = TestHelper.GetValidContext();
                context.Password = string.Empty;

                ExceptionTester.CallMethodAndExpectException<JiraCliException>(() => context.ValidateContext());
            }

            [TestCase]
            public void SucceedsForValidContext()
            {
                var context = TestHelper.GetValidContext();
                //{
                //    JiraUrl = "someurl",
                //    Version = "1.0"
                //};

                // should not throw
                context.ValidateContext();
            }
        }
    }
}