using MySql.Data.MySqlClient;
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

namespace BookOFLEGENDS
{
    /// <summary>
    /// Interaction logic for PBooksUpdate.xaml
    /// </summary>
    public partial class PBooksUpdate : Page
    {
        public string usernameEx = "";
        public int bookIDEx = 0;
        public string connStr = "server=localhost;user=root;database=books;port=3306;password=root";
        public PBooksUpdate(string username, int bookID)
        {
            InitializeComponent();
            usernameEx = username;
            bookIDEx = bookID;
            Initalize();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Initalize() 
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"SELECT * FROM books.Books WHERE Id = {bookIDEx}", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        BookNameBox.Text = reader["Title"].ToString();
                        AuthorBox.Text = reader["Author"].ToString();
                        PageCountBox.Text = reader["PageCount"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hiba történt az adatok betöltése során.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void BooksBack(object sender, RoutedEventArgs e)
        {
            PBooks books = new PBooks(usernameEx);
            NavigationService.Navigate(books);
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

        private void BooksUpdate_Click(object sender, RoutedEventArgs e)
        {
            string author = AuthorBox.Text;
            string bookName = BookNameBox.Text;
            string pageCount = PageCountBox.Text;
            if (!Validate(author, bookName, pageCount)) return;
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"UPDATE books.Books SET Author = '{author}', Title = '{bookName}', PageCount = {pageCount} WHERE Id = {bookIDEx}", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                MessageBox.Show("Sikeresen frissítetted a könyvet!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PBooks books = new PBooks(usernameEx);
                NavigationService.Navigate(books);
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
