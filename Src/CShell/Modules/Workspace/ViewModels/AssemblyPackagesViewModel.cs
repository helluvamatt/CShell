namespace CShell.Modules.Workspace.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Caliburn.Micro;

    using CShell.Hosting.Package;
    using CShell.Modules.Workspace.Results;
    using CShell.Modules.Workspace.Views;

    using ScriptCs;

    [Export]
    public class AssemblyPackagesViewModel : Screen
    {
        private readonly INugetInstallationProvider _installationProvider;

        private string _filterText;

        private IList<IPackageInfo> _packageReferences;

        private bool _isSearching;

        public IList<IPackageInfo> PackageReferences
        {
            get
            {
                return _packageReferences ?? (_packageReferences = new List<IPackageInfo>());
            }
            set
            {
                _packageReferences = value;

                NotifyOfPropertyChange(() => PackageReferences);
            }
        }

        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;

                NotifyOfPropertyChange(() => FilterText);
            }
        }

        public bool IsSearching
        {
            get
            {
                return _isSearching;
            }
            set
            {
                _isSearching = value;

                NotifyOfPropertyChange(() => IsSearching);
            }
        }

        [ImportingConstructor]
        public AssemblyPackagesViewModel(INugetInstallationProvider installationProvider)
        {
            DisplayName = "Manage Nuget Packages";

            _installationProvider = installationProvider;
        }

        public async void Search()
        {
            try
            {
                IsSearching = true;

                PackageReferences = await Task.Factory.StartNew(() => _installationProvider.SearchPackages(FilterText, 0, 15).ToList());
            }
            finally 
            {
                IsSearching = false;
            }
        }

        public void Install(IPackageInfo packageInfo)
        {
            _installationProvider.InstallPackage(new PackageReference(packageInfo.Id, packageInfo.FrameworkName, packageInfo.Version));
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            _installationProvider.Initialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            PackageReferences.Clear();
            FilterText = string.Empty;
            IsSearching = false;
        }
    }
}
