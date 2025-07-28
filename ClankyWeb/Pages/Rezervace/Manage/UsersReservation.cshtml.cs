using System.Linq;
using System.Threading.Tasks;
using ClankyWeb.Data;
using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Pages.Rezervace.Manage
{
    [Authorize(Roles = $"{NRole.Admin},{NRole.Autor}")]
    public class UsersReservationModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ReservationEvent EventItem { get; set; }
        public int ConfirmedCount { get; set; }

        public bool IsCapacityFull { get; set; }

        public UsersReservationModel(ApplicationDbContext db) => _db = db;

        public async Task OnGetAsync(int id)
        {
            EventItem = await _db.ReservationEvents
                .Include(e => e.Reservations).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            ConfirmedCount = EventItem.Reservations
                .Count(r => r.Status == ReservationStatus.Confirmed);
            IsCapacityFull = ConfirmedCount >= EventItem.Capacity;
        }

        public async Task<IActionResult> OnPostConfirmAsync(int id, int reservationId)
        {
            var res = await _db.Reservations.FindAsync(reservationId);
            if (res != null)
            {
                res.Status = ReservationStatus.Confirmed;
                await _db.SaveChangesAsync();
            }
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int id, int reservationId)
        {
            var res = await _db.Reservations.FindAsync(reservationId);
            if (res != null)
            {
                res.Status = ReservationStatus.Rejected;
                await _db.SaveChangesAsync();
            }
            return RedirectToPage(new { id });
        }
    }
}
