using CShell.Framework.Services;
using ScriptCs;

namespace CShell.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Reflection;

    using ScriptCs.Contracts;

    public class ReplScriptExecutorFactory : IReplScriptExecutorFactory
    {
        private readonly IReplOutput replOutput;
        private readonly IDefaultReferences defaultReferences;

        private readonly IPackageAssemblyResolver _packageAssemblyResolver;

        private readonly IAssemblyUtility _assemblyUtility;

        private readonly ScriptServices scriptServices;

        public ReplScriptExecutorFactory(ScriptServices scriptServices, IReplOutput replOutput, IDefaultReferences defaultReferences, IPackageAssemblyResolver packageAssemblyResolver, IAssemblyUtility assemblyUtility)
        {
            this.scriptServices = scriptServices;
            this.replOutput = replOutput;
            this.defaultReferences = defaultReferences;
            _packageAssemblyResolver = packageAssemblyResolver;
            _assemblyUtility = assemblyUtility;
        }

        public IReplScriptExecutor Create(string workspaceDirectory)
        {
            IFileSystem fileSystem = scriptServices.FileSystem;
            fileSystem.CurrentDirectory = workspaceDirectory;
            scriptServices.InstallationProvider.Initialize();

            var replExecutor = new ReplScriptExecutor(
                replOutput, 
                scriptServices.ObjectSerializer, 
                fileSystem, 
                scriptServices.FilePreProcessor,
                scriptServices.Engine,
                scriptServices.LogProvider,
                scriptServices.ReplCommands,
                defaultReferences,
                _packageAssemblyResolver
                );

            var assemblies = scriptServices.AssemblyResolver.GetAssemblyPaths(fileSystem.CurrentDirectory).ToList();
            IEnumerable<IScriptPack> scriptPacks = GetScriptPacks(assemblies.Where(assembly => ShouldLoadAssembly(fileSystem, assembly)));

            replExecutor.Initialize(assemblies, scriptPacks);
            replOutput.Initialize(replExecutor);

            return replExecutor;
        }

        private static IEnumerable<IScriptPack> GetScriptPacks(IEnumerable<string> assemblies)
        {
            var aggregateCatalog = new AggregateCatalog();

            foreach (var assemblyPath in assemblies)
            {
                var catalog = new AssemblyCatalog(assemblyPath);
                aggregateCatalog.Catalogs.Add(catalog);
            }

            var container = new CompositionContainer(aggregateCatalog);
            var scriptPacks = container.GetExportedValues<IScriptPack>(); //scriptServices.ScriptPackResolver.GetPacks();
            return scriptPacks;
        }

        // HACK: Filter out assemblies in the GAC by checking if full path is specified.
        private bool ShouldLoadAssembly(IFileSystem fileSystem, string assembly)
        {
            return fileSystem.IsPathRooted(assembly) && _assemblyUtility.IsManagedAssembly(assembly);
        }
    }
}
