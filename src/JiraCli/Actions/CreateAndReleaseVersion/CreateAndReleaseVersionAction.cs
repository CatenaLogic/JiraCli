// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateAndReleaseVersionAction.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Services;

    [Action("CreateAndReleaseVersion",
        Description = "Creates and releases a version for the specified project",
        ArgumentsUsage = "-project [projectkey] -version [version]")]
    public class CreateAndReleaseVersionAction : ActionBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IVersionService _versionService;

        public CreateAndReleaseVersionAction(IVersionService versionService)
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
            // Note: we need a different instance of jira because it caches results

            var createVersionJira = CreateJira(context);
            await _versionService.CreateVersionAsync(createVersionJira, context.Project, context.Version);

            var releaseVersionJira = CreateJira(context);
            await _versionService.ReleaseVersionAsync(releaseVersionJira, context.Project, context.Version);

            if (context.MergeVersions)
            {
                var mergeVersionsJira = CreateJira(context);
                await _versionService.MergeVersionsAsync(mergeVersionsJira, context.Project, context.Version);
            }
        }
    }
}
