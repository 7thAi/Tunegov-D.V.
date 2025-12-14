using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class UsersTilesPage : Page
    {
        private List<User> _users;

        public UsersTilesPage()
        {
            InitializeComponent();

            CbRole.SelectedIndex = 0;
            CbSort.SelectedIndex = 0;

            LoadData();
        }

        private void LoadData()
        {
            _users = WpfApp1Entities.GetContext().User.ToList();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<User> filtered = _users;

            // Поиск
            if (!string.IsNullOrWhiteSpace(TbSearch.Text))
            {
                filtered = filtered.Where(u =>
                    u.Login.ToLower().Contains(TbSearch.Text.ToLower()));
            }

            // Фильтр по роли
            ComboBoxItem roleItem = CbRole.SelectedItem as ComboBoxItem;
            if (roleItem != null && roleItem.Content.ToString() != "Все")
            {
                filtered = filtered.Where(u => u.Role == roleItem.Content.ToString());
            }

            // Сортировка
            ComboBoxItem sortItem = CbSort.SelectedItem as ComboBoxItem;
            if (sortItem != null)
            {
                string sort = sortItem.Content.ToString();

                if (sort == "По ID ↑")
                    filtered = filtered.OrderBy(u => u.ID);
                else if (sort == "По ID ↓")
                    filtered = filtered.OrderByDescending(u => u.ID);
                else if (sort == "По логину A-Z")
                    filtered = filtered.OrderBy(u => u.Login);
                else if (sort == "По логину Z-A")
                    filtered = filtered.OrderByDescending(u => u.Login);
            }

            LvUsers.ItemsSource = filtered.ToList();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
