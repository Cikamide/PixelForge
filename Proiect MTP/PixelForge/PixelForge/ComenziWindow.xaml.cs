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
using System.Windows.Shapes;

namespace PixelForge
{
    /// <summary>
    /// Interaction logic for ComenziWindow.xaml
    /// </summary>
    public partial class ComenziWindow : Window
    {
        public ComenziWindow()
        {
            InitializeComponent();
            IncarcaComenzi();
        }
        private void IncarcaComenzi(string numeClient = "")
        {
            ComenziListPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                // Query cu filtrare opțională după client (LIKE + % pentru căutare parțială)
                string query = "SELECT * FROM Comenzi";
                if (!string.IsNullOrEmpty(numeClient))
                {
                    query += " WHERE Client LIKE @client";
                }

                using (var command = new SQLiteCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(numeClient))
                    {
                        command.Parameters.AddWithValue("@client", "%" + numeClient + "%");
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        bool hasResults = false;

                        while (reader.Read())
                        {
                            hasResults = true;
                            int comandaId = Convert.ToInt32(reader["Id"]);

                            Border card = new Border
                            {
                                Background = Brushes.White,
                                CornerRadius = new CornerRadius(10),
                                Padding = new Thickness(15),
                                Margin = new Thickness(10),
                                Width = 600
                            };

                            StackPanel panel = new StackPanel();

                            panel.Children.Add(new TextBlock
                            {
                                Text = $"ID Comandă: {comandaId}",
                                FontWeight = FontWeights.Bold,
                                FontSize = 14
                            });

                            panel.Children.Add(new TextBlock
                            {
                                Text = $"Client: {reader["Client"]}",
                                FontSize = 16,
                                FontWeight = FontWeights.Bold,
                                Margin = new Thickness(0, 5, 0, 10)
                            });

                            panel.Children.Add(new TextBlock { Text = $"Procesor: {reader["Procesor"]}" });
                            panel.Children.Add(new TextBlock { Text = $"Cooler: {reader["Cooler"]}" });
                            panel.Children.Add(new TextBlock { Text = $"Placă de bază: {reader["MB"]}" });
                            panel.Children.Add(new TextBlock { Text = $"RAM: {reader["Rami"]}" });
                            panel.Children.Add(new TextBlock { Text = $"Stocare: {reader["Stocare"]}" });
                            panel.Children.Add(new TextBlock { Text = $"GPU: {reader["GPU"]}" });
                            panel.Children.Add(new TextBlock { Text = $"Carcasă: {reader["Carcasa"]}" });
                            panel.Children.Add(new TextBlock { Text = $"Sursă: {reader["PSU"]}" });

                            bool validat = Convert.ToBoolean(reader["Validat"]);
                            panel.Children.Add(new TextBlock
                            {
                                Text = $"Validat: {(validat ? "Da" : "Nu")}",
                                Margin = new Thickness(0, 10, 0, 0),
                                FontWeight = FontWeights.SemiBold
                            });

                            panel.Children.Add(new TextBlock
                            {
                                Text = $"Preț Total: {reader["PretTotal"]} RON",
                                FontWeight = FontWeights.Bold,
                                FontSize = 16,
                                Margin = new Thickness(0, 5, 0, 0)
                            });
                            StackPanel buttonsPanel = new StackPanel
                            {
                                Orientation = Orientation.Horizontal,
                                HorizontalAlignment = HorizontalAlignment.Right,
                                Margin = new Thickness(0, 10, 0, 0)
                            };

                            Button stergeButton = new Button
                            {
                                Content = "Șterge",
                                Width = 70,               
                                Height = 30,
                                Background = Brushes.Gray, 
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                Margin = new Thickness(5, 0, 5, 0),
                                HorizontalAlignment = HorizontalAlignment.Right,
                                Cursor = Cursors.Hand,
                                Tag = comandaId  // ID-ul comenzii pentru identificare
                            };
                            stergeButton.Click += StergeComanda_Click;

                            panel.Children.Add(stergeButton);
                            Button valideazaButton = new Button
                            {
                                Content = "Validează",
                                Margin = new Thickness(5, 0, 5, 0),
                                Tag = comandaId,
                                Background = Brushes.Gray,
                                Foreground = Brushes.White,
                                Cursor = validat ? Cursors.Arrow : Cursors.Hand,
                                FontWeight = FontWeights.Bold,
                                Width = 75,
                                Height = 30,
                                IsEnabled = !validat
                            };
                            valideazaButton.Click += ValideazaComanda_Click;
                            panel.Children.Add(valideazaButton);

                            card.Child = panel;
                            ComenziListPanel.Children.Add(card);
                        }

                        if (!hasResults)
                        {
                            ComenziListPanel.Children.Add(new TextBlock
                            {
                                Text = "Nu există comenzi în baza de date.",
                                Foreground = Brushes.White,
                                FontSize = 18,
                                FontWeight = FontWeights.SemiBold,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Margin = new Thickness(20)
                            });
                        }
                    }
                }
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StaffPage staffPage = new StaffPage();
            staffPage.Show();
            this.Close();
        }

        // Butonul de cautare
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string textCautare = SearchTextBox.Text.Trim();
            IncarcaComenzi(textCautare);
        }
        private void StergeComanda_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                try
                {
                    using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM Comenzi WHERE Id = @id";
                        using (var command = new SQLiteCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Comanda a fost ștearsă.", "Șters", MessageBoxButton.OK, MessageBoxImage.Information);
                    IncarcaComenzi(SearchTextBox.Text.Trim()); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la ștergere: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ValideazaComanda_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int comandaId = (int)btn.Tag;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string updateQuery = "UPDATE Comenzi SET Validat = 1 WHERE Id = @id"; 
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", comandaId);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Comanda a fost validată.");

            // Reîncarcă lista pentru a reflecta schimbarea
            IncarcaComenzi(SearchTextBox.Text.Trim());
        }


    }
}
