using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookOFLEGENDS
{
    /// <summary>
    /// Interaction logic for PAccountsAdd.xaml
    /// </summary>
    public partial class PAccountsAdd : Page
    {
        public string nameEx = "";
        public PAccountsAdd(string username)
        {
            InitializeComponent();
            nameEx = username;
        }

        private void UsersBack(object sender, RoutedEventArgs e)
        {
            PAccounts pAccounts = new PAccounts(nameEx);
            this.NavigationService.Navigate(pAccounts);
        }

        private void UsersAdd_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string role = UsersRole.Text;

            string result = Validate(username, email, password, role);
            if (result == "success")
            {
                if (role == "Admin")
                {
                    MessageBox.Show("Admin felhasználó létrehozva!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Users user = new Users(username, email, password, Role.Admin);
                    using (StreamWriter sw = new StreamWriter("users.txt", true))
                    {
                        sw.WriteLine(user.ToString());
                    }
                }
                else if (role == "User")
                {
                    Users user = new Users(username, email, password, Role.User);
                    using (StreamWriter sw = new StreamWriter("users.txt", true))
                    {
                        sw.WriteLine(user.ToString());
                    }
                    MessageBox.Show("Felhasználó létrehozva!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Hibás szerepkör!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                PAccounts pAccounts = new PAccounts(nameEx);
                this.NavigationService.Navigate(pAccounts);
            }
        }

        private string Validate(string username, string email, string password, string role)
        {
            if (username == "" || email == "" || password == "" || role == "")
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "error";
            }

            Regex pattern = new Regex(@"^([\w\.\-]+)@(gmail)((\.(\w){2,3})+)$");
            if (!pattern.IsMatch(email))
            {
                MessageBox.Show("Nem megfelelő email formátum!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "error";
            }

            if (File.Exists("users.txt"))
            {
                using (StreamReader r = new StreamReader("users.txt"))
                {
                    string line = r.ReadLine();
                    if (line == null)
                    {
                        return "success";
                    }
                }
                using (StreamReader line = new StreamReader("users.txt"))
                {
                    string x;
                    while ((x = line.ReadLine()) != null)
                    {
                        Users user = Users.FromString(x);
                        if (user.Username == username)
                        {
                            MessageBox.Show("Ez a felhasználónév már foglalt!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return "error";
                        }
                    }
                }
            }

            if (!IsPasswordStrong(password))
            {
                MessageBox.Show("A jelszónak tartalmaznia kell legalább egy nagybetűt, egy kisbetűt, egy számot és legalább 8 karakter hosszúnak kell lennie!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "error";
            }
            return "success";
        }

        private bool IsPasswordStrong(string password)
        {
            var res = password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit) && password.Length >= 8 && password.Any(ch => char.IsLetterOrDigit(ch));
            return res;
        }
    }
}
