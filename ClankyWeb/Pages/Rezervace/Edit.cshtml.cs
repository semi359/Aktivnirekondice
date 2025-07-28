using System.Threading.Tasks;
using ClankyWeb.Data;
using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClankyWeb.Pages.Rezervace
{
    [Authorize(Roles = $"{NRole.Admin},{NRole.Autor}")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty] public ReservationEvent EventItem { get; set; }

        public EditModel(ApplicationDbContext db) => _db = db;

        public async Task OnGetAsync(int id)
        {
            EventItem = await _db.ReservationEvents.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            _db.ReservationEvents.Update(EventItem);
            await _db.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = EventItem.Id });
        }

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
