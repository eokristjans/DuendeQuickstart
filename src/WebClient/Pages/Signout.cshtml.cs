using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class SignoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // This is one of many interesting methods on the RazorPages.PageModela!
            // This will clear the local cookie and then redirect to the IdentityServer,
            // who will clear its cookies and then give the user a link to return back to the web application.
            return SignOut("Cookies", "oidc");
        }
    }
}
