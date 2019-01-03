using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomReservation
    {
        public int IdRoomReservation { get; set; }
        public Room Room { get; set; }
        public Reservation Reservation { get; set; }
        public int Quantity { get; set; }

    }
}
