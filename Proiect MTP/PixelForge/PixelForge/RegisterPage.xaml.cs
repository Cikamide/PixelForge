using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace PixelForge
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
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string parola = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(parola))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "Data Source=DataBase.db";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                
                string checkQuery = "SELECT COUNT(*) FROM Utilizator WHERE Username = @Username";
                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Utilizatorul există deja!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                
                string insertQuery = "INSERT INTO Utilizator (Username, Parola, Tip) VALUES (@Username, @Parola, 'CLIENT')";
                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Parola", parola);
                    insertCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Înregistrare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();

            
            Window.GetWindow(this)?.Close();
        }
    }
}
