using System;
using System.Windows;
using FinancialForecasting.Migration;
using Functional.Maybe;
using SimpleInjector;
using SimpleInjector.Integration.Wcf;

namespace FinancialForecasting.Desktop
{
    public static class AppStart
    {
        private static Container _container;
        private static SimpleInjectorServiceHost _host;

        /// <summary>
        ///     Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            SetupDatabasePath();
            HostServices();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            var app = new App();
            app.InitializeComponent();
            app.Run();
            CloseServices();
        }

        private static void SetupDatabasePath()
        {
            var commonApplicationData = Environment.SpecialFolder.CommonApplicationData;
            var folderPath = Environment.GetFolderPath(commonApplicationData);
            AppDomain.CurrentDomain.SetData("DataDirectory", folderPath);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject.ToMaybe().Cast<object, Exception>();
            MessageBox.Show(exception.Select(x => x.ToString()).OrElseDefault());
            MessageBox.Show(exception.Select(x => x.InnerException).Select(x => x.ToString()).OrElseDefault());
        }

        private static void HostServices()
        {
            _container = new Container();
            _container.Register<FinancialForecastingContext>();
            _container.Verify();
            _host = new SimpleInjectorServiceHost(_container, typeof (MigrationService));
            _host.Open();
        }

        private static void CloseServices()
        {
            _host.Close();
        }
    }
}