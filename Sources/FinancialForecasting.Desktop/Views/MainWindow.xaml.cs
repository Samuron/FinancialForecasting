using System.Windows.Controls;
using FinancialForecasting.Desktop.ViewModels;
using MahApps.Metro.Controls;

namespace FinancialForecasting.Desktop.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView) sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "Модель Альтмана";
                    break;
                case 1:
                    flipview.BannerText = "Нечітке множинне виведення";
                    break;
                case 2:
                    flipview.BannerText = "Модель Мамдані";
                    break;
                case 3:
                    flipview.BannerText = "Порівняння моделей";
                    break;
            }
        }
    }
}