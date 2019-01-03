using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingValais.Models
{
    public class ResearchParameters
    {
        public int NbPersons { get; set; }
        public int NbStars { get; set; }
        public string Location { get; set; }
        public Boolean HasParking { get; set; }
        public Boolean HasWifi { get; set; }
    }
}