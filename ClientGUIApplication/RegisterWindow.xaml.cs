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
using System.Xml.Linq;

namespace ClientGUIApplication
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private ServerInterface foob;
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Username.Text;
            string password = Password.Text;
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
            string vali = foob.Register(name, password);

            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
