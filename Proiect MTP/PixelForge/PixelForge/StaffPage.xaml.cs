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

namespace PixelForge
{
    public partial class StaffPage : Window
    {
        public StaffPage()
        {
            InitializeComponent();
        }
        private void ProduseButton_Click(object sender, RoutedEventArgs e)
        {
            ProduseWindow produseWindow = new ProduseWindow();
            produseWindow.Show();
            this.Close();
        }

        private void ComenziButton_Click(object sender, RoutedEventArgs e)
        {
            ComenziWindow comenziWindow = new ComenziWindow();
            comenziWindow.Show();
            this.Close();
        }

        private void ConturiButton_Click(object sender, RoutedEventArgs e)
        {
            ConturiWindow conturiWindow = new ConturiWindow();
            conturiWindow.Show();
            this.Close();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();

            this.Close(); 
        }
    }
}
