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
    /// Interaction logic for ConturiWindow.xaml
    /// </summary>
    public partial class ConturiWindow : Window
    {
        public ConturiWindow()
        {
            InitializeComponent();
            ShowUsers();
        }
        private void AdaugaButton_Click(object sender, RoutedEventArgs e)
        {
            AddUser();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StaffPage staffPage = new StaffPage();
            staffPage.Show();
            this.Close();
        }
        private void ShowUsers()
        {
            ConturiPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Utilizator";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idUtilizator = Convert.ToInt32(reader["Id"]);

                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 300
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {idUtilizator}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Username: {reader["Username"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Parola: {reader["Parola"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Tip: {reader["Tip"]}"
                        });

                        // Container pentru butoane (poți adăuga și alte butoane aici pe viitor)
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Padding = new Thickness(6, 2, 6, 2),
                            MinWidth = 60,
                            FontSize = 12,
                            Tag = idUtilizator
                        };

                        stergeButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;

                            try
                            {
                                using var connDelete = new SQLiteConnection("Data Source=DataBase.db");
                                connDelete.Open();

                                string deleteQuery = "DELETE FROM Utilizator WHERE Id = @id";
                                using var deleteCmd = new SQLiteCommand(deleteQuery, connDelete);
                                deleteCmd.Parameters.AddWithValue("@id", id);
                                int rows = deleteCmd.ExecuteNonQuery();

                                if (rows > 0)
                                {
                                    MessageBox.Show("Utilizatorul a fost șters.", "Șters", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ShowUsers(); 
                                }
                                else
                                {
                                    MessageBox.Show("Ștergerea nu a reușit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Eroare la ștergere: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        };

                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);

                        card.Child = panel;
                        ConturiPanel.Children.Add(card);
                    }
                }
            }
        }
        private void AddUser()
        {
            FormPanel.Children.Clear();

            var card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            var panel = new StackPanel { Margin = new Thickness(5) };

            // Username
            panel.Children.Add(new TextBlock { Text = "Username:", FontWeight = FontWeights.Bold });
            var txtUsername = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtUsername);

            // Parola
            panel.Children.Add(new TextBlock { Text = "Parolă:", FontWeight = FontWeights.Bold });
            var txtParola = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtParola);

            // Tip
            panel.Children.Add(new TextBlock { Text = "Tip cont:", FontWeight = FontWeights.Bold });
            var cmbTip = new ComboBox { Margin = new Thickness(0, 0, 0, 20), Width = 150 };
            cmbTip.Items.Add("CLIENT");
            cmbTip.Items.Add("STAFF");
            cmbTip.SelectedIndex = 0;
            panel.Children.Add(cmbTip);

            // Buton salvare
            var btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                string username = txtUsername.Text.Trim();
                string parola = txtParola.Text.Trim();
                string tip = cmbTip.SelectedItem as string;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(parola))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string insert = @"INSERT INTO Utilizator (Username, Parola, Tip)
                                      VALUES (@u, @p, @t)";
                    using var cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", parola);
                    cmd.Parameters.AddWithValue("@t", tip);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Eroare la inserare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Cont adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowUsers(); // reîncarcă lista
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
    }
}
