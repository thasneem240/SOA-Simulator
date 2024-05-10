using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
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
    /// Interaction logic for SearchServicesWindow.xaml
    /// Window to search for specidfied services and allow the usre to graphically select the needed one using a list box
    /// </summary>
    public partial class SearchServicesWindow : Window
    {
        public static SearchServicesWindow Instance;
        public string selectedService;
        private List<string> dropList = new List<string>();
        private string searchString;
        public SearchServicesWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private async void Button_Click(object sender, RoutedEventArgs e)//asynchornously get the specified and display it in a listbox when the button is pressed
        {
            try
            {
                string[] nameSplit = { "" };
                string[] split = { "" };
                List<string> sourceList = new List<string>();
                searchString = searchBox.Text;
                Task<string> task = new Task<string>(useService);
                task.Start();
                statusLabel.Content = "Loading services please wait......";
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
                    statusLabel.Content = "Authentication Error, please login again.";
                }
                else if (sourceList[0].Equals("No matching services"))
                {
                    statusLabel.Content = "No matching services";
                }
                else
                {
                    statusLabel.Content = "Done";
                    myListbox.ItemsSource = sourceList;
                }
            }
            catch(System.IndexOutOfRangeException mes) 
            {
                statusLabel.Content = "Exception caught: " + mes.Message;
            }
            catch (System.FormatException message)
            {
                statusLabel.Content = "Exception caught: " + message.Message + " Please fill all input boxes";
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //selecting a service graphically via a listbox
        {
            string[] nameSplit = { "" };
            string[] split = { "" };
            string temp =  myListbox.SelectedItem.ToString();
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

            ServiceWindowS serviceWindowS = new ServiceWindowS();
            this.Close();
            serviceWindowS.Show();
        }
        private string useService()//Connect to the RegistryProject api and connect to the search services service 
        {
            RestClient restClient = new RestClient("http://localhost:61099/");
            RestRequest restRequest = new RestRequest("api/Search/{token}/{searchText}", Method.Get);
            restRequest.AddUrlSegment("token", LogInWindow.Instance.token);
            restRequest.AddUrlSegment("searchText",searchString);
            RestResponse restResponse = restClient.Execute(restRequest);
            dropList = JsonConvert.DeserializeObject<List<string>>(restResponse.Content);
            return " ";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Button to go back to the Main services window 
        {
            MainServicesWindow mainServicesWindow = new MainServicesWindow();
            this.Close();
            mainServicesWindow.Show();
        }
    }
}
