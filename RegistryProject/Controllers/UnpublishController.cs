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
    [RoutePrefix("api/Unpublish")]
    public class UnpublishController : ApiController
    {

        private ServerInterface foob;
        string registryPath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\RegistryProject\Registry.txt";
        // DELETE: api/Unpublish/5
        
        public IHttpActionResult Delete([FromBody] List<string> deleteText) //Controller for the srevice of unpublishing a service
        {
            var tcp = new NetTcpBinding();
            int token = int.Parse(deleteText[0]);//the first element of the input string is the token
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
            //Call validate service 
            string vali = foob.Validate(token);//calling the validate service from the authenticator server



            if (vali.Equals("validated"))//if validated return a jason object with a status approved message, else return a jason object with a status denied message 
            {
                List<string> list = new List<string>();
                List<string> rewrite = new List<string>();
                list = File.ReadAllLines(registryPath).ToList();
                File.WriteAllLines(registryPath, rewrite);        
                string[] endPointSplit = { "" };
                string[] split = { "" };
                for (var i = 0; i < list.Count; i++)
                {

                    endPointSplit = list[i].Split(',');
                    split = endPointSplit[2].Split(':');
                    string s1 = split[1] + ":" + split[2] + ":" + split[3];
                    string s2 = deleteText[1];
                    bool b = s1.Contains(s2);
                    if (!b)
                    {
                        rewrite.Add(list[i]);
                    }
                    


                }
                if (list.Count == rewrite.Count) //if there is no matching service return a jason object with a No matching service messaage
                {
                    File.WriteAllLines(registryPath, rewrite);
                    var result = new
                    {
                        Status = "No matching service",

                    };
                    return Ok(result);
                }
                else
                {
                    File.WriteAllLines(registryPath, rewrite);
                    var result = new
                    {
                        Status = "Approved",

                    };
                    return Ok(result);
                }


                
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
