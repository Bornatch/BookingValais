using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DTO;
using DAL;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BLL
{
    public class ClientManager
    {
        static String baseUri = "http://localhost:3749/api/Clients/";

        //Get Client for login purposes
        public static Client GetClient(string surname, string name, string password)
        {
            Client client = new Client();
            try
            {
                string uri = baseUri + "GetClient/" + surname + "/" + name + "/" + password;
                client = null;
                using (HttpClient httpClient = new HttpClient())
                {

                    Task<String> response = httpClient.GetStringAsync(uri);
                    client = JsonConvert.DeserializeObject<Client>(response.Result);
                }
            }
            catch
            {
                return null;
            }

            return client;
            
        }
    }
}