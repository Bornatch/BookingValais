using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookingValais.ViewModels
{
    public class RoomVM
    {
        [Required]
        public IEnumerable<SelectListItem> NumberRooms { get; set; }
    }

    public enum NumberRooms
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9
    }
}