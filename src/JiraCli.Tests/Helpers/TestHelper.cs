// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Tests
{
    public static class TestHelper
    {
        public static string GetDefaultCommandLine()
        {
            return "-url http://myjira.atlassian.net -user username -pw password";
        }

        public static Context GetValidContext()
        {
            return ArgumentParser.ParseArguments(GetDefaultCommandLine());
        }
    }
}