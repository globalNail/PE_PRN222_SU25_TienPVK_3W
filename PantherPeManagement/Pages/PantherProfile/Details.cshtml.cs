using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PantherPeManagement.Pages.PantherProfile
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.Su25pantherDbContext _context;

        public DetailsModel(DAL.Su25pantherDbContext context)
        {
            _context = context;
        }

        public DAL.PantherProfile PantherProfile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pantherprofile = await _context.PantherProfiles.FirstOrDefaultAsync(m => m.PantherProfileId == id);
            if (pantherprofile == null)
            {
                return NotFound();
            }
            else
            {
                PantherProfile = pantherprofile;
            }
            return Page();
        }
    }
}
