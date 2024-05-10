using Authenticator;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublishingConsoleApplication
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            int token= -1;
            string control;
            ServerInterface foob;
            do
            {
                Console.Clear();
                control = "0";
                Console.WriteLine("Welcome to the Service Publishing Console, please enter the number of the action you want to perform: ");
                Console.WriteLine("1: Register new user.");
                Console.WriteLine("2: Login.");
                Console.WriteLine("3: Publish a new service.");
                Console.WriteLine("4: Unpublish a service.");
                Console.WriteLine("5: Exit app.");
                Console.Write("Please enter the number of the action you want to perform: ");
                control = Console.ReadLine();


                if (control.Equals("1")) //registering a new user 
                {
                    Console.Clear();
                    Console.Write("Please enter new user's name: ");
                    string name = Console.ReadLine();
                    Console.Write("Please enter new user's password: ");
                    string password = Console.ReadLine();
                    Console.Write("Please re-enter the password : ");
                    string reEnter = Console.ReadLine();
                    if (reEnter.Equals(password))
                    {
                        var tcp = new NetTcpBinding();
                        //Set the URL and create the connection!
                        var URL = "net.tcp://localhost:8100/AuthenticationService";
                        var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
                        foob = chanFactory.CreateChannel();
                        string vali = foob.Register(name, password); //calling the register service from the authenticator server
                        Console.WriteLine(vali);
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Please make sure that the re-entered password is the same as the intended password");
                        Console.ReadLine();
                    }


                }
                else if (control.Equals("2")) //logging in to an existing user 
                {
                    Console.Clear();
                    Console.Write("Please enter user's name: ");
                    string name = Console.ReadLine();
                    Console.Write("Please enter user's password: ");
                    string password = Console.ReadLine();
                    var tcp = new NetTcpBinding();
                    //Set the URL and create the connection!
                    var URL = "net.tcp://localhost:8100/AuthenticationService";
                    var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
                    foob = chanFactory.CreateChannel();
                    int vali = foob.Login(name, password);//calling the log-in service from the authenticator server
                    if (vali == -1)
                    {
                        Console.WriteLine("User does not exist, please register the new user.");
                    }
                    else
                    {
                        token = vali;
                        Console.WriteLine("Login successful,token generated and stored.");
                    }
                    Console.ReadLine();

                }
                else if (control.Equals("3")) //Publishihng a new service
                {
                    Console.Clear();
                    if (token == -1)
                    {
                        Console.WriteLine("User not logged in.");
                    }
                    else
                    {

                        int check = 0;
                        List<string> inDetails = new List<string>();
                        inDetails.Add(token.ToString());
                        Console.Write("Please enter the service name: ");
                        inDetails.Add(Console.ReadLine());
                        Console.Write("Please enter the service description: ");
                        inDetails.Add(Console.ReadLine());
                        Console.Write("Please enter the service endpoint: ");
                        inDetails.Add(Console.ReadLine());
                        Console.Write("Please enter the number of operands used in the service: ");
                        inDetails.Add(Console.ReadLine());
                        Console.Write("Please enter the type of  operands used in the service : ");
                        inDetails.Add(Console.ReadLine());
                        for (int i = 0; i < inDetails.Count; i++)//Check if all the required information is entered.
                        {
                            if (string.IsNullOrEmpty(inDetails[i]))
                            {
                                check++;
                            }
                        }
                        if (check == 0)
                        {
                            if (inDetails[3].Split(':').Length == 3)//Check if the service endpoint is entered correctly.
                            {
                                int value;
                                if (int.TryParse(inDetails[4], out value))//Check if the number of operands entered correctly.
                                {
                                    RestClient restClient = new RestClient("http://localhost:61099/"); //Connect to the RegistryProject api and connect to the publish service 
                                    RestRequest restRequest = new RestRequest("api/Publish", Method.Post);
                                    restRequest.AddBody(inDetails);
                                    RestResponse restResponse = restClient.Execute(restRequest);
                                    JObject parsed = JObject.Parse(restResponse.Content);
                                    foreach (var pair in parsed)
                                    {
                                        Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Please enter the number of operands used in the service as an integer value.");
                                }
                              
                            }
                            else
                            {
                                Console.WriteLine("Please enter the service endpoint in the following format 'http://localhost:portNumber/api/ServiceName/{token}/{operand1}/{operand2}/{operand3}'");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Please fill in all the required information");
                        }

                        
                    }
                    Console.ReadLine();


                }
                else if (control.Equals("4"))//Unpublishing a service 
                {
                    
                    Console.Clear();
                    if (token == -1)
                    {
                        Console.WriteLine("User not logged in.");
                    }
                    else
                    {



                        List<string> deletVal = new List<string>();
                        deletVal.Add(token.ToString());
                        Console.Write("Please enter the exact endpoint of the service you want to unpublish: ");
                        deletVal.Add(Console.ReadLine());
                        RestClient restClient = new RestClient("http://localhost:61099/");//Connect to the  RegistryProject  api and connect to the unpublish service 
                        RestRequest restRequest = new RestRequest("api/Unpublish", Method.Delete);
                        restRequest.AddBody(deletVal);
                        RestResponse restResponse = restClient.Execute(restRequest);
                        Console.WriteLine(restResponse.Content);
                        
                    }
                    Console.ReadLine();
                }



            } while(!control.Equals("5")); //exiting the consol app

        }
    }
}
