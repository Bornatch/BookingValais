using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Picture
    {
        public int IdPicture { get; set; }
        public Room Room { get; set; }
        public string Url { get; set; }
    }
}
