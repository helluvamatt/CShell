namespace CShell.Hosting.Package
{
    using System.Collections.Generic;

    using ScriptCs.Contracts;

    public interface INugetInstallationProvider
    {
        void Initialize();

        IEnumerable<string> GetRepositorySources(string path);

        IEnumerable<IPackageInfo> SearchPackages(string filter, int page, int pageSize);

        void InstallPackage(IPackageReference packageId, bool allowPreRelease = false);

        bool IsInstalled(IPackageReference packageReference, bool allowPreRelease = false);
    }
}