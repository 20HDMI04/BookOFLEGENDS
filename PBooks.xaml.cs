using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for PBooks.xaml
    /// </summary>
    public partial class PBooks : Page
    {
        public string nameEx = "";
        public string connStr = "server=localhost;user=root;database=books;port=3306;password=root";
        List<string> booksEx = new List<string>();
        public PBooks(string username)
        {
            InitializeComponent();
            nameEx = username;
            Page_Loaded();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Page_Loaded()
        {
            listBox.Items.Clear();
            List<string> books = new List<string>();
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM books.Books", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string data = $"{reader["Id"]};{reader["Author"]};{reader["Title"]};{reader["PageCount"]}";
                        books.Add(data);
                    }
                    conn.Close();
                }

                books.Sort(delegate (string s1, string s2)
                {
                    return s1.Split(';')[2].Trim().ToString().CompareTo(s2.Split(';')[2].Trim().ToString());
                });
                booksEx = books;
                foreach (string book in books)
                {
                    string[] data = book.Split(';');
                    listBox.Items.Add($"ID: {data[0]} - Author: {data[1]} - Title: {data[2]} - Pages: {data[3]}");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hiba történt az adatok betöltése során.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                listBox.Items.Add("Error loading data.");
                throw;
            }
            
        }

        private void BooksOrder_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.Items.Count > 0)
            {
                listBox.Items.Clear();
            }
            if (BooksOrderABC.Content.ToString() == "Könyvek rendezése A-Z szerint")
            {
                BooksOrderABC.Content = "Könyvek rendezése Z-A szerint";
                PageAZ();
            }
            else
            {
                BooksOrderABC.Content = "Könyvek rendezése A-Z szerint";
                PageZA();
            }
        }

        private void PageZA()
        {
            listBox.Items.Clear();

            var res = booksEx.Select(s => s).OrderByDescending(s => s.Split(';')[2].Trim().ToString()).ToList();

            foreach (string book in res)
            {
                string[] data = book.Split(';');
                listBox.Items.Add($"ID: {data[0]} - Author: {data[1]} - Title: {data[2]} - Pages: {data[3]}");
            }
        }

        private void PageAZ()
        {
            listBox.Items.Clear();

            var res = booksEx.Select(s => s).OrderBy(s => s.Split(';')[2].Trim().ToString()).ToList();

            foreach (string book in res)
            {
                string[] data = book.Split(';');
                listBox.Items.Add($"ID: {data[0]} - Author: {data[1]} - Title: {data[2]} - Pages: {data[3]}");
            }
        }

        private void BooksAdd_Click(object sender, RoutedEventArgs e)
        {
            PBooksAdd pBooksAdd = new PBooksAdd(nameEx);
            this.NavigationService.Navigate(pBooksAdd);
            return;
        }

        private void BooksUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("Kérlek válassz ki egy könyvet!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PBooksUpdate pBooksUpdate = new PBooksUpdate(nameEx, int.Parse(listBox.SelectedItem.ToString().Split('-')[0].Split(':')[1].Trim()));
            this.NavigationService.Navigate(pBooksUpdate);
            return;
        }

        private void BooksDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("Kérlek válassz ki egy könyvet!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<string> lines = File.ReadAllLines("users.txt").ToList();
            List<string> selectedUser = lines.Where(x => x.Split(';')[0].Trim() == nameEx).ToList();
            string role = selectedUser[0].Split(';')[3].Trim();
            if (role != "Admin")
            {
                MessageBox.Show("Nincs jogosultságod a törléshez!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Biztosan törölni szeretnéd a könyvet?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                string selectedBookId = listBox.SelectedItem.ToString().Split('-')[0].Split(':')[1].Trim();
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"DELETE FROM books.Books WHERE id={selectedBookId}", conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sikeres törlés!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Page_Loaded();
                    conn.Close();
                }
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Hiba történt a törlés során.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
        }
    }
}
