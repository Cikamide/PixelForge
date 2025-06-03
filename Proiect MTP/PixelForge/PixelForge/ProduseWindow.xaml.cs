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
    /// Interaction logic for ProduseWindow.xaml
    /// </summary>
    public partial class ProduseWindow : Window
    {
        public ProduseWindow()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StaffPage loginWindow = new StaffPage();
            loginWindow.Show();
            this.Close();
        }
        private void AdaugaButton_Click(object sender, RoutedEventArgs e)
        {
            AdaugareComponenta fereastraAdaugare = new AdaugareComponenta();
            fereastraAdaugare.Show();
            this.Close();
        }

        private void CpuButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Procesor";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock { Text = $"Socket: {reader["Socket"]}" });
                        panel.Children.Add(new TextBlock { Text = $"Frecvență: {reader["FrecventaGhz"]} GHz" });

                        bool coolerInclus = Convert.ToBoolean(reader["CoolerInclus"]);
                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Cooler inclus: {(coolerInclus ? "Da" : "Nu")}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareProcesor(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("Procesor", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);

                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există produse de tip CPU în bază de date.",
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

        private void CoolerButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Cooler";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        bool aio = Convert.ToBoolean(reader["AIO"]);
                        panel.Children.Add(new TextBlock
                        {
                            Text = $"AIO: {(aio ? "Da" : "Nu")}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"RPM: {reader["RPM"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareCooler(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            
                            Tag = new Tuple<string, int>("Cooler", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);

                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există coolere în baza de date.",
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

        private void MbButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM PlacaDeBaza";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock { Text = $"Socket: {reader["Socket"]}" });
                        panel.Children.Add(new TextBlock { Text = $"Memorie: {reader["Memorie"]} GB" });
                        panel.Children.Add(new TextBlock { Text = $"Sloturi RAM: {reader["SloturiRam"]}" });
                        panel.Children.Add(new TextBlock { Text = $"Sloturi M.2: {reader["SloturiM2"]}" });
                        panel.Children.Add(new TextBlock { Text = $"Preț: {reader["Pret"]} RON" });

                        
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareMb(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("PlacaDeBaza", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);
                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există plăci de bază în baza de date.",
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

        private void RamiButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Rami";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock { Text = $"Viteză: {reader["Viteza"]} MHz" });
                        panel.Children.Add(new TextBlock { Text = $"Capacitate totală: {reader["Capacitate"]} GB" });
                        panel.Children.Add(new TextBlock { Text = $"Număr module: {reader["NrModule"]}" });
                        panel.Children.Add(new TextBlock { Text = $"Preț: {reader["Pret"]} RON" });

                        
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareRami(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("Rami", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);
                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există module RAM în baza de date.",
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

        private void StocareButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Stocare";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock { Text = $"Capacitate: {reader["Capacitate"]} GB" });
                        panel.Children.Add(new TextBlock { Text = $"Tip: {reader["Tip"]}" });
                        panel.Children.Add(new TextBlock { Text = $"Preț: {reader["Pret"]} RON" });

                      
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareStocare(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("Stocare", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);
                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există unități de stocare în baza de date.",
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


        private void GpuButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM GPU";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock { Text = $"Memorie: {reader["MemorieGB"]} GB" });
                        panel.Children.Add(new TextBlock { Text = $"Preț: {reader["Pret"]} RON" });

                        
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareGpu(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("GPU", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);
                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există plăci video în baza de date.",
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


        private void CaseButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Carcasa";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock { Text = $"Volum: {reader["Volum"]} L" });
                        panel.Children.Add(new TextBlock { Text = $"Preț: {reader["Pret"]} RON" });

                       
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditareCase(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("Carcasa", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);

                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există carcase în baza de date.",
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


        private void PsuButton_Click(object sender, RoutedEventArgs e)
        {
            ProductListPanel.Children.Clear(); 

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Sursa";
                using (var command = new SQLiteCommand(query, connection))
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
                            Width = 500
                        };

                        StackPanel panel = new StackPanel();

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"ID: {reader["Id"]}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 14
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nume: {reader["Nume"]}",
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Eficiență: {reader["Eficienta"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Putere: {reader["PutereW"]} W"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        
                        StackPanel buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Button modificaButton = new Button
                        {
                            Content = "Modifică",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = Convert.ToInt32(reader["Id"])
                        };
                        modificaButton.Click += (s, e) =>
                        {
                            int id = (int)((Button)s).Tag;
                            EditarePsu(id);
                        };

                        Button stergeButton = new Button
                        {
                            Content = "Șterge",
                            Margin = new Thickness(5, 0, 5, 0),
                            Tag = new Tuple<string, int>("Sursa", Convert.ToInt32(reader["Id"]))
                        };
                        stergeButton.Click += StergeButton_Click;

                        buttonsPanel.Children.Add(modificaButton);
                        buttonsPanel.Children.Add(stergeButton);

                        panel.Children.Add(buttonsPanel);

                        card.Child = panel;
                        ProductListPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ProductListPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există surse de alimentare în baza de date.",
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
        private void StergeButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag is Tuple<string, int> info)
            {
                string tabela = info.Item1;
                int id = info.Item2;

                using var connection = new SQLiteConnection("Data Source=DataBase.db");
                connection.Open();

                string query = $"DELETE FROM {tabela} WHERE Id = @id";

                using var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Elementul cu Id {id} din tabela {tabela} a fost șters cu succes.", "Ștergere reușită", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ștergerea nu a reușit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                switch (tabela)
                {
                    case "Procesor":
                        CpuButton_Click(null, null);
                        break;
                    case "Cooler":
                        CoolerButton_Click(null, null);
                        break;
                    case "PlacaDeBaza":
                        MbButton_Click(null, null);
                        break;
                    case "Rami":
                        RamiButton_Click(null, null);
                        break;
                    case "Stocare":
                        StocareButton_Click(null, null);
                        break;
                    case "GPU":
                        GpuButton_Click(null, null);
                        break;
                    case "Carcasa":
                        CaseButton_Click(null, null);
                        break;
                    case "Sursa":
                        PsuButton_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
        }
        private void EditareProcesor(int idProcesor)
        {
            FormPanel.Children.Clear();
            string nume = "", socket = "";
            double frecventa = 0, pret = 0;
            bool coolerInclus = true;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Procesor WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idProcesor);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    socket = reader["Socket"].ToString();
                    frecventa = Convert.ToDouble(reader["FrecventaGhz"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                    coolerInclus = Convert.ToBoolean(reader["CoolerInclus"]);
                }
            }

           
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };

            
            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock { Text = "Socket:", FontWeight = FontWeights.Bold });
            TextBox txtSocket = new TextBox { Text = socket, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSocket);

            
            panel.Children.Add(new TextBlock { Text = "Frecvență (GHz):", FontWeight = FontWeights.Bold });
            TextBox txtFrec = new TextBox { Text = frecventa.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtFrec);

            
            panel.Children.Add(new TextBlock { Text = "Cooler inclus:", FontWeight = FontWeights.Bold });
            ComboBox cmbCooler = new ComboBox { Margin = new Thickness(0, 0, 0, 10) };
            cmbCooler.Items.Add("Da");
            cmbCooler.Items.Add("Nu");
            cmbCooler.SelectedIndex = coolerInclus ? 0 : 1;
            panel.Children.Add(cmbCooler);

            
            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
                UPDATE Procesor SET
                    Nume = @nume,
                    Socket = @socket,
                    FrecventaGhz = @frecventa,
                    CoolerInclus = @cooler,
                    Pret = @pret
                WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@socket", txtSocket.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@frecventa", Convert.ToDouble(txtFrec.Text));
                    updateCmd.Parameters.AddWithValue("@cooler", cmbCooler.SelectedItem.ToString() == "Da");
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idProcesor);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        CpuButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditareCooler(int idCooler)
        {
            FormPanel.Children.Clear();
            string nume = "";
            bool aio = false;
            int rpm = 0;
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Cooler WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idCooler);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    aio = Convert.ToBoolean(reader["AIO"]);
                    rpm = Convert.ToInt32(reader["RPM"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }

            
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };

            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            panel.Children.Add(new TextBlock { Text = "AIO:", FontWeight = FontWeights.Bold });
            ComboBox cmbAIO = new ComboBox { Margin = new Thickness(0, 0, 0, 10) };
            cmbAIO.Items.Add("Da");
            cmbAIO.Items.Add("Nu");
            cmbAIO.SelectedIndex = aio ? 0 : 1;
            panel.Children.Add(cmbAIO);


            panel.Children.Add(new TextBlock { Text = "RPM:", FontWeight = FontWeights.Bold });
            TextBox txtRPM = new TextBox { Text = rpm.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtRPM);

            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);


            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE Cooler SET
                Nume = @nume,
                AIO = @aio,
                RPM = @rpm,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@aio", cmbAIO.SelectedItem.ToString() == "Da");
                    updateCmd.Parameters.AddWithValue("@rpm", Convert.ToInt32(txtRPM.Text));
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idCooler);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        CoolerButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditareMb(int idPlaca)
        {
            FormPanel.Children.Clear();
            string nume = "";
            string socket = "";
            int memorie = 0;
            int sloturiRam = 0;
            int sloturiM2 = 0;
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM PlacaDeBaza WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idPlaca);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    socket = reader["Socket"].ToString();
                    memorie = Convert.ToInt32(reader["Memorie"]);
                    sloturiRam = Convert.ToInt32(reader["SloturiRam"]);
                    sloturiM2 = Convert.ToInt32(reader["SloturiM2"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }


            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };


            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);


            panel.Children.Add(new TextBlock { Text = "Socket:", FontWeight = FontWeights.Bold });
            TextBox txtSocket = new TextBox { Text = socket, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSocket);


            panel.Children.Add(new TextBlock { Text = "Memorie (GB):", FontWeight = FontWeights.Bold });
            TextBox txtMemorie = new TextBox { Text = memorie.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtMemorie);


            panel.Children.Add(new TextBlock { Text = "Sloturi RAM:", FontWeight = FontWeights.Bold });
            TextBox txtSloturiRam = new TextBox { Text = sloturiRam.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSloturiRam);


            panel.Children.Add(new TextBlock { Text = "Sloturi M.2:", FontWeight = FontWeights.Bold });
            TextBox txtSloturiM2 = new TextBox { Text = sloturiM2.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSloturiM2);


            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);


            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE PlacaDeBaza SET
                Nume = @nume,
                Socket = @socket,
                Memorie = @memorie,
                SloturiRam = @sloturiRam,
                SloturiM2 = @sloturiM2,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@socket", txtSocket.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@memorie", Convert.ToInt32(txtMemorie.Text));
                    updateCmd.Parameters.AddWithValue("@sloturiRam", Convert.ToInt32(txtSloturiRam.Text));
                    updateCmd.Parameters.AddWithValue("@sloturiM2", Convert.ToInt32(txtSloturiM2.Text));
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idPlaca);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        MbButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditareRami(int idRami)
        {
            FormPanel.Children.Clear();
            string nume = "";
            int viteza = 0;
            int capacitate = 0;
            int nrModule = 0;
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Rami WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idRami);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    viteza = Convert.ToInt32(reader["Viteza"]);
                    capacitate = Convert.ToInt32(reader["Capacitate"]);
                    nrModule = Convert.ToInt32(reader["NrModule"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }


            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };


            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);


            panel.Children.Add(new TextBlock { Text = "Viteza (MHz):", FontWeight = FontWeights.Bold });
            TextBox txtViteza = new TextBox { Text = viteza.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtViteza);


            panel.Children.Add(new TextBlock { Text = "Capacitate (GB):", FontWeight = FontWeights.Bold });
            TextBox txtCapacitate = new TextBox { Text = capacitate.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtCapacitate);


            panel.Children.Add(new TextBlock { Text = "Număr module:", FontWeight = FontWeights.Bold });
            TextBox txtNrModule = new TextBox { Text = nrModule.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNrModule);

            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE Rami SET
                Nume = @nume,
                Viteza = @viteza,
                Capacitate = @capacitate,
                NrModule = @nrModule,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@viteza", Convert.ToInt32(txtViteza.Text));
                    updateCmd.Parameters.AddWithValue("@capacitate", Convert.ToInt32(txtCapacitate.Text));
                    updateCmd.Parameters.AddWithValue("@nrModule", Convert.ToInt32(txtNrModule.Text));
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idRami);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        RamiButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditareStocare(int idStocare)
        {
            FormPanel.Children.Clear();
            string nume = "";
            int capacitate = 0;
            string tip = "";
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Stocare WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idStocare);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    capacitate = Convert.ToInt32(reader["Capacitate"]);
                    tip = reader["Tip"].ToString();
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }

            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };


            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);


            panel.Children.Add(new TextBlock { Text = "Capacitate (GB):", FontWeight = FontWeights.Bold });
            TextBox txtCapacitate = new TextBox { Text = capacitate.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtCapacitate);


            panel.Children.Add(new TextBlock { Text = "Tip:", FontWeight = FontWeights.Bold });
            TextBox txtTip = new TextBox { Text = tip, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtTip);


            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);


            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE Stocare SET
                Nume = @nume,
                Capacitate = @capacitate,
                Tip = @tip,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@capacitate", Convert.ToInt32(txtCapacitate.Text));
                    updateCmd.Parameters.AddWithValue("@tip", txtTip.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idStocare);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        StocareButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditareGpu(int idGpu)
        {
            FormPanel.Children.Clear();
            string nume = "";
            int memorieGb = 0;
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM GPU WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idGpu);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    memorieGb = Convert.ToInt32(reader["MemorieGB"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }

            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };


            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);


            panel.Children.Add(new TextBlock { Text = "Memorie (GB):", FontWeight = FontWeights.Bold });
            TextBox txtMemorie = new TextBox { Text = memorieGb.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtMemorie);


            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);


            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE GPU SET
                Nume = @nume,
                MemorieGB = @memorie,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@memorie", Convert.ToInt32(txtMemorie.Text));
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idGpu);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        GpuButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditareCase(int idCarcasa)
        {
            FormPanel.Children.Clear();
            string nume = "";
            double volum = 0;
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Carcasa WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idCarcasa);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    volum = Convert.ToDouble(reader["Volum"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }

            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };


            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            panel.Children.Add(new TextBlock { Text = "Volum (litri):", FontWeight = FontWeights.Bold });
            TextBox txtVolum = new TextBox { Text = volum.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtVolum);

            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);


            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE Carcasa SET
                Nume = @nume,
                Volum = @volum,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@volum", Convert.ToDouble(txtVolum.Text));
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idCarcasa);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        CaseButton_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void EditarePsu(int idSursa)
        {
            FormPanel.Children.Clear();
            string nume = "";
            string eficienta = "";
            int putereW = 0;
            double pret = 0;

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Sursa WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idSursa);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nume = reader["Nume"].ToString();
                    eficienta = reader["Eficienta"].ToString();
                    putereW = Convert.ToInt32(reader["PutereW"]);
                    pret = Convert.ToDouble(reader["Pret"]);
                }
            }

            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };


            panel.Children.Add(new TextBlock { Text = "Nume:", FontWeight = FontWeights.Bold });
            TextBox txtNume = new TextBox { Text = nume, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);


            panel.Children.Add(new TextBlock { Text = "Eficiență:", FontWeight = FontWeights.Bold });
            TextBox txtEficienta = new TextBox { Text = eficienta, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtEficienta);


            panel.Children.Add(new TextBlock { Text = "Putere (W):", FontWeight = FontWeights.Bold });
            TextBox txtPutere = new TextBox { Text = putereW.ToString(), Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtPutere);


            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Text = pret.ToString(), Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);


            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, e) =>
            {
                try
                {
                    using var conn = new SQLiteConnection("Data Source=DataBase.db");
                    conn.Open();

                    string updateQuery = @"
            UPDATE Sursa SET
                Nume = @nume,
                Eficienta = @eficienta,
                PutereW = @putere,
                Pret = @pret
            WHERE Id = @id;";

                    using var updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@nume", txtNume.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@eficienta", txtEficienta.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@putere", Convert.ToInt32(txtPutere.Text));
                    updateCmd.Parameters.AddWithValue("@pret", Convert.ToDouble(txtPret.Text));
                    updateCmd.Parameters.AddWithValue("@id", idSursa);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Modificare realizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        FormPanel.Children.Clear();
                        PsuButton_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a putut modifica componenta.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }



    }
}
