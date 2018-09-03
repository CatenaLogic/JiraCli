// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomDateTimeConverter.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Models.Formatters
{
    using Newtonsoft.Json.Converters;

    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}