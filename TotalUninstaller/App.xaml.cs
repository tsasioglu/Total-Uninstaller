using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;

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
            ConfigureLogging();
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private static void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

            using (var target = new FileTarget())
            {
                target.Name     = "log";
                target.FileName = @"${specialfolder:folder=ApplicationData}/TotalUninstaller/log_${cached:${longdate}:cached=true}.txt";
                target.Layout   = @"${longdate} ${message} ${exception:format=tostring}";
                config.AddTarget("log", target);

                var rule = new LoggingRule("*", LogLevel.Trace, target);
                config.LoggingRules.Add(rule);
            }

            LogManager.Configuration = config;
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var logger = LogManager.GetLogger("log");
            logger.Error("Unhandled Exception:", args.Exception);
        }
    }
}
