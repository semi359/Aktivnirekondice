using System;
using System.ComponentModel.DataAnnotations;
using ClankyWeb.Data;

namespace ClankyWeb.Data.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int ReservationEventId { get; set; }
        public ReservationEvent ReservationEvent { get; set; }

        public int UserId { get; set; }
        public NUser User { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        [DataType(DataType.DateTime)]
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
