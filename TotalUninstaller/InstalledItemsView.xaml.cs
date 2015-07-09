using System.Linq;
using System.Windows.Input;

namespace TotalUninstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class InstalledItemsView
    {
        public InstalledItemsView()
        {
            InitializeComponent();
            DataContext = new InstalledItemsViewModel(this);
        }

        private void DataGrid_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                foreach (var installedItem in DataGrid.SelectedItems.Cast<InstalledItem>())
                    installedItem.Uninstall = !installedItem.Uninstall;

                e.Handled = true;
            }
        }
    }
}
