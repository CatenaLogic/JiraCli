namespace JiraCli
{
    using System;

    public interface IHelpWriter
    {
        void WriteAppHeader(Action<string> writer);
        void WriteHelp(Action<string> writer);
    }
}