using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for ServiceWindowAS.xaml
    /// Dynamically creating a window to allow the user to enter data and get the result , according to the service they chose from the show all services window
    /// </summary>
    public partial class ServiceWindowAS : Window
    {
        private List<TextBox> list = new List<TextBox>();
        private List<int> ints = new List<int>();
        private TextBox result;
        private string apiEndPoint;
        private int numOfOperand;
        private Label statusLabel;
        public ServiceWindowAS()//Dynamically add text boxes, labels and buttons to a stack panel and display them
        {
            InitializeComponent();
            string val = ShowAllServicesWindow.Instance.selectedService;
            StackPanel stackPanel = new StackPanel();
            

            string[] strings = val.Split(',');
            string[] tempname;
            string[] tempApiEndPoint;
            string[] tempNumOfOperand;

            string name = strings[0];
            apiEndPoint = strings[2];
            string num = strings[3];
            tempNumOfOperand = num.Split(':');
            numOfOperand = Int32.Parse(tempNumOfOperand[1].Trim(new char[] { '"', '”' }));
            tempname = name.Split(':');
            name = tempname[1];
            tempApiEndPoint = apiEndPoint.Split(':');
            apiEndPoint = tempApiEndPoint[1] + ":" + tempApiEndPoint[2] + ":" + tempApiEndPoint[3];
            apiEndPoint = apiEndPoint.Trim(new char[] { '"', '”' });

            Label label = new Label();
            label.Margin = new Thickness(5);
            label.Width = 120;
            label.Content = name;
            stackPanel.Children.Add(label);
            int count = 1;

            for (int i = 0; i < numOfOperand; i++)
            {

                list.Add(new TextBox());
                list[i].Name = "textBox" + count.ToString();
                list[i].Margin = new Thickness(5);
                list[i].Text = "Value" + " " + count.ToString();
                list[i].Width = 120;
                stackPanel.Children.Add(list[i]);
                count++;
            }
            result = new TextBox();
            result.Margin = new Thickness(5);
            result.Width = 120;
            result.Name = "resultBox";
            stackPanel.Children.Add(result);

            Button button = new Button();
            button.Margin = new Thickness(5);
            button.Name = "btn_result";
            button.Content = "Test";
            button.Width = 50;
            button.Click += button_search_click;
            stackPanel.Children.Add(button);

            Button backButton = new Button();
            backButton.Margin = new Thickness(5);
            backButton.Name = "btn_back";
            backButton.Content = "Back";
            backButton.Width = 50;
            backButton.Click += back_button_click;
            stackPanel.Children.Add(backButton);

            statusLabel = new Label();
            statusLabel.Margin = new Thickness(5);
            statusLabel.Width = 350;
            statusLabel.Content = "Ready";
            stackPanel.Children.Add(statusLabel);
            this.Content = stackPanel;

        }


        private void back_button_click(object sender, RoutedEventArgs e)//Button to go back to the Main ShowAllServices window 
        {
            ShowAllServicesWindow showAllServicesWindow = new ShowAllServicesWindow();
            this.Close();
            showAllServicesWindow.Show();
        }

        private async void button_search_click(object sender, RoutedEventArgs e)//asynchornously get the results after entering the input values when the button is pressed
        {
            try
            {
                for (int x = 0; x < numOfOperand; x++)
                {
                    ints.Add(Int32.Parse(list[x].Text));
                }
                Task<string> task = new Task<string>(useService);
                task.Start();
                statusLabel.Content = "Processing please wait....";
                string ret = await task;
                if (ret.Equals("Denied"))
                {
                    statusLabel.Content = "Authentication Error, please login again.";
                }
                else
                {
                    statusLabel.Content = "Done";
                    result.Text = ret;
                }
            }
            catch(System.FormatException message)
            {
                statusLabel.Content = "Exception caught: " + message.Message; 
            }
            
            


        }

        private string useService()//Connect to the ServiceProvider api and connect to appropiate service 
        {
            string url;
            string endpoint = "";
            int count = 0;

            for (int i = 0; i < apiEndPoint.Length; i++)
            {
                if ((apiEndPoint[i].Equals('/')))
                {

                    count++;
                    if (count == 3)
                    {
                        endpoint = apiEndPoint.Substring(i + 1);
                    }

                }
            }

            string end = endpoint.Replace('”', ' ');
            RestClient restClient = new RestClient("http://localhost:59985/");
            RestRequest restRequest = new RestRequest(endpoint, Method.Get);
            int tok = LogInWindow.Instance.token;
            restRequest.AddUrlSegment("token", tok);
            int varCount = 1;
            for (int i = 0; i < numOfOperand; i++)
            {
                restRequest.AddUrlSegment("num" + varCount.ToString(), ints[i] );
                varCount++;
            }

            RestResponse restResponse = restClient.Execute(restRequest);
            string res = restResponse.Content;
            string[] temp = res.Split(':');
            res = temp[1].Replace('}', ' ');
            return res;
        }


    }
}
