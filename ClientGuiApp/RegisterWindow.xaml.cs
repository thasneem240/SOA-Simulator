using Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for RegisterWindow.xaml
    /// //Register window that allows the user to register a new user
    /// </summary>  
    public partial class RegisterWindow : Window
    {
        private string name;
        private string password;
        private string rePassword;
        private ServerInterface foob;
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)// asynchornously connect to the Authenticator server and access the register service to generate a valid token, when the button is pressed
        {
            try
            {
                name = Username.Text;
                password = Password.Text;
                rePassword = RePassword.Text;
                if (rePassword.Equals(password))
                {
                    Task<string> task = new Task<string>(useRegisterService);
                    task.Start();
                    statusLabel.Content = "Registering new user please wait......";
                    string val = await task;
                    if (val.Equals("Error"))
                    {
                        statusLabel.Content = "Error User already exists";
                    }
                    else
                    {
                        statusLabel.Content = "User Registered";
                    }
                }
                else
                {
                    statusLabel.Content = "Please make sure that the re-entered password is the same as the intended password";
                }

            }
            catch (System.FormatException message)
            {
                statusLabel.Content = "Exception caught: " + message.Message+" Please fill all input boxes";
            }

        }

        private string useRegisterService()//Connect to the Authenticator server an access the register service.
        {
            string retval ="Done";
        
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
            string vali = foob.Register(name, password);
            if (vali.Equals("Registration unsuccessfull, details already used."))
            {
                return "Error";
            }
            return retval;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)//BUtton to go back to the main window
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void RePassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
