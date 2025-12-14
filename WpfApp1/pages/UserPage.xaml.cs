using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            CmbSorting.SelectedIndex = 0;
            CheckUser.IsChecked = false;

            UpdateUsers();
        }

        private void UpdateUsers()
        {
            var currentUsers = WpfApp1Entities.GetContext().User.ToList();

            // Поиск по ФИО
            if (!string.IsNullOrWhiteSpace(TextBoxSearch.Text))
            {
                currentUsers = currentUsers
                    .Where(x => x.FIO.ToLower()
                    .Contains(TextBoxSearch.Text.ToLower()))
                    .ToList();
            }

            // Фильтр по роли "Пользователь"
            if (CheckUser.IsChecked.Value)
            {
                currentUsers = currentUsers
                    .Where(x => x.Role.Contains("Пользователь"))
                    .ToList();
            }

            // Сортировка
            if (CmbSorting.SelectedIndex == 0)
                ListUser.ItemsSource = currentUsers.OrderBy(x => x.FIO).ToList();
            else
                ListUser.ItemsSource = currentUsers.OrderByDescending(x => x.FIO).ToList();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CheckUser_Changed(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            CheckUser.IsChecked = false;
            CmbSorting.SelectedIndex = 0;
            UpdateUsers();
        }
    }
}
