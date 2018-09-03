using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiraCli.Models
{
    public class JiraIssueType : JiraObjectBase
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("subtask")]
        public bool SubTask { get; set; }

    }
}
