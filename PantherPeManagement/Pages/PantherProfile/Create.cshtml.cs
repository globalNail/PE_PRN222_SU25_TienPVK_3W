using BLL.DTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PantherPeManagement.Pages;

namespace PantherPeManagement.Pages.PantherProfile
{
    public class CreateModel : BasePageModel
    {
        private readonly IPantherProfileService _pantherProfileService;
        private readonly IPantherTypeService _pantherTypeService;

        public CreateModel(IPantherProfileService pantherProfileService, IPantherTypeService pantherTypeService)
        {
            _pantherProfileService = pantherProfileService;
            _pantherTypeService = pantherTypeService;
        }

        [BindProperty]
        public PantherProfileRequest CreatePantherProfileRequest { get; set; } = new();
        public SelectList? PantherTypes { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet()
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

            await LoadPantherTypesAsync();
            return Page();
        }

        private async Task LoadPantherTypesAsync()
        {
            var lionTypes = await _pantherTypeService.GetAllPantherTypesAsync();
            PantherTypes = new SelectList(lionTypes, "PantherTypeId", "PantherTypeName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var authResult = CheckAuthenticationAndAuthorization(requireManagerRole: true);
            if (authResult != null)
            {
                return authResult;
            }

            if (!HasManagerRole())
            {
                ErrorMessage = "You have no permission to access this function!";
                await LoadPantherTypesAsync();
                return Page();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadPantherTypesAsync();
                    return Page();
                }
                await _pantherProfileService.CreatePantherProfileAsync(CreatePantherProfileRequest);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while creating the panther profile: {ex.Message}";
                await LoadPantherTypesAsync();
                return Page();
            }
        }
    }
}
