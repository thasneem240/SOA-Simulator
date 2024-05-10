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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ServerImplmentation : ServerInterface
    {
        public static ServerImplmentation Instance { get; } = new ServerImplmentation();
        

        String detailsFilePath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\Authenticator\LoginDetails.txt";
        String tokensFilePath = @"C:\Users\Thasneem2\source\repos\Assignmnet_1\Authenticator\TokenFile.txt";
        private readonly Random random = new Random();





        public int Login(string name, string password) //Login service 
        {
            String fileLine = name + " " + password;
            int token = -1;
            List<string> fileLines = new List<string>();
            List<string> tokenFileLines = new List<string>();
            fileLines = File.ReadAllLines(detailsFilePath).ToList();
            if (fileLines.Contains(fileLine))
            {
                token = random.Next(100000000, 999999999); //rendomly generate a token value to be stored in the TokenFile.txt.
                string tokenSt = token.ToString();
                tokenFileLines= File.ReadAllLines(tokensFilePath).ToList();
                tokenFileLines.Add(tokenSt);
                File.WriteAllLines(tokensFilePath, tokenFileLines);

            }
            return token;
        }

        public string Validate(int token) //Validate service 
        {
            List<string> tokenFileLines = new List<string>();
            tokenFileLines = File.ReadAllLines(tokensFilePath).ToList();
            string retrunval = " ";
            if (tokenFileLines.Contains(token.ToString()))
            {
                retrunval = "validated";
            }
            else
            {
                retrunval = "not validated";
            }
            return retrunval;
        }

        public string Register(string name, string password) //Registerring new user service 
        {
            String message;
            String fileLine = name + " " + password;
            List<string> fileLines = new List<string>();
            fileLines = File.ReadAllLines(detailsFilePath).ToList();
            if (fileLines.Contains(fileLine))
            {
                message = "Registration unsuccessfull, details already used.";
                
            }
            else
            {
                message = "Successfully registered.";
                fileLines.Add(fileLine);
                File.WriteAllLines(detailsFilePath, fileLines);
            }
            return message;
        }
    }

}

