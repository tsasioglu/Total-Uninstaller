using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Deployment.WindowsInstaller;

namespace TotalUninstaller
{
    public class InstalledItemsViewModel : INotifyPropertyChanged
    {
        private readonly InstalledItemsView _view;

        public ICommand UninstallCommand { get; set; }
        public ICommand CancelUninstallCommand { get; set; }

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

        private int _uninstallCurrent;

        public int UninstallCurrent
        {
            get { return _uninstallCurrent; }
            set
            {
                _uninstallCurrent = value;
                OnPropertyChanged();
            }
        }

        private int _uninstallTotal;

        public int UninstallTotal
        {
            get { return _uninstallTotal; }
            set
            {
                _uninstallTotal = value;
                OnPropertyChanged();
            }
        }

        private int _uninstallProgress;

        public int UninstallProgress
        {
            get { return _uninstallProgress; }
            set
            {
                _uninstallProgress = value;
                OnPropertyChanged();
            }
        }

        private bool _uninstallInProgress;

        public bool UninstallInProgress
        {
            get { return _uninstallInProgress; }
            set
            {
                _uninstallInProgress = value;
                OnPropertyChanged();
            }
        }

        private bool _cancelling;

        public bool Cancelling
        {
            get { return _cancelling; }
            set
            {
                _cancelling = value;
                OnPropertyChanged();
            }
        }

        public InstalledItemsViewModel(InstalledItemsView view)
        {
            _view = view;

            UninstallCommand       = new DelegateCommand(Uninstall);
            CancelUninstallCommand = new DelegateCommand(CancelUninstall);

            LoadUninstallableItems();
        }

        private void CancelUninstall()
        {
            Cancelling          = true;
            UninstallInProgress = false;
        }

        private void LoadUninstallableItems()
        {
            var installations = ProductInstallation.GetProducts(null, "s-1-1-0", UserContexts.All)
                                                   .Where(ins  => ins.ProductName != null)
                                                   .Select(ins => new InstalledItem(ins.ProductName, 
                                                                                    ins.ProductCode, 
                                                                                    ins.InstallDate, 
                                                                                    ins.InstallLocation, 
                                                                                    ins.UrlInfoAbout, 
                                                                                    ins.ProductVersion))
                                                   .OrderBy(ins => ins.Product);
            
            Items = new ObservableCollection<InstalledItem>(installations);

            Cancelling          = false;
            UninstallInProgress = false;
            UninstallCurrent    = 0;
            UninstallTotal      = 0;
            UninstallProgress   = 0;

        }
        
        private void Uninstall()
        {
            var itemsToUninstall = Items.Where(i => i.Uninstall)
                                        .ToList();

            var count = itemsToUninstall.Count;
            if (count == 0)
                return;

            string plural = count > 1 ? "s" : String.Empty;

            Task<MessageDialogResult> task = _view.ShowMessageAsync(String.Format("Uninstall {0} item{1}", count, plural),
                                                                    String.Format("Are you sure you want to uninstall the selected {0} item{1}?", count, plural),
                                                                    MessageDialogStyle.AffirmativeAndNegative);

            task.ContinueWith(t =>
            {
                if (t.Result == MessageDialogResult.Negative)
                    return;

                UninstallInProgress = true;
                UninstallTotal = count;
                foreach (InstalledItem item in itemsToUninstall.TakeWhile(item => !Cancelling))
                    Uninstall(item);

                LoadUninstallableItems();
            });
        }

        private void Uninstall(InstalledItem item)
        {
            UninstallCurrent++;
            UninstallProgress = UninstallCurrent * 100 / UninstallTotal;

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
