using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Authenticator
{
    internal class Program
    {
        private ServerImplmentation serverImplmentation = new ServerImplmentation();
        static void Main(string[] args)
        {
            //Start the server
            Thread maintherad = Thread.CurrentThread;
            Console.WriteLine("Welcome");
            var tcp = new NetTcpBinding();
            //Bind the interface
            //Create the host
            var host = new ServiceHost(typeof(ServerImplmentation));
            host.AddServiceEndpoint(typeof(ServerInterface), tcp, "net.tcp://0.0.0.0:8100/AuthenticationService");
            host.Open();
            //Hold the server open until someone does something
            Console.WriteLine("System Online");
            Console.WriteLine("Please enter login duration in minutes");
            string var = Console.ReadLine();
            Thread thr1 = new Thread(() => timer(var));
            thr1.Start();
            Console.ReadLine();
            //Close the host
            host.Close();
        }
        public static void timer(string mins) //method to periodically clean the token file
        {
            try
            {
                List<string> tokenFileLines = new List<string>();
                String tokensFilePath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\Authenticator\TokenFile.txt";
                tokenFileLines.Add(" ");
                int val = 60000 * (Int32.Parse(mins));
                for (; ; )
                {

                    Thread.Sleep(val);
                    File.WriteAllLines(tokensFilePath, tokenFileLines);



                }
            }
            catch (System.FormatException message)
            {
                Console.WriteLine("Exception caught: " + message.Message + " Please enter a whole number value.");
            }
        }

    }
}
