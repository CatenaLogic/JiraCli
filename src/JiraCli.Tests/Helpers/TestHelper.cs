namespace JiraCli.Tests
{
    public static class TestHelper
    {
        public static string GetDefaultCommandLine()
        {
            return "-url http://myjira.atlassian.net -user username -pw password -action someaction";
        }

        public static Context GetValidContext()
        {
            return ArgumentParser.ParseArguments(GetDefaultCommandLine());
        }
    }
}