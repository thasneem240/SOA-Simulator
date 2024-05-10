using Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LogInWindow.xaml
    /// Log in window that allows the user to login to an existing user account and generates and stores a token that is needed to access the services. 
    /// </summary>
    public partial class LogInWindow : Window
    {
        public static LogInWindow Instance;
        public int token;
        private string name;
        private string password;
        private ServerInterface foob;
        public LogInWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)// asynchornously connect to the Authenticator server and access the login service to generate a valid token, when the button is pressed
        {

            try
            {
                name = username.Text;
                password = passwordBox.Text;
                Task<int> task2 = new Task<int>(useLogInService);
                task2.Start();
                statusLabel2.Content = "Logging in please wait......";
                int val = await task2;
                if (val == -1)
                {
                    statusLabel2.Content = "Error User does not exists";
                }
                else
                {
                    token = val;
                    statusLabel2.Content = "Logged in please wait.....";
                    Thread.Sleep(2000);
                    statusLabel2.Content = "Please press the 'Done' button to access user services.";
                }
            }
            catch(System.FormatException message)
            {
                statusLabel2.Content = "Exception caught: " + message.Message + " Please fill all input boxes";
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Button to go to the MainServicesWindow
        {
            MainServicesWindow mainServicesWindow = new MainServicesWindow();
            this.Close();
            mainServicesWindow.Show();  
        }
        private int useLogInService()//Connect to the Authenticator server an access the Login service.
        {
            int retval = -1;
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
            retval = foob.Login(name, password);
            return retval;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//BUtton to go back to the main window
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
