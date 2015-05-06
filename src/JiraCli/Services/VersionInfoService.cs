// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionInfoService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using Catel;

    public class VersionInfoService : IVersionInfoService
    {
        public bool IsStableVersion(string version)
        {
            Argument.IsNotNull(() => version);

            // Assume that if we can parse it, it's a stable version

            try
            {
                var parsedVersion = new Version(version);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}