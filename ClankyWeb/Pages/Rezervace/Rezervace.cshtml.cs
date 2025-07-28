using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClankyWeb.Data;
using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Pages.Rezervace
{
    [Authorize]
    public class RezervaceModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IList<ReservationEvent> Events { get; set; }
        public RezervaceModel(ApplicationDbContext db) => _db = db;

        public async Task OnGetAsync()
        {
            Events = await _db.ReservationEvents
                              .Include(e => e.Reservations)
                              .OrderBy(e => e.Date)
                              .ToListAsync();
        }
    }
}

