using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext == null) return;
            var text = ((TextBox)sender).Text;
            var installedItemsViewModel = ((InstalledItemsViewModel)DataContext);
            installedItemsViewModel.Items = new ObservableCollection<InstalledItem>(installedItemsViewModel.AllItems.Where(x => x.Product.ToUpper().Contains(text.ToUpper())));
        }
    }
}
