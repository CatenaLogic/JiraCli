// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraProjectVersion.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Models
{
    using Newtonsoft.Json;
    using System;
    using Formatters;

    public class JiraProjectVersion : JiraObjectBase
    {
        public long? Sequence { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }

        public bool Released { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? ReleaseDate { get; set; }

        public string UserReleaseDate { get; set; }

        public string Project { get; set; }

        public int? ProjectId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}