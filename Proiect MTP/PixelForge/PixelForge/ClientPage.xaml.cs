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
using System.IO;
using System.Data;

namespace PixelForge
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Window
    {
        public ClientPage()
        {
            InitializeComponent();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CrearePCButton_Click(object sender, RoutedEventArgs e)
        {
            CreatePC createPC = new CreatePC();
            createPC.Show();
            this.Close();
        }

        private void StatusComenziButton_Click(object sender, RoutedEventArgs e)
        {
            StatusComenzi statusComenzi = new StatusComenzi();
            statusComenzi.Show();
            this.Close();
        }

    }
}
