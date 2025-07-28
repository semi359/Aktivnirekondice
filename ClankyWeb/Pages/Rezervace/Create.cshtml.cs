using System.Threading.Tasks;
using ClankyWeb.Data;
using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClankyWeb.Pages.Rezervace
{
    [Authorize(Roles = $"{NRole.Admin},{NRole.Autor}")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty] public ReservationEvent EventItem { get; set; }
        public CreateModel(ApplicationDbContext db) => _db = db;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            _db.ReservationEvents.Add(EventItem);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Rezervace/Rezervace");
        }
    }
}
