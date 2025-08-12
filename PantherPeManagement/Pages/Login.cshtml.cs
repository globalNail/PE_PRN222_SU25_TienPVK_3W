using BLL.DTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PantherPeManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IPantherAccountService _pantherAccountService;

        public LoginModel(IPantherAccountService pantherAccountService)
        {
            _pantherAccountService = pantherAccountService;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            var email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/PantherProfile/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var pantherAccount = await _pantherAccountService.Authenticate(LoginRequest);
            if (pantherAccount == null)
            {
                ErrorMessage = "Invalid Email or Password!";
                return Page();
            }
            // Set authentication cookie or session here
            HttpContext.Session.SetInt32("AccountId", pantherAccount.AccountId);
            HttpContext.Session.SetString("Email", pantherAccount.Email);
            HttpContext.Session.SetInt32("RoleId", pantherAccount.RoleId);

            return RedirectToPage("/PantherProfile/Index");
        }
    }
}
