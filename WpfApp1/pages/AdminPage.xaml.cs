using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            Loaded += AdminPage_Loaded;
        }

        private void AdminPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Обновление данных
        private void LoadData()
        {
            DgUsers.ItemsSource = WpfApp1Entities.GetContext()
                .User
                .ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsers.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя!");
                return;
            }

            var user = DgUsers.SelectedItem as User;
            NavigationService.Navigate(new AddEditPage(user));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsers.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя!");
                return;
            }

            var user = DgUsers.SelectedItem as User;

            if (MessageBox.Show("Удалить пользователя?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                WpfApp1Entities.GetContext().User.Remove(user);
                WpfApp1Entities.GetContext().SaveChanges();

                LoadData();
            }
        }
    }
}
