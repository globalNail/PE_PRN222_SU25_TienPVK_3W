using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PantherPeManagement.Pages
{
    public abstract class BasePageModel : PageModel
    {
        protected bool IsUserLoggedIn()
        {
            var email = HttpContext.Session.GetString("Email");
            return !string.IsNullOrEmpty(email);
        }

        protected bool HasManagerRole()
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            return roleId == 2;
        }

        protected IActionResult? CheckAuthenticationAndAuthorization(bool requireManagerRole = false)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToPage("/Login");
            }

            if (requireManagerRole && !HasManagerRole())
            {
                return null; 
            }

            return null;
        }

        protected void SetUnauthorizedError()
        {
            TempData["ErrorMessage"] = "You have no permission to access this function!";
        }
    }
}
