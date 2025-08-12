using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PantherPeManagement.Pages.PantherProfile
{
    public class EditModel : PageModel
    {
        private readonly DAL.Su25pantherDbContext _context;

        public EditModel(DAL.Su25pantherDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DAL.PantherProfile PantherProfile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pantherprofile =  await _context.PantherProfiles.FirstOrDefaultAsync(m => m.PantherProfileId == id);
            if (pantherprofile == null)
            {
                return NotFound();
            }
            PantherProfile = pantherprofile;
           ViewData["PantherTypeId"] = new SelectList(_context.PantherTypes, "PantherTypeId", "PantherTypeId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PantherProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PantherProfileExists(PantherProfile.PantherProfileId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PantherProfileExists(int id)
        {
            return _context.PantherProfiles.Any(e => e.PantherProfileId == id);
        }
    }
}
