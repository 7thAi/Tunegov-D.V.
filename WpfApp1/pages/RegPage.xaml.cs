using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Pages
{
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        // Хеширование пароля
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }

        // Ограничение ввода логина: только буквы и цифры
        private void TbLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[a-zA-Z0-9]");
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            // Проверка пустоты
            if (string.IsNullOrWhiteSpace(TbFIO.Text) ||
                string.IsNullOrWhiteSpace(TbLogin.Text) ||
                string.IsNullOrWhiteSpace(PbPassword.Password) ||
                string.IsNullOrWhiteSpace(PbPassword2.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            // Проверка ФИО
            if (TbFIO.Text.Length < 5)
            {
                MessageBox.Show("ФИО должно содержать не менее 5 символов");
                return;
            }

            // Проверка длины логина
            if (TbLogin.Text.Length < 3)
            {
                MessageBox.Show("Логин должен быть не менее 3 символов");
                return;
            }

            // Проверка длины пароля
            if (PbPassword.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов");
                return;
            }

            // Проверка совпадения паролей
            if (PbPassword.Password != PbPassword2.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            var db = WpfApp1Entities.GetContext();

            // Проверка уникальности логина
            if (db.User.Any(u => u.Login == TbLogin.Text))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            User newUser = new User()
            {
                FIO = TbFIO.Text,
                Login = TbLogin.Text,
                Password = GetHash(PbPassword.Password),
                Role = "Пользователь",
                Photo = null
            };

            db.User.Add(newUser);
            db.SaveChanges();

            MessageBox.Show("Регистрация прошла успешно!");
            NavigationService.Navigate(new AuthPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
