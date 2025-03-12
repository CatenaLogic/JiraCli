namespace JiraCli
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ActionAttribute : Attribute
    {
        public ActionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Description { get; set; }

        public string ArgumentsUsage { get; set; }
    }
}