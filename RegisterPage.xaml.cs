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
using System.Windows.Shapes;

namespace BookOFLEGENDS
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        public enum Role
        {
            Admin,
            User
        }

        private string Validate(string username, string email, string password, string passwordAgain, bool terms)
        {
            if (!terms)
            {
                MessageBox.Show("Fogadd el hogy ellopjuk az összes adatod! (Nem biztonságos!)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "error";
            }

            if (password != passwordAgain)
            {
                MessageBox.Show("A két jelszó nem egyezik!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "error";
            }

            if (username == "" || email == "" || password == "" || passwordAgain == "")
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

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private bool IsPasswordStrong(string password)
        {
            var res = password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit) && password.Length >= 8 && password.Any(ch => char.IsLetterOrDigit(ch));
            return res;
        }


        private bool firstUser()
        {
            try
            {
                if (File.Exists("users.txt"))
                {
                    using (StreamReader r = new StreamReader("users.txt"))
                    {
                        string line = r.ReadLine();
                        Regex pattern = new Regex(@".*Admin.*");
                        if (pattern.IsMatch(line))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                return true;
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string email = emailBox.Text;
            string password = passwordBox.Password;
            string passwordAgain = passwordBox2.Password;
            bool terms = termsAndCondition.IsChecked.Value;

            string result = Validate(username, email, password, passwordAgain, terms);
            if (result == "success")
            {
                Role role = firstUser() ? Role.Admin : Role.User;
                using (StreamWriter sw = new StreamWriter("users.txt", true))
                {
                    sw.WriteLine($"{username};{email};{password};{role}");
                }
                MessageBox.Show("Sikeres regisztráció!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sikertelen regisztráció!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
