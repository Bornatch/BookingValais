using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DTO
{
    public class Client
    {
        public int Idclient { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}