using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public int IdClient { get; set; }
        public Decimal TotalPrice { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string hotelName { get; set; }
    }
}
