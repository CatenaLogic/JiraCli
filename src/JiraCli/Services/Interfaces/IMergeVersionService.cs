namespace JiraCli.Services
{
    public interface IMergeVersionService
    {
        bool ShouldBeMerged(string versionBeingReleased, string versionToCheck);
    }
}