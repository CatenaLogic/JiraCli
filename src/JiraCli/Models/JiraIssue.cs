// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraIssue.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using JiraCli.Models;
using Newtonsoft.Json;
using System;

namespace JiraCli.Models
{  

    public class JiraIssue : JiraObjectBase
    {

        private string _KeyString;

        [JsonProperty("expand")]
        public string Expand { get; set; }

        #region Special key solution
        [JsonProperty("key")]
        public string ProxyKey
        {
            get
            {
                return Key.ToString();
            }
            set
            {
                _KeyString = value;
            }
        }

        [JsonIgnore]
        public IssueKey Key
        {
            get
            {
                return IssueKey.Parse(_KeyString);
            }
        }
        #endregion Special key solution

        [JsonProperty("fields")]
        public Fields Fields { get; set; }

        [JsonProperty("parent")]
        public JiraIssue Parent { get; set; }

       

    }  

 

}