using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interfaces;
using PantherPeManagement.Pages;

namespace PantherPeManagement.Pages.PantherProfile
{
    public class DeleteModel : BasePageModel
    {
        private readonly IPantherProfileService _pantherProfileService;

        public DeleteModel(IPantherProfileService pantherProfileService)
        {
            _pantherProfileService = pantherProfileService;
        }

        [BindProperty]
        public DAL.PantherProfile PantherProfile { get; set; } = default!;
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var authResult = CheckAuthenticationAndAuthorization(requireManagerRole: true);
            if (authResult != null)
            {
                return authResult;
            }

            if (!HasManagerRole())
            {
                ErrorMessage = "You have no permission to access this function!";
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var pantherProfile = await _pantherProfileService.GetPantherProfileByIdAsync(id.Value);
                if (pantherProfile == null)
                {
                    return NotFound();
                }

                PantherProfile = pantherProfile;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while loading the panther profile: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var authResult = CheckAuthenticationAndAuthorization(requireManagerRole: true);
            if (authResult != null)
            {
                return authResult;
            }

            if (!HasManagerRole())
            {
                ErrorMessage = "You have no permission to access this function!";
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _pantherProfileService.DeletePantherProfileAsync(id.Value);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while deleting the panther profile: {ex.Message}";
                var pantherProfile = await _pantherProfileService.GetPantherProfileByIdAsync(id.Value);
                if (pantherProfile != null)
                {
                    PantherProfile = pantherProfile;
                }
                return Page();
            }
        }
    }
}
