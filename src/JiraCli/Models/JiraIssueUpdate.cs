namespace JiraCli.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class JiraIssueUpdate : JiraObjectBase
    {

        public JiraIssueUpdate()
        {
            Update = new Dictionary<string, object>();
        }

        public void AddFieldValue(string name, object value)
        {
            var array = new List<object>();
            // array.Add("add", value);
            array.Add(new { Add = value });
            Update[name] = array;
        }
      
        public Dictionary<string, object> Update { get; set; }

    }


}