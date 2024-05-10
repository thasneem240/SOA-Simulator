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

namespace ClientGuiApp
{
    /// <summary>
    /// Interaction logic for MainServicesWindow.xaml
    /// Window to allow the user to search for a specific service or to allow the user to display all available services 
    /// </summary>
    public partial class MainServicesWindow : Window
    {
        public MainServicesWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)//BUtton to go back to the ShowAllServicesWindow
        {
            ShowAllServicesWindow showAllServicesWindow = new ShowAllServicesWindow();
            this.Close();
            showAllServicesWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //BUtton to go to the SearchServicesWindow
        {
            SearchServicesWindow searchServicesWindow = new SearchServicesWindow();
            this.Close();
            searchServicesWindow.Show();    
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//BUtton to go back to the log in window
        {
            LogInWindow logInWindow = new LogInWindow();
            this.Close();
            logInWindow.Show();
        }
    }
}
