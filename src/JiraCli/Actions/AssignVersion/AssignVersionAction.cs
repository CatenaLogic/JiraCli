// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateAndReleaseVersionAction.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Services;

    [Action("AssignVersion",
        Description = "Assigns a version to the issues specified",
        ArgumentsUsage = "-project [projectkey] -version [version] -issues [issue1,issues2,issue3]")]
    public class AssignVersionAction : ActionBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IVersionService _versionService;

        public AssignVersionAction(IVersionService versionService)
        {
            Argument.IsNotNull(() => versionService);

            _versionService = versionService;
        }

        protected override void ValidateContext(Context context)
        {
            if (string.IsNullOrEmpty(context.Project))
            {
                Log.ErrorAndThrowException<JiraCliException>("Project is missing");
            }

            if (string.IsNullOrEmpty(context.Version))
            {
                Log.ErrorAndThrowException<JiraCliException>("Version is missing");
            }

            if (context.Issues == null || !context.Issues.Any())
            {
                Log.ErrorAndThrowException<JiraCliException>("Issues are missing.");
            }
        }

        protected override void ExecuteWithContext(Context context)
        {
            // Note: we need a different instance of jira because it caches results

            var createVersionJira = CreateJira(context);
            //  var version = _versionService.
            _versionService.AssignVersionToIssues(createVersionJira, context.Project, context.Version, context.Issues);

        }
    }
}