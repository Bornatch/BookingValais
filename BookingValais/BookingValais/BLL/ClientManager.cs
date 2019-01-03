using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DTO;
using DAL;

namespace BLL
{
    public class ClientManager
    {
        //Get Client for login purposes
        public static Client GetClient(string surname, string name, string password)
        {
            Client client = DAL.ClientDb.GetClient(surname, name, password);
            return client;
        }
    }
}