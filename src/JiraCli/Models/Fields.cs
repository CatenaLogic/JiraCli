using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiraCli.Models
{
    public class Fields
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("assignee")]
        public Assignee Assignee { get; set; }

        [JsonProperty("parent")]
        public JiraIssue Parent { get; set; }

        [JsonProperty("issuetype")]
        public JiraIssueType IssueType { get; set; }
    }
}
