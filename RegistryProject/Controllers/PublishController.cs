using Authenticator;
using Microsoft.Ajax.Utilities;
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
    public class PublishController : ApiController
    {

        private ServerInterface foob;
        string registryPath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\RegistryProject\Registry.txt";
        // POST: api/Publish
        public IHttpActionResult Post([FromBody] List<string> value) //Controller for the srevice of publishing a new service
        {
            int token = int.Parse(value[0]);//the first element of the input string is the token
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
         
            string vali = foob.Validate(token);//calling the validate service from the authenticator server

            if (vali.Equals("validated"))//if validated return a jason object with a status approved message, else return a jason object with a status denied message 
            {
                List<string> list = new List<string>();
                list = File.ReadAllLines(registryPath).ToList();
                var serviceInfo = new
                {
                    Name = value[1],
                    Description = value[2],
                    API_Endpoint = value[3],
                    Number_of_operands = value[4],
                    Operand_type = value[5],    

                };
                string json = JsonConvert.SerializeObject(serviceInfo);
                list.Add(json);
                File.WriteAllLines(registryPath, list);

                var result = new
                {
                    Status = "Approved"

                };
                return Ok(result);
            }
            else
            {
                var result = new
                {
                    Status = "Denied",
                    Reason = "Authentication Error",

                };
                return Ok(result);
            }

        }


    }
}
