namespace JiraCli
{
    using System;

    public class JiraCliException : Exception
    {
        public JiraCliException(string message)
            : base(message)
        {
        }
    }
}