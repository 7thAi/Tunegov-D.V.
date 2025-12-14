using Microsoft.Win32;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp1.Pages
{
    public partial class AddEditPage : Page
    {
        private User _currentUser;
        private string _photo;

        private const string DefaultPhoto =
            "pack://application:,,,/Resources/DefaultPhoto.png";

        public AddEditPage(User selectedUser)
        {
            InitializeComponent();
            _currentUser = selectedUser;

            if (_currentUser != null)
            {
                TbFIO.Text = _currentUser.FIO;
                TbLogin.Text = _currentUser.Login;
                CbRole.Text = _currentUser.Role;

                _photo = string.IsNullOrEmpty(_currentUser.Photo)
                    ? DefaultPhoto
                    : _currentUser.Photo;
            }
            else
            {
                _photo = DefaultPhoto;
            }

            ImgPhoto.Source = new BitmapImage(new System.Uri(_photo));
        }

        private void BtnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                _photo = dlg.FileName;
                ImgPhoto.Source = new BitmapImage(new System.Uri(_photo));
            }
        }

        private string GetHash(string input)
        {
            using (SHA1 sha = SHA1.Create())
            {
                return string.Concat(
                    sha.ComputeHash(Encoding.UTF8.GetBytes(input))
                       .Select(x => x.ToString("X2")));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbFIO.Text) ||
                string.IsNullOrWhiteSpace(TbLogin.Text))
            {
                MessageBox.Show("ФИО и логин обязательны для заполнения!");
                return;
            }

            var db = WpfApp1Entities.GetContext();

            if (_currentUser != null)
            {
                _currentUser.FIO = TbFIO.Text;
                _currentUser.Login = TbLogin.Text;
                _currentUser.Role = CbRole.Text;
                _currentUser.Photo = _photo;

                if (!string.IsNullOrWhiteSpace(PbPassword.Password))
                    _currentUser.Password = GetHash(PbPassword.Password);
            }
            else
            {
                db.User.Add(new User
                {
                    FIO = TbFIO.Text,
                    Login = TbLogin.Text,
                    Password = GetHash("123"),
                    Role = CbRole.Text,
                    Photo = _photo
                });
            }

            db.SaveChanges();
            NavigationService.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
