// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVersionInfoService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    public interface IVersionInfoService
    {
        bool IsStableVersion(string version);
    }
}