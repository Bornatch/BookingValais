using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingValais.ViewModels
{
    public class HomeVM
    {
        [Required]
        public int NbPersons { get; set; }

        [Required]
        public IEnumerable<SelectListItem> Stars { get; set; }

        public Stars HotelStars { get; set; }

        public Persons HotelPersons { get; set; }

    }

    public enum Stars
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

    public enum Persons {
        One = 1,
        Two = 2
    }
}