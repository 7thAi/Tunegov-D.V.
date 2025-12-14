using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // загрузка страницы авторизации после старта
            MainFrame.Navigate(new Pages.AuthPage());

            // запуск таймера даты/времени
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                TxtDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            };
            timer.Start();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "Вы действительно хотите вернуться?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MainFrame.GoBack();
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            BtnBack.Visibility = MainFrame.CanGoBack
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        // Подтверждение выхода
        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show(
                "Вы действительно хотите выйти из приложения?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
