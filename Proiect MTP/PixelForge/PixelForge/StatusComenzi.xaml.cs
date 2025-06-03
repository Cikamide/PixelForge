using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PixelForge
{
    public partial class StatusComenzi : Window
    {
        public StatusComenzi()
        {
            InitializeComponent();
            IncarcaComenzi();  // Încărcăm toate comenzile la început
        }

        // Metoda care încarcă comenzile din baza de date, eventual filtrate după numele clientului
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
                                Text = $"ID Comandă: {reader["Id"]}",
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
            ClientPage clientPage = new ClientPage();
            clientPage.Show();
            this.Close();
        }

        // Butonul de cautare
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string textCautare = SearchTextBox.Text.Trim();
            IncarcaComenzi(textCautare);
        }
    }
}
