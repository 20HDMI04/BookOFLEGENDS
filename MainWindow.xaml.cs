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
using System.Windows.Threading;

namespace BookOFLEGENDS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            time.Text = TimeSpan.FromTicks(DateTime.Now.Ticks).ToString(@"hh\:mm\:ss");
            ElapsedTime = TimeSpan.FromTicks(DateTime.Now.Ticks);
            StopperTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            StopperTimer.Tick += StopperTimer_Tick;
            StopperTimer.Start();
        }

        private DispatcherTimer StopperTimer;
        private TimeSpan ElapsedTime; 
        
        private void StopperTimer_Tick(object sender, EventArgs e)
        {
            ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
            time.Text = ElapsedTime.ToString(@"hh\:mm\:ss");
        }

        public enum Role
        {
            Admin,
            User
        }

        private void registerOpenButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage registerWindow = new RegisterPage();
            registerWindow.Show();
            StopperTimer.Stop();
            this.Close();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text == "" || passwordBox.Password == "")
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string username = usernameBox.Text;
            string password = passwordBox.Password;

            if (!File.Exists("users.txt"))
            {
                MessageBox.Show("Az adatbázisban még nincs egy felhasználó sem!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else {
                try
                {
                    using (StreamReader r = new StreamReader("users.txt"))
                    {
                        
                        string[] lines = File.ReadAllLines("users.txt");
                        foreach (string line in lines) {
                            Users oneUser = Users.FromString(line);
                            if (oneUser.Username == username && oneUser.Password == password)
                            {
                                BookOfData bookOfData = new BookOfData(username);
                                bookOfData.Show();
                                StopperTimer.Stop();
                                this.Close();
                                return;
                            }
                        }
                    }   
                    MessageBox.Show($"Nincs ilyen felhasználó név és jelszó!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show($"Hiba történt az  adatbázis beolvasása közben!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
        }
    }
}
