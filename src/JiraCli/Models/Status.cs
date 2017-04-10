using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiraCli.Models
{
    public class Status : JiraObjectBase
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        //[JsonProperty("name")]
        //public string Name { get; set; }

        //[JsonProperty("id")]
        //public int Id { get; set; }
    }
}
