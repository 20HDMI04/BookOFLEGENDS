using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace BookOFLEGENDS
{
   
    
    public partial class PBooksAdd : Page
    {
        public string connStr = "server=localhost;user=root;database=books;port=3306;password=root";
        public string usernameEx = ""; 
        public PBooksAdd(string username)
        {
            InitializeComponent();
            usernameEx = username;
        }

            
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }

        private void BooksBack(object sender, RoutedEventArgs e)
        {
            PBooks pBooks = new PBooks(usernameEx);
            this.NavigationService.Navigate(pBooks);
        }

        private bool Validate(string Author, string BookName, string PageCount) 
        {
            if (string.IsNullOrWhiteSpace(Author) || string.IsNullOrWhiteSpace(BookName) || string.IsNullOrWhiteSpace(PageCount))
            {
                MessageBox.Show("Kérlek töltsd ki az összes mezőt!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(PageCount, @"^\d+$"))
            {
                MessageBox.Show("A lapok számának egész számnak kell lennie!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(BookName, @"^\d+$"))
            {
                MessageBox.Show("A könyv címe nem lehet csak szám!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Author, @"^\d+$"))
            {
                MessageBox.Show("A könyv szerzője nem lehet csak szám!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int pages = int.Parse(PageCount);
            if (pages <= 0)
            {
                MessageBox.Show("A lapok számának nagyobbnak kell lennie mint nulla!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void BooksAdd_Click(object sender, RoutedEventArgs e)
        {
            string Author = AuthorBox.Text;
            string BookName = BookNameBox.Text;
            string PageCount = PageCountBox.Text;
            if(!Validate(Author, BookName, PageCount)) return;
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"INSERT INTO books.Books (Id, Author, Title, PageCount) VALUES({0}, '{Author}', '{BookName}', {PageCount})", conn);
                    var reader = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                MessageBox.Show("Sikeresen hozzáadtad a könyvet!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PBooks pBooks = new PBooks(usernameEx);
                this.NavigationService.Navigate(pBooks);
            }
            catch (Exception)
            {
                MessageBox.Show("Hiba történt az adatok betöltése során.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
            return;
        }
    }
}
