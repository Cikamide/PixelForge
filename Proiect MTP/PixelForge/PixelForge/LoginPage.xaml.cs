using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dapper;
using System.Data.SQLite;
using System.Data;

namespace PixelForge
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DatabaseInit.Initializare();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string parola = PasswordBox.Password;
            string tip = ((ComboBoxItem)TipComboBox.SelectedItem)?.Content.ToString(); //null conditional

            if (TipComboBox.SelectedItem == null)
            {
                MessageBox.Show("Selectează tipul de utilizator!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(parola) || string.IsNullOrWhiteSpace(tip))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "Data Source=DataBase.db";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Utilizator WHERE Username = @Username AND Parola = @Parola AND Tip = @Tip";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    cmd.Parameters.AddWithValue("@Tip", tip);

                    long count = (long)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Autentificat cu succes", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (tip == "STAFF")
                        {
                            StaffPage staffWindow = new StaffPage();
                            staffWindow.Show();
                            this.Close();
                        }
                        else if (tip == "CLIENT")
                        {
                            ClientPage clientWindow = new ClientPage();
                            clientWindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Utilizatorul NU există!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage registerWindow = new RegisterPage();
            registerWindow.Show();
            this.Close(); 
        }
    }
}