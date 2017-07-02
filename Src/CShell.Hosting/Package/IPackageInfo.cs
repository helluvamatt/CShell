namespace CShell.Hosting.Package
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Versioning;

    using ScriptCs.Contracts;

    public interface IPackageInfo
    {
        Uri IconUrl { get; }

        string Title { get; }

        string Description { get; }

        int DownloadCount { get; }

        IEnumerable<string> FrameworkAssemblies { get; }

        string Id { get; }

        string TextVersion { get; }

        Version Version { get; }

        FrameworkName FrameworkName { get; }

        IEnumerable<IPackageObject> Dependencies { get; set; }

        string FullName { get; }

        string PublishedDate { get; }

        string Authors { get; }

        IEnumerable<string> GetCompatibleDlls(FrameworkName frameworkName);

        IEnumerable<string> GetContentFiles();
    }
}