using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
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
    /// Interaction logic for ShowAllServicesWindow.xaml
    /// This window is used to show all the availabel services and is used to allow the user to graphically select the needed service from the available services.
    /// </summary>
    public partial class ShowAllServicesWindow : Window
    {
        private List<string> dropList = new List<string>();
        public static ShowAllServicesWindow Instance;
        public string selectedService;
        public ShowAllServicesWindow()
        {
            InitializeComponent();
            Instance = this;

        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //selecting a service graphically via a listbox
        {
            string[] nameSplit = { "" };
            string[] split = { "" };
            string temp = listBox.SelectedItem.ToString();
            for (var i = 0; i < dropList.Count; i++)
            {

                nameSplit = dropList[i].Split(',');
                split = nameSplit[0].Split(':');
                string s1 = split[1];
                bool b = s1.ToLower().Contains(temp.ToLower());
                if (b)
                {
                    selectedService = dropList[i];
                }

            }

            ServiceWindowAS serviceWindowAS = new ServiceWindowAS();
            this.Close();
            serviceWindowAS.Show();

        }

        private string useService() //Connect to the RegistryProject api and connect to the get all services service 
        {
            RestClient restClient = new RestClient("http://localhost:61099/");
            RestRequest restRequest = new RestRequest("api/AllServices/{token}", Method.Get);
            restRequest.AddUrlSegment("token",LogInWindow.Instance.token);
            RestResponse restResponse = restClient.Execute(restRequest);
            dropList = JsonConvert.DeserializeObject<List<string>>(restResponse.Content);
            return " ";
        }

        private async void Button_Click(object sender, RoutedEventArgs e) //asynchornously get all the available services and display it in a listbox when the button is pressed
        {
            
            try
            {
                string[] nameSplit = { "" };
                string[] split = { "" };
                List<string> sourceList = new List<string>();
                Task<string> task = new Task<string>(useService);
                task.Start();
                status.Content = "Loading services please wait......";
                string val = await task;
                for (var i = 0; i < dropList.Count; i++)
                {

                    nameSplit = dropList[i].Split(',');
                    split = nameSplit[0].Split(':');
                    string s1 = split[1];
                    sourceList.Add(s1);

                }
                if (sourceList[0].Equals("Denied"))
                {
                    status.Content = "Authentication Error, please login again.";
                }
                else if (sourceList[0].Equals("No services available"))
                {
                    status.Content = "No services available";
                }
                else
                {
                    status.Content = "Done";
                    listBox.ItemsSource = sourceList;
                }
            }
            catch(System.IndexOutOfRangeException mes)
            {
                status.Content = "Exception caught: " + mes.Message;
            }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Button to go back to the Main services window 
        {
            MainServicesWindow mainServicesWindow = new MainServicesWindow();
            this.Close();
            mainServicesWindow.Show();
        }
    }
}
