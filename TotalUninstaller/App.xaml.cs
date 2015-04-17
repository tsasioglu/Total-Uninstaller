using System.Windows;
using System.Windows.Threading;
using NLog;

namespace TotalUninstaller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var logger = LogManager.GetLogger("log");
            logger.Error("Unhandled Exception:", args.Exception);
        }
    }
}
