// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using System;
    using Catel;
    using Catel.Logging;

    public class MergeVersionService : IMergeVersionService
    {
        private readonly IVersionInfoService _versionInfoService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public MergeVersionService(IVersionInfoService versionInfoService)
        {
            Argument.IsNotNull(() => versionInfoService);

            _versionInfoService = versionInfoService;
        }

        public bool ShouldBeMerged(string versionBeingReleased, string versionToCheck)
        {
            Argument.IsNotNull(() => versionBeingReleased);
            Argument.IsNotNull(() => versionToCheck);

            if (!_versionInfoService.IsStableVersion(versionBeingReleased))
            {
                return false;
            }

            if (versionToCheck.StartsWith(versionBeingReleased))
            {
                return true;
            }

            return false;
        }
    }
}