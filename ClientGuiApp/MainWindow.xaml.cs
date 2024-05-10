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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Main window that allows the user to either register a new user or log in as an existing one.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)//Button to open the new user registering window
        {
            RegisterWindow registerWindow = new RegisterWindow();
            this.Close();
            registerWindow.Show();
        }

        private void OpenWIndow(object sender, RoutedEventArgs e)//button to open the log in window
        {
            LogInWindow login = new LogInWindow();
            this.Close();
            login.Show();
        }
    }
}
