using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BookOFLEGENDS
{
    /// <summary>
    /// Interaction logic for BookOfData.xaml
    /// </summary>
    public partial class BookOfData : Window
    {
        public string nameEx = ""; 
        public BookOfData(string name)
        {
            InitializeComponent();
            MainFrame.Navigate(new PAccounts(name));
            nameEx = name;
        }

        private void BooksList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PBooks(nameEx));
        }

        private void AllUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PAccounts(nameEx));
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
