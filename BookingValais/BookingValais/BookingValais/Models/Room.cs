using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingValais.Models
{
    public class Room
    {
        public int IdRoom { get; set; }
        public Hotel Hotel { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public Decimal Price { get; set; }
        public Boolean HasTV { get; set; }
        public Boolean HasHairDryer { get; set; }
    }
}