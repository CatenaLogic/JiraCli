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

    [Action("CreateVersion",
        Description = "Creates a version for the specified project",
        ArgumentsUsage = "-project [projectkey] -version [version]")]
    public class CreateVersionAction : ActionBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IVersionService _versionService;

        public CreateVersionAction(IVersionService versionService)
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

            await _versionService.CreateVersionAsync(jira, context.Project, context.Version);
        }
    }
}
