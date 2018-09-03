// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraObjectBase.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Newtonsoft.Json;

namespace JiraCli.Models
{
    public class JiraObjectBase
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string Self { get; set; }
    }
}