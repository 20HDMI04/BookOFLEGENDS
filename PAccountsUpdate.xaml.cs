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
    /// Interaction logic for PAccountsUpdate.xaml
    /// </summary>
    public partial class PAccountsUpdate : Page
    {
        public string nameEx = "";
        public string nameToUpdateEx = "";
        public string roleToUpdateEx = "";
        public PAccountsUpdate(string username, string nametoUpdate)
        {
            InitializeComponent();
            nameEx = username;
            nameToUpdateEx = nametoUpdate.Split('-')[0].Split(':')[1].Trim();
            roleToUpdateEx = nametoUpdate.Split('-')[2].Split(':')[1].Trim();
            Initalizing();
        }

        private void Initalizing()
        {
            List<string> lines = File.ReadAllLines("users.txt").ToList();

            List<string> selectedUser = lines.Where(x => x.Split(';')[0].Trim() == nameToUpdateEx).ToList();
            Console.WriteLine(selectedUser);
            string usernametoChange = selectedUser[0].Split(';')[0].Trim();
            string emailtoChange = selectedUser[0].Split(';')[1].Trim();
            string passwordtoChange = selectedUser[0].Split(';')[2].Trim();
            string roletoChange = selectedUser[0].Split(';')[3].Trim();


            UsernameBox.Text = usernametoChange;
            EmailBox.Text = emailtoChange;
            PasswordBox.Password = passwordtoChange;
            UsersRole.Text = roletoChange;
        }
        private void UsersBack(object sender, RoutedEventArgs e)
        {
            PAccounts pAccounts = new PAccounts(nameEx);
            this.NavigationService.Navigate(pAccounts);
        }

        private void UsersUpdate_Click(object sender, RoutedEventArgs e)
        {
            string changedUsername = UsernameBox.Text;
            string changedEmail = EmailBox.Text;
            string changedPassword = PasswordBox.Password;
            string changedRole = UsersRole.Text;

            if (Validate(changedUsername, changedEmail, changedPassword, changedRole) != "success")
            {
                MessageBox.Show("Hiba történt a felhasználó szerkesztésekor!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Update();

            PAccounts pAccounts = new PAccounts(nameEx);
            this.NavigationService.Navigate(pAccounts);
            return;
        }

        private string Validate(string username, string email, string password,string role)
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
                if (username != nameToUpdateEx)
                {
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

        private void Update()
        {
            List<string> lines = File.ReadAllLines("users.txt").ToList();

            string selectedUser = nameToUpdateEx;
            string username = selectedUser;
            string role = roleToUpdateEx;
            string changedUsername = UsernameBox.Text;
            string changedEmail = EmailBox.Text;
            string changedPassword = PasswordBox.Password;
            string changedRole = UsersRole.Text;

            if (role == "Admin" && lines[0].Contains(username))
            {
                MessageBox.Show("Ez az Admin felhasználó nem szerkeszthető!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            int index = lines.FindIndex(x => x.Split(';')[0].Trim() == nameToUpdateEx);
            lines[index] = $"{changedUsername};{changedEmail};{changedPassword};{changedRole}";
            File.WriteAllLines("users.txt", lines);
            MessageBox.Show("Felhasználó szerkesztve!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
