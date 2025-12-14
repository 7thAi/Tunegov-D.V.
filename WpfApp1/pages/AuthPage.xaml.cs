using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbLogin.Text) ||
                string.IsNullOrWhiteSpace(PbPassword.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            string hash = RegPage.GetHash(PbPassword.Password);

            var user = WpfApp1Entities.GetContext().User
                        .FirstOrDefault(x => x.Login == TbLogin.Text &&
                                             x.Password == hash);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден!");
                return;
            }

            MessageBox.Show("Успешный вход!");

            if (user.Role == "Администратор")
                NavigationService.Navigate(new AdminPage());
            else
                NavigationService.Navigate(new UserPage());
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}
