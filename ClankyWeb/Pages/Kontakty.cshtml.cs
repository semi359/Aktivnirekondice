using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClankyWeb.Pages
{
    public class KontaktyModel : PageModel
    {
        [BindProperty]
        public KontaktForm Form { get; set; }

        public bool IsSubmitted { get; set; }

        public void OnGet()
        {
            // Zajistíme, že Form není null
            Form = new KontaktForm();
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                // ponecháme Form s hodnotami, ModelState vypsané chyby
                return;
            }

            // TODO: Sem dejte odeslání e-mailu / uložení do DB

            IsSubmitted = true;
            // Uložíme hodnotu odeslaného e-mailu do zvláštního pole,
            // abychom Form mohli vyèistit a pøesto mìli e-mail pro modal
            SubmittedEmail = Form.Email;

            ModelState.Clear();
            // Prostor pro nový prázdný formuláø
            Form = new KontaktForm();
        }

        // Toto pole uchová e-mail pro modal
        public string SubmittedEmail { get; set; }
    }

    public class KontaktForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool NotRobot { get; set; }
    }
}
