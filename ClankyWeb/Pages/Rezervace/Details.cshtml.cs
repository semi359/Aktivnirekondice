using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClankyWeb.Data;
using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Pages.Rezervace
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ReservationEvent EventItem { get; set; }

        public int CurrentUserId { get; set; }
        public bool IsRegistered { get; set; }
        public int ConfirmedCount { get; set; }
        public bool IsCapacityFull { get; set; }

        public DetailsModel(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            EventItem = await _db.ReservationEvents
                .Include(e => e.Reservations)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (EventItem == null)
                return Page();

            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(uid, out var userId))
            {
                CurrentUserId = userId;
                IsRegistered = EventItem.Reservations.Any(r => r.UserId == userId);
            }

            ConfirmedCount = EventItem.Reservations.Count(r => r.Status == ReservationStatus.Confirmed);
            IsCapacityFull = ConfirmedCount >= EventItem.Capacity;

            return Page();
        }

        public async Task<IActionResult> OnPostRegisterAsync(int id)
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(uid, out var userId))
            {
                var ev = await _db.ReservationEvents
                    .Include(e => e.Reservations)
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (ev != null)
                {
                    var confirmed = ev.Reservations.Count(r => r.Status == ReservationStatus.Confirmed);
                    if (confirmed < ev.Capacity && !ev.Reservations.Any(r => r.UserId == userId))
                    {
                        _db.Reservations.Add(new Reservation
                        {
                            ReservationEventId = id,
                            UserId = userId,
                            Status = ReservationStatus.Pending
                        });
                        await _db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToPage("./Details", new { id });
        }
        //delete události
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var ev = await _db.ReservationEvents.FindAsync(id);
            if (ev != null)
            {
                _db.ReservationEvents.Remove(ev);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("./Rezervace");
        }
    }
}
