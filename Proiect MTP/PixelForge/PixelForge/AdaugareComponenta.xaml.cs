using System;
using System.Collections.Generic;
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
using System.Data.SQLite;

namespace PixelForge
{
    /// <summary>
    /// Interaction logic for AdaugareComponenta.xaml
    /// </summary>
    public partial class AdaugareComponenta : Window
    {
        public AdaugareComponenta()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ProduseWindow fereastraproduse = new ProduseWindow();
            fereastraproduse.Show();
            this.Close();
        }
        private void CpuButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanel.Children.Clear(); 

           
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };

            
            panel.Children.Add(new TextBlock
            {
                Text = "Nume:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtNume = new TextBox { Name = "TxtNume", Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Socket:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtSocket = new TextBox { Name = "TxtSocket", Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSocket);

           
            panel.Children.Add(new TextBlock
            {
                Text = "Frecvență (GHz):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtFrecventa = new TextBox { Name = "TxtFrecventa", Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtFrecventa);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Cooler inclus:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            ComboBox cmbCooler = new ComboBox
            {
                Name = "CmbCooler",
                Margin = new Thickness(0, 0, 0, 10),
                Width = 120
            };
            cmbCooler.Items.Add("Da");
            cmbCooler.Items.Add("Nu");
            cmbCooler.SelectedIndex = 0; 
            panel.Children.Add(cmbCooler);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Preț (RON):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtPret = new TextBox { Name = "TxtPret", Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnSalveaza.Click += (s, args) =>
            {
                
                string nume = txtNume.Text.Trim();
                string socket = txtSocket.Text.Trim();
                string frecText = txtFrecventa.Text.Trim();
                string pretText = txtPret.Text.Trim();
                bool coolerInclus = (cmbCooler.SelectedItem as string) == "Da";

                
                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(socket) ||
                    string.IsNullOrWhiteSpace(frecText) ||
                    string.IsNullOrWhiteSpace(pretText))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(frecText, out double frecventa))
                {
                    MessageBox.Show("Frecvența trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretText, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                        INSERT INTO Procesor (Nume, Socket, FrecventaGhz, CoolerInclus, Pret)
                        VALUES (@nume, @socket, @frecventa, @cooler, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@socket", socket);
                    cmd.Parameters.AddWithValue("@frecventa", frecventa);
                    cmd.Parameters.AddWithValue("@cooler", coolerInclus);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Procesor adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);

            
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void CoolerButton_Click(object sender, RoutedEventArgs e)
        {
           
            FormPanel.Children.Clear();

            
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };

           
            panel.Children.Add(new TextBlock
            {
                Text = "Nume:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock
            {
                Text = "AIO:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            ComboBox cmbAio = new ComboBox
            {
                Margin = new Thickness(0, 0, 0, 10),
                Width = 120
            };
            cmbAio.Items.Add("Da");
            cmbAio.Items.Add("Nu");
            cmbAio.SelectedIndex = 0; 
            panel.Children.Add(cmbAio);

            
            panel.Children.Add(new TextBlock
            {
                Text = "RPM:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtRpm = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtRpm);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Preț (RON):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnSalveaza.Click += (s, args) =>
            {
                
                string nume = txtNume.Text.Trim();
                bool aio = (cmbAio.SelectedItem as string) == "Da";
                string rpmText = txtRpm.Text.Trim();
                string pretText = txtPret.Text.Trim();

               
                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(rpmText) ||
                    string.IsNullOrWhiteSpace(pretText))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(rpmText, out int rpm))
                {
                    MessageBox.Show("RPM trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretText, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO Cooler (Nume, AIO, RPM, Pret)
                VALUES (@nume, @aio, @rpm, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@aio", aio);
                    cmd.Parameters.AddWithValue("@rpm", rpm);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Cooler adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);

            
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void MbButton_Click(object sender, RoutedEventArgs e)
        {
           
            FormPanel.Children.Clear();

            
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };

            
            panel.Children.Add(new TextBlock
            {
                Text = "Nume:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Socket:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtSocket = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSocket);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Memorie (GB):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtMemorie = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtMemorie);

           
            panel.Children.Add(new TextBlock
            {
                Text = "Sloturi RAM:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtSloturiRam = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSloturiRam);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Sloturi M.2:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtSloturiM2 = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtSloturiM2);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Preț (RON):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnSalveaza.Click += (s, args) =>
            {
                
                string nume = txtNume.Text.Trim();
                string socket = txtSocket.Text.Trim();
                string memorieText = txtMemorie.Text.Trim();
                string sloturiRamText = txtSloturiRam.Text.Trim();
                string sloturiM2Text = txtSloturiM2.Text.Trim();
                string pretTextInner = txtPret.Text.Trim();

                
                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(socket) ||
                    string.IsNullOrWhiteSpace(memorieText) ||
                    string.IsNullOrWhiteSpace(sloturiRamText) ||
                    string.IsNullOrWhiteSpace(sloturiM2Text) ||
                    string.IsNullOrWhiteSpace(pretTextInner))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(memorieText, out int memorie))
                {
                    MessageBox.Show("Memoria trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(sloturiRamText, out int sloturiRam))
                {
                    MessageBox.Show("Sloturi RAM trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(sloturiM2Text, out int sloturiM2))
                {
                    MessageBox.Show("Sloturi M.2 trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretTextInner, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO PlacaDeBaza (Nume, Socket, Memorie, SloturiRam, SloturiM2, Pret)
                VALUES (@nume, @socket, @memorie, @sloturiRam, @sloturiM2, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@socket", socket);
                    cmd.Parameters.AddWithValue("@memorie", memorie);
                    cmd.Parameters.AddWithValue("@sloturiRam", sloturiRam);
                    cmd.Parameters.AddWithValue("@sloturiM2", sloturiM2);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Placă de bază adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);

            
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void RamiButton_Click(object sender, RoutedEventArgs e)
        {
            
            FormPanel.Children.Clear();

            
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(10),
                Width = 500
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(5) };

            
            panel.Children.Add(new TextBlock
            {
                Text = "Nume:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Viteză (MHz):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtViteza = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtViteza);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Capacitate (GB):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtCapacitate = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtCapacitate);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Număr module:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtNrModule = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNrModule);

            
            panel.Children.Add(new TextBlock
            {
                Text = "Preț (RON):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 2)
            });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

           
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnSalveaza.Click += (s, args) =>
            {
                
                string nume = txtNume.Text.Trim();
                string vitezaText = txtViteza.Text.Trim();
                string capacitateText = txtCapacitate.Text.Trim();
                string nrModuleText = txtNrModule.Text.Trim();
                string pretTextInner = txtPret.Text.Trim();

                
                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(vitezaText) ||
                    string.IsNullOrWhiteSpace(capacitateText) ||
                    string.IsNullOrWhiteSpace(nrModuleText) ||
                    string.IsNullOrWhiteSpace(pretTextInner))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(vitezaText, out int viteza))
                {
                    MessageBox.Show("Viteza trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(capacitateText, out int capacitate))
                {
                    MessageBox.Show("Capacitatea trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(nrModuleText, out int nrModule))
                {
                    MessageBox.Show("Numărul de module trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretTextInner, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO Rami (Nume, Viteza, Capacitate, NrModule, Pret)
                VALUES (@nume, @viteza, @capacitate, @nrModule, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@viteza", viteza);
                    cmd.Parameters.AddWithValue("@capacitate", capacitate);
                    cmd.Parameters.AddWithValue("@nrModule", nrModule);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("RAM adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);

            
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void StocareButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanel.Children.Clear();

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
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock { Text = "Capacitate (GB):", FontWeight = FontWeights.Bold });
            TextBox txtCapacitate = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtCapacitate);

           
            panel.Children.Add(new TextBlock { Text = "Tip (HDD / SSD / NVMe):", FontWeight = FontWeights.Bold });
            TextBox txtTip = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtTip);

            
            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, args) =>
            {
                string nume = txtNume.Text.Trim();
                string capacitateText = txtCapacitate.Text.Trim();
                string tip = txtTip.Text.Trim();
                string pretText = txtPret.Text.Trim();

                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(capacitateText) ||
                    string.IsNullOrWhiteSpace(tip) ||
                    string.IsNullOrWhiteSpace(pretText))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(capacitateText, out int capacitate))
                {
                    MessageBox.Show("Capacitatea trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretText, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO Stocare (Nume, Capacitate, Tip, Pret)
                VALUES (@nume, @capacitate, @tip, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@capacitate", capacitate);
                    cmd.Parameters.AddWithValue("@tip", tip);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Stocare adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void GpuButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanel.Children.Clear();

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
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

           
            panel.Children.Add(new TextBlock { Text = "Memorie (GB):", FontWeight = FontWeights.Bold });
            TextBox txtMemorie = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtMemorie);

          
            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, args) =>
            {
                string nume = txtNume.Text.Trim();
                string memorieText = txtMemorie.Text.Trim();
                string pretText = txtPret.Text.Trim();

                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(memorieText) ||
                    string.IsNullOrWhiteSpace(pretText))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(memorieText, out int memorie))
                {
                    MessageBox.Show("Memoria trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretText, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO GPU (Nume, MemorieGB, Pret)
                VALUES (@nume, @memorie, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@memorie", memorie);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Placă video adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void CaseButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanel.Children.Clear();

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
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

           
            panel.Children.Add(new TextBlock { Text = "Volum (litri):", FontWeight = FontWeights.Bold });
            TextBox txtVolum = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtVolum);

            
            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, args) =>
            {
                string nume = txtNume.Text.Trim();
                string volumText = txtVolum.Text.Trim();
                string pretText = txtPret.Text.Trim();

                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(volumText) ||
                    string.IsNullOrWhiteSpace(pretText))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(volumText, out double volum))
                {
                    MessageBox.Show("Volumul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretText, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO Carcasa (Nume, Volum, Pret)
                VALUES (@nume, @volum, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@volum", volum);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Carcasa a fost adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }
        private void PsuButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanel.Children.Clear();

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
            TextBox txtNume = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtNume);

            
            panel.Children.Add(new TextBlock { Text = "Eficiență (ex: 80+ Bronze):", FontWeight = FontWeights.Bold });
            TextBox txtEficienta = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtEficienta);

            
            panel.Children.Add(new TextBlock { Text = "Putere (W):", FontWeight = FontWeights.Bold });
            TextBox txtPutere = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(txtPutere);

            
            panel.Children.Add(new TextBlock { Text = "Preț (RON):", FontWeight = FontWeights.Bold });
            TextBox txtPret = new TextBox { Margin = new Thickness(0, 0, 0, 20) };
            panel.Children.Add(txtPret);

            
            Button btnSalveaza = new Button
            {
                Content = "Salvează",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            btnSalveaza.Click += (s, args) =>
            {
                string nume = txtNume.Text.Trim();
                string eficienta = txtEficienta.Text.Trim();
                string putereText = txtPutere.Text.Trim();
                string pretText = txtPret.Text.Trim();

                if (string.IsNullOrWhiteSpace(nume) ||
                    string.IsNullOrWhiteSpace(eficienta) ||
                    string.IsNullOrWhiteSpace(putereText) ||
                    string.IsNullOrWhiteSpace(pretText))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(putereText, out int putere))
                {
                    MessageBox.Show("Puterea trebuie să fie un număr întreg.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(pretText, out double pret))
                {
                    MessageBox.Show("Prețul trebuie să fie un număr real.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    using var connection = new SQLiteConnection("Data Source=DataBase.db");
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO Sursa (Nume, Eficienta, PutereW, Pret)
                VALUES (@nume, @eficienta, @putere, @pret);";

                    using var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@eficienta", eficienta);
                    cmd.Parameters.AddWithValue("@putere", putere);
                    cmd.Parameters.AddWithValue("@pret", pret);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Eroare la inserare: {ex.Message}", "Eroare BD", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Sursa a fost adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                FormPanel.Children.Clear();
            };

            panel.Children.Add(btnSalveaza);
            card.Child = panel;
            FormPanel.Children.Add(card);
        }


    }
}
