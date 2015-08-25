// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraSearchRequest.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Models
{
    using System.Collections.Generic;

    public class JiraSearchRequest
    {
        public JiraSearchRequest()
        {
            MaxResults = 200;
            Fields = new List<string>();
        }

        public string Jql { get; set; }

        public int StartAt { get; set; }

        public int MaxResults { get; set; }

        public List<string> Fields { get; private set; } 
    }
}