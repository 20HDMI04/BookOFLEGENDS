using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaction logic for PAccounts.xaml
    /// </summary>
    public partial class PAccounts : Page
    {
        public string nameEx = "";
        public PAccounts(string name)
        {
            InitializeComponent();
            Initalizing(name);
            UsersAz();
            nameEx = name;
        }

        private void UsersAz() {

            if (!File.Exists("users.txt")) {
                listBox.Items.Add("No users found.");
            }
            else
            {
                try
                {
                    List<string> accounts = new List<string>(); 
                    string[] lines = File.ReadAllLines("users.txt");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 4)
                        {
                            string username = parts[0];
                            string email = parts[1];
                            string role = parts[3];
                            accounts.Add($"Username: {username} - Email: {email} - Role: {role}");
                        }
                    }
                    accounts.Sort();
                    foreach (string account in accounts)
                    {
                        listBox.Items.Add(account);
                    }
                }
                catch (Exception)
                {
                    listBox.Items.Add("Error reading users file.");
                    throw;
                }
                
            }
        }

        private void UsersZA()
        {
            if (!File.Exists("users.txt"))
            {
                listBox.Items.Add("No users found.");
                MessageBox.Show("No users found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    List<string> accounts = new List<string>();
                    string[] lines = File.ReadAllLines("users.txt");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 4)
                        {
                            string username = parts[0];
                            string email = parts[1];
                            string role = parts[3];
                            accounts.Add($"Username: {username} - Email: {email} - Role: {role}");
                        }
                    }
                    accounts.Sort();
                    accounts.Reverse();
                    foreach (string account in accounts)
                    {
                        listBox.Items.Add(account);
                    }
                }
                catch (Exception)
                {
                    listBox.Items.Add("Error reading users file.");
                    MessageBox.Show("Error reading users file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
        }

        private void Initalizing(string name) {
            if (!File.Exists("users.txt"))
            {
                MessageBox.Show("No users found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    string[] lines = File.ReadAllLines("users.txt");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 4)
                        {
                            string username = parts[0];
                            string role = parts[3];
                            if (name == username)
                            {
                                if (role == "Admin")
                                {
                                    UsersAdd.IsEnabled = true;
                                    UsersAdd.Visibility = Visibility.Visible;
                                    UsersUpdate.IsEnabled = true;
                                    UsersUpdate.Visibility = Visibility.Visible;
                                    UsersDelete.IsEnabled = true;
                                    UsersDelete.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    UsersAdd.IsEnabled = false;
                                    UsersAdd.Visibility = Visibility.Collapsed;
                                    UsersUpdate.IsEnabled = false;
                                    UsersUpdate.Visibility = Visibility.Collapsed;
                                    UsersDelete.IsEnabled = false;
                                    UsersDelete.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error reading users file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

            }
        }



        private void UsersOrder_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.Items.Count > 0)
            {
                listBox.Items.Clear();
            }
            if (UsersOrderABC.Content.ToString() == "Felhasználók rendezése A-Z szerint")
            {
                UsersOrderABC.Content = "Felhasználók rendezése Z-A szerint";
                UsersAz();
            }
            else
            {
                UsersOrderABC.Content = "Felhasználók rendezése A-Z szerint";
                UsersZA();
            }
        }

        private void UsersAdd_Click(object sender, RoutedEventArgs e)
        {
            PAccountsAdd pAccountsAdd = new PAccountsAdd(nameEx);
            this.NavigationService.Navigate(pAccountsAdd);
        }

        private void UsersUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("Kérlek válassz ki egy felhasználót!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PAccountsUpdate pAccountsUpdate = new PAccountsUpdate(nameEx, listBox.SelectedItem.ToString());
            this.NavigationService.Navigate(pAccountsUpdate);
        }

        private void UsersDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("Kérlek válassz ki egy felhasználót!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<string> lines = File.ReadAllLines("users.txt").ToList();
            string selectedUser = listBox.SelectedItem.ToString();
            string username = selectedUser.Split('-')[0].Split(':')[1].Trim();
            string role = selectedUser.Split('-')[2].Split(':')[1].Trim();
            if (role == "Admin")
            {
                MessageBox.Show("Admin felhasználó nem törölhető!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                string exUsername = parts[0];
                if (exUsername == username)
                {
                    lines.Remove(line);
                    File.WriteAllLines("users.txt", lines);
                    MessageBox.Show("Felhasználó törölve!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    listBox.Items.Clear();
                    UsersAz();
                    return;
                }
            }
        }
    }
}
