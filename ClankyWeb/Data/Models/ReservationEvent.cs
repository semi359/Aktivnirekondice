using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClankyWeb.Data.Models
{
    public class ReservationEvent
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        public int Capacity { get; set; }

        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        // navigační kolekce registrací
        public ICollection<Reservation> Reservations { get; set; }
            = new List<Reservation>();
    }
}
