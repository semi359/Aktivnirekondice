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
            // Zajist�me, �e Form nen� null
            Form = new KontaktForm();
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                // ponech�me Form s hodnotami, ModelState vypsan� chyby
                return;
            }

            // TODO: Sem dejte odesl�n� e-mailu / ulo�en� do DB

            IsSubmitted = true;
            // Ulo��me hodnotu odeslan�ho e-mailu do zvl�tn�ho pole,
            // abychom Form mohli vy�istit a p�esto m�li e-mail pro modal
            SubmittedEmail = Form.Email;

            ModelState.Clear();
            // Prostor pro nov� pr�zdn� formul��
            Form = new KontaktForm();
        }

        // Toto pole uchov� e-mail pro modal
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
