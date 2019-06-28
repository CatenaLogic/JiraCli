// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateVersionAction.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Services;

    [Action("ReleaseVersion",
        Description = "Releases a version for the specified project",
        ArgumentsUsage = "-project [projectkey] -version [version]")]
    public class ReleaseVersionAction : ActionBase
    {
        private readonly IVersionService _versionService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public ReleaseVersionAction(IVersionService versionService)
        {
            Argument.IsNotNull(() => versionService);

            _versionService = versionService;
        }

        protected override void ValidateContext(Context context)
        {
            if (string.IsNullOrEmpty(context.Project))
            {
                throw Log.ErrorAndCreateException<JiraCliException>("Project is missing");
            }

            if (string.IsNullOrEmpty(context.Version))
            {
                throw Log.ErrorAndCreateException<JiraCliException>("Version is missing");
            }
        }

        protected override async Task ExecuteWithContextAsync(Context context)
        {
            var jira = CreateJira(context);

            await _versionService.ReleaseVersionAsync(jira, context.Project, context.Version);

            if (context.MergeVersions)
            {
                var mergeVersionsJira = CreateJira(context);
                await _versionService.MergeVersionsAsync(mergeVersionsJira, context.Project, context.Version);
            }
        }
    }
}
