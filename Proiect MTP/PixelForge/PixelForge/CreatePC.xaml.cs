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
    /// Interaction logic for CreatePC.xaml
    /// </summary>
    public partial class CreatePC : Window
    {
        public int total = 0;
        public double getPretTotal()
        {
            return total;
        }
        public CreatePC()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClientPage clientPage = new ClientPage();
            clientPage.Show();
            this.Close(); 
        }
        private void ComandaButton_Click(object sender, RoutedEventArgs e)
        {
            string numeClient = ClientNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(numeClient))
            {
                MessageBox.Show("Te rugăm să introduci numele clientului.", "Lipsă informație", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(CpuSelectedText.Text) ||
                string.IsNullOrEmpty(CoolerSelectedText.Text) ||
                string.IsNullOrEmpty(MbSelectedText.Text) ||
                string.IsNullOrEmpty(RamiSelectedText.Text) ||
                string.IsNullOrEmpty(StocareSelectedText.Text) ||
                string.IsNullOrEmpty(GpuSelectedText.Text) ||
                string.IsNullOrEmpty(CarcasaSelectedText.Text) ||
                string.IsNullOrEmpty(PsuSelectedText.Text))
            {
                MessageBox.Show("Te rugăm să selectezi toate componentele înainte de a plasa comanda.", "Completare necesară", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Comenzi 
                    (Client, Procesor, Cooler, MB, Rami, Stocare, GPU, Carcasa, PSU, Validat, PretTotal) 
                    VALUES 
                    (@client, @procesor, @cooler, @mb, @rami, @stocare, @gpu, @carcasa, @psu, @validat, @pretTotal)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@client", numeClient);
                    command.Parameters.AddWithValue("@procesor", CpuSelectedText.Text);
                    command.Parameters.AddWithValue("@cooler", CoolerSelectedText.Text);
                    command.Parameters.AddWithValue("@mb", MbSelectedText.Text);
                    command.Parameters.AddWithValue("@rami", RamiSelectedText.Text);
                    command.Parameters.AddWithValue("@stocare", StocareSelectedText.Text);
                    command.Parameters.AddWithValue("@gpu", GpuSelectedText.Text);
                    command.Parameters.AddWithValue("@carcasa", CarcasaSelectedText.Text);
                    command.Parameters.AddWithValue("@psu", PsuSelectedText.Text);
                    command.Parameters.AddWithValue("@validat", false);
                    command.Parameters.AddWithValue("@pretTotal", getPretTotal());

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Comanda a fost plasată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        //CPU
        private void CpuButton_Click(object obj, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

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
                            Width = 500,
                            Tag = reader["Id"]  // ID-ul procesorului
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

                        // Adaugă butoanele (opțional pentru drag and drop doar pe Border)
                        // ...

                        card.Child = panel;

                        // Drag and drop start
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idCpu = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idCpu.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }

                    if (!hasResults)
                    {
                        ComponentCardsPanel.Children.Add(new TextBlock
                        {
                            Text = "Nu există procesoare în baza de date.",
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
        private void CpuDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idCpu = int.Parse(idStr);

                var cpu = GetCpuById(idCpu);
                if (cpu != null)
                {
                    CpuDropZone.Tag = idCpu; 
                    CpuSelectedText.Text = cpu.Nume;
                    UpdatePretTotal();      
                }
            }
        }
        private CPU GetCpuById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Procesor WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CPU
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Socket = reader["Socket"].ToString(),
                                FrecventaGhz = Convert.ToDouble(reader["FrecventaGhz"]),
                                CoolerInclus = Convert.ToBoolean(reader["CoolerInclus"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }

            return null; // dacă nu găsește CPU-ul
        }

        //Cooler
        private void CoolerButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Cooler";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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
                            Text = $"AIO: {(Convert.ToBoolean(reader["AIO"]) ? "Da" : "Nu")}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"RPM: {reader["RPM"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idCooler = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idCooler.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void CoolerZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idCooler = int.Parse(idStr);

                var cooler = GetCoolerById(idCooler);
                if (cooler != null)
                {
                    CoolerSelectedText.Text = cooler.Nume;
                    CoolerDropZone.Tag = cooler.Id;
                    UpdatePretTotal();
                }
            }
        }
        private Cooler GetCoolerById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Cooler WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cooler
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                AIO = Convert.ToBoolean(reader["AIO"]),
                                RPM = Convert.ToInt32(reader["RPM"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        //Mb
        private void MbButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM PlacaDeBaza";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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
                            Text = $"Socket: {reader["Socket"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Memorie max: {reader["Memorie"]} GB"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Sloturi RAM: {reader["SloturiRam"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Sloturi M.2: {reader["SloturiM2"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idPlaca = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idPlaca.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void MbDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idPlaca = int.Parse(idStr);

                var placa = GetPlacaDeBazaById(idPlaca);
                if (placa != null)
                {
                    MbSelectedText.Text = placa.Nume;
                    MbDropZone.Tag = placa.Id;
                    UpdatePretTotal();
                }
            }
        }
        private PlacaDeBaza GetPlacaDeBazaById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM PlacaDeBaza WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PlacaDeBaza
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Socket = reader["Socket"].ToString(),
                                Memorie = Convert.ToInt32(reader["Memorie"]),
                                SloturiRam = Convert.ToInt32(reader["SloturiRam"]),
                                SloturiM2 = Convert.ToInt32(reader["SloturiM2"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }

            return null;
        }


        //Rami
        private void RamiButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Rami";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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
                            Text = $"Viteză: {reader["Viteza"]} MHz"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Capacitate: {reader["Capacitate"]} GB"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Nr Module: {reader["NrModule"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idRami = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idRami.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void RamiDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idRami = int.Parse(idStr);

                var rami = GetRamiById(idRami);
                if (rami != null)
                {
                    RamiSelectedText.Text = rami.Nume;
                    RamiDropZone.Tag = rami.Id;
                    UpdatePretTotal();
                }
            }
        }
        private Rami GetRamiById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Rami WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Rami
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Viteza = Convert.ToInt32(reader["Viteza"]),
                                Capacitate = Convert.ToInt32(reader["Capacitate"]),
                                NrModule = Convert.ToInt32(reader["NrModule"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }

            return null;
        }


        //Stocare
        private void StocareButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Stocare";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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
                            Text = $"Capacitate: {reader["Capacitate"]} GB"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Tip: {reader["Tip"]}"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idStocare = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idStocare.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void StocareDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idStocare = int.Parse(idStr);

                var stocare = GetStocareById(idStocare);
                if (stocare != null)
                {
                    StocareSelectedText.Text = stocare.Nume;
                    StocareDropZone.Tag = stocare.Id;
                    UpdatePretTotal();
                }
            }
        }
        private Stocare GetStocareById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Stocare WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Stocare
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Capacitate = Convert.ToInt32(reader["Capacitate"]),
                                Tip = reader["Tip"].ToString(),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }
            return null;
        }


        //GPU
        private void GpuButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM GPU";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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
                            Text = $"Memorie: {reader["MemorieGB"]} GB"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idGpu = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idGpu.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void GpuDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idGpu = int.Parse(idStr);

                var gpu = GetGpuById(idGpu);
                if (gpu != null)
                {
                    GpuSelectedText.Text = gpu.Nume;
                    GpuDropZone.Tag = gpu.Id;
                    UpdatePretTotal();
                }
            }
        }
        private GPU GetGpuById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM GPU WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new GPU
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                MemorieGB = Convert.ToInt32(reader["MemorieGB"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }
            return null;
        }


        //Case
        private void CaseButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Carcasa";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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
                            Text = $"Volum: {reader["Volum"]} litri"
                        });

                        panel.Children.Add(new TextBlock
                        {
                            Text = $"Preț: {reader["Pret"]} RON"
                        });

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idCarcasa = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idCarcasa.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void CaseDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idCarcasa = int.Parse(idStr);

                var carcasa = GetCarcasaById(idCarcasa);
                if (carcasa != null)
                {
                    CarcasaSelectedText.Text = carcasa.Nume;
                    CarcasaDropZone.Tag = carcasa.Id;
                    UpdatePretTotal();
                }
            }
        }
        private Carcasa GetCarcasaById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Carcasa WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Carcasa
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Volum = Convert.ToDouble(reader["Volum"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }
            return null;
        }


        //PSU
        private void PsuButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentCardsPanel.Children.Clear();

            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Sursa";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Border card = new Border
                        {
                            Background = Brushes.White,
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(15),
                            Margin = new Thickness(10),
                            Width = 500,
                            Tag = reader["Id"]
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

                        card.Child = panel;

                        // Drag and drop
                        card.MouseMove += (s, e) =>
                        {
                            if (e.LeftButton == MouseButtonState.Pressed)
                            {
                                var border = s as Border;
                                if (border != null)
                                {
                                    int idSursa = Convert.ToInt32(border.Tag);
                                    DragDrop.DoDragDrop(border, idSursa.ToString(), DragDropEffects.Copy);
                                }
                            }
                        };

                        ComponentCardsPanel.Children.Add(card);
                    }
                }
            }
        }
        private void PsuDropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string idStr = (string)e.Data.GetData(DataFormats.StringFormat);
                int idSursa = int.Parse(idStr);

                var sursa = GetSursaById(idSursa);
                if (sursa != null)
                {
                    PsuSelectedText.Text = sursa.Nume;
                    PsuDropZone.Tag = sursa.Id;
                    UpdatePretTotal();
                }
            }
        }
        private Sursa GetSursaById(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Sursa WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Sursa
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Eficienta = reader["Eficienta"].ToString(),
                                PutereW = Convert.ToInt32(reader["PutereW"]),
                                Pret = Convert.ToDouble(reader["Pret"])
                            };
                        }
                    }
                }
            }
            return null;
        }






        private void UpdatePretTotal()
        {
            int pretCpu=0, pretCooler=0, pretMB=0, pretRami = 0, pretStocare = 0, pretGPU = 0, pretCase = 0, pretPSU = 0;
            // CPU
            if (CpuDropZone.Tag is int idCpu)
                pretCpu = GetPretComponenteDinDB(idCpu, "Procesor");

            // Cooler
            if (CoolerDropZone.Tag is int idCooler)
                pretCooler = GetPretComponenteDinDB(idCooler, "Cooler");

            // Placa de bază
            if (MbDropZone.Tag is int idMb)
                pretMB = GetPretComponenteDinDB(idMb, "PlacaDeBaza");

            // RAM
            if (RamiDropZone.Tag is int idRam)
                pretRami = GetPretComponenteDinDB(idRam, "Rami");

            // Stocare
            if (StocareDropZone.Tag is int idStoc)
                pretStocare = GetPretComponenteDinDB(idStoc, "Stocare");

            // GPU
            if (GpuDropZone.Tag is int idGpu)
                pretGPU = GetPretComponenteDinDB(idGpu, "GPU");

            // Carcasa
            if (CarcasaDropZone.Tag is int idCarcasa)
                pretCase = GetPretComponenteDinDB(idCarcasa, "Carcasa");

            // PSU
            if (PsuDropZone.Tag is int idPsu)
                pretPSU = GetPretComponenteDinDB(idPsu, "Sursa");
            total = pretCase + pretCooler+pretCpu+pretGPU+pretMB+pretPSU+pretRami+pretStocare;

            PretTotalTextBlock.Text = $"Preț Total: {total} RON";
        }
        private void DropZone_DragEnter(object sender, DragEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.BorderBrush = Brushes.LimeGreen; // indică zonă activă
            }
        }
        private void DropZone_DragLeave(object sender, DragEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.BorderBrush = Brushes.White; // revine la normal
            }
        }
        private int GetPretComponenteDinDB(int id, string tableName)
        {
            using (var connection = new SQLiteConnection("Data Source=DataBase.db"))
            {
                connection.Open();

                // Construieste query-ul folosind numele tabelului și ID-ul
                string query = $"SELECT Pret FROM {tableName} WHERE Id = @id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int pret))
                    {
                        return pret;
                    }
                }
            }

            return 0; // dacă nu găsește sau apare eroare
        }
    }
    public class CPU
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Socket { get; set; }
        public double FrecventaGhz { get; set; }
        public bool CoolerInclus { get; set; }
        public double Pret { get; set; }
    }
    public class Cooler
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public bool AIO { get; set; }
        public int RPM { get; set; }
        public double Pret { get; set; }
    }
    public class PlacaDeBaza
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Socket { get; set; }
        public int Memorie { get; set; }       // probabil memorie maximă suportată în GB
        public int SloturiRam { get; set; }    // numărul de sloturi RAM
        public int SloturiM2 { get; set; }     // numărul de sloturi M.2
        public double Pret { get; set; }
    }
    public class Rami
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public int Viteza { get; set; }
        public int Capacitate { get; set; }
        public int NrModule { get; set; }
        public double Pret { get; set; }
    }
    public class Stocare
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public int Capacitate { get; set; }  // în GB, de exemplu
        public string Tip { get; set; }      // SSD, HDD, NVMe, etc.
        public double Pret { get; set; }
    }
    public class GPU
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public int MemorieGB { get; set; }
        public double Pret { get; set; }
    }
    public class Carcasa
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public double Volum { get; set; }
        public double Pret { get; set; }
    }
    public class Sursa
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Eficienta { get; set; }
        public int PutereW { get; set; }
        public double Pret { get; set; }
    }
}
