using Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("api/MulTwoNumbers")] //Controller for the srevice of multiplying two numbers 
    public class MulTwoNumbersController : ApiController
    {
        private ServerInterface foob;
        [Route("{token}/{num1}/{num2}")]
        public IHttpActionResult Get(int token,int num1, int num2)
        {
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/AuthenticationService";
            var chanFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
            //Call validate service 
            string vali = foob.Validate(token);//calling the validate service from the authenticator server
            if (vali.Equals("validated"))//if validated return a jason object with the answer else return a jason object with a status denied message
            {
                var result = new
                {
                    answer = num1 * num2
                };
                return Ok(result);
            }
            else
            {
                var result = new
                {
                    Status = "Denied",
                    Reason = "Authentication Error"
                };
                return Ok(result);
            }
        }
    }
}
