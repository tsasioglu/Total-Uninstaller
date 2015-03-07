using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Deployment.WindowsInstaller;

namespace TotalUninstaller
{
    public class InstalledItemsViewModel : INotifyPropertyChanged
    {
        private readonly InstalledItemsView _view;

        public ICommand UninstallCommand { get; set; }

        private ObservableCollection<InstalledItem> _items; 
        public  ObservableCollection<InstalledItem> Items 
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            } 
        }
        
        public InstalledItemsViewModel(InstalledItemsView view)
        {
            _view = view;
            UninstallCommand = new DelegateCommand(Uninstall);

            LoadUninstallableItems();
        }

        private void LoadUninstallableItems()
        {
            var installations = ProductInstallation.GetProducts(null, "s-1-1-0", UserContexts.All)
                                                   .Select(ins => new InstalledItem(ins.ProductName, 
                                                                                    ins.ProductCode, 
                                                                                    ins.InstallDate, 
                                                                                    ins.InstallLocation, 
                                                                                    ins.UrlInfoAbout, 
                                                                                    ins.ProductVersion))
                                                   .OrderBy(ins => ins.Product);
            
            Items = new ObservableCollection<InstalledItem>(installations);
        }
        
        private void Uninstall()
        {
            var itemsToUninstall = Items.Where(i => i.Uninstall)
                                        .ToList();

            var count = itemsToUninstall.Count;
            if (count == 0)
                return;

            string plural = count > 1 ? "s" : String.Empty;
            
            var result =
                MessageBox.Show(
                    String.Format("Are you sure you want to uninstall the selected {0} item{1}?", count, plural),
                    String.Format("Uninstall {0} item{1}", count, plural),
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.No)
                return;

            foreach (InstalledItem item in itemsToUninstall)
                Uninstall(item);

            LoadUninstallableItems();
        }

        private static void Uninstall(InstalledItem item)
        {
            Installer.SetInternalUI(InstallUIOptions.ProgressOnly | InstallUIOptions.SourceResolutionOnly | InstallUIOptions.UacOnly);
            Installer.ConfigureProduct(item.ProductCode, 0, InstallState.Absent, "IGNOREDEPENDENCIES=\"ALL\"");
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
