using Catel.IoC;
using JiraCli;
using JiraCli.Services;
/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IActionManager, ActionManager>();
        serviceLocator.RegisterType<IHelpWriter, HelpWriter>();
        serviceLocator.RegisterType<IVersionInfoService, VersionInfoService>();
        serviceLocator.RegisterType<IMergeVersionService, MergeVersionService>();
        serviceLocator.RegisterType<IVersionService, VersionService>();
    }
}