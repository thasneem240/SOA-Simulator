using Authenticator;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace RegistryProject.Controllers
{
    [RoutePrefix("api/AllServices")]//Controller for the srevice of getting all the available  services 
    public class AllServicesController : ApiController
    {


        // GET: api/AllServices
        private ServerInterface foob;
        string registryPath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\RegistryProject\Registry.txt";
        [Route("{token}")]
        public IEnumerable<string> Get(int token)
        {

            List<string> list = new List<string>();
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
      
            string vali = foob.Validate(token);//calling the validate service from the authenticator server

            if (vali.Equals("validated"))//if validated return a list of the available services, else return a list with a jason object with a status denied message 
            {

                list = File.ReadAllLines(registryPath).ToList();//if there are no services available store a "No services available" message in the list and return it 
                if (list.Count == 0)
                {
                    list.Add("No services available");
                }
                return list;

            }
            else
            {
                var result = new
                {
                    Status = "Denied",
                    Reason = "Authentication Error",

                };
                string json = JsonConvert.SerializeObject(result);
                list.Add(json);
                return list;
            }
        }
    }
}
