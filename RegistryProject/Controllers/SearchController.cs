using Authenticator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace RegistryProject.Controllers
{
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {
        private ServerInterface foob;
        string registryPath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\RegistryProject\Registry.txt";
        // GET: api/Search/5
        [Route("{token}/{searchText}")]//Controller for the srevice of searching for a specific service
        public IEnumerable<string> Get(int token ,string searchText)
        {

            
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
           
            string vali = foob.Validate(token);//calling the validate service from the authenticator server
            List<string> returnList = new List<string>();

            if (vali.Equals("validated"))//if validated return a list of the matching services, else return a list with a jason object with a status denied message 
            {
                List<string> list = new List<string>();
                
                list = File.ReadAllLines(registryPath).ToList();
                string[] descriptionSplit = { "" };
                string[] split = { "" };
                for (var i = 0; i < list.Count; i++)
                {

                    descriptionSplit = list[i].Split(',');
                    split = descriptionSplit[1].Split(':');
                    string s1 = split[1];
                    string s2 = searchText;
                    bool b = s1.ToLower().Contains(s2.ToLower());
                    if (b)
                    {
                        returnList.Add(list[i]);
                    }

                }
                if(returnList.Count == 0)//if there are no matching services available store a "No matching services" message in the list and return i
                {
                    returnList.Add("No matching services");
                }
                return returnList;
            }
            else
            {
                var result = new
                {
                    Status = "Denied",
                    Reason = "Authentication Error",

                };
                string json = JsonConvert.SerializeObject(result);
                returnList.Add(json);
                return returnList;
            }













        }


    }
}
