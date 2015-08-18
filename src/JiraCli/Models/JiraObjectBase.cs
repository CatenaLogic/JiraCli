// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraObjectBase.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Models
{
    public class JiraObjectBase
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Self { get; set; }
    }
}