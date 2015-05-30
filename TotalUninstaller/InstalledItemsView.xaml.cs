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
    }
}
