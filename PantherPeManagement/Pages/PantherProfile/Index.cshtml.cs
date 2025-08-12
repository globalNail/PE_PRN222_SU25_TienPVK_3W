using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PantherPeManagement.Pages.PantherProfile
{
    public class IndexModel : BasePageModel
    {
        private readonly IPantherProfileService _pantherProfileService;

        public IndexModel(IPantherProfileService pantherProfileService)
        {
            _pantherProfileService = pantherProfileService;
        }

        public IEnumerable<DAL.PantherProfile> PantherProfiles { get; set; } = new List<DAL.PantherProfile>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 3;

        [BindProperty(SupportsGet = true)]
        public double? SearchWeight { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchPantherTypeName { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsSearching { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            var authResult = CheckAuthenticationAndAuthorization();
            if (authResult != null)
            {
                return authResult;
            }

            if (IsSearching && (SearchWeight.HasValue || !string.IsNullOrEmpty(SearchPantherTypeName)))
            {
                var searchResult = await _pantherProfileService.SearchPantherProfilesAsync(pageNumber, PageSize, SearchWeight, SearchPantherTypeName);
                PantherProfiles = searchResult.pantherProfiles;
                TotalCount = searchResult.totalCount;
                TotalPages = searchResult.totalPages;
                HasPrevious = searchResult.hasPrevious;
                HasNext = searchResult.hasNext;
            }
            else
            {
                var pagedResult = await _pantherProfileService.GetPagedPantherProfilesAsync(pageNumber, PageSize);
                PantherProfiles = pagedResult.pantherProfiles;
                TotalCount = pagedResult.totalCount;
                TotalPages = pagedResult.totalPages;
                HasPrevious = pagedResult.hasPrevious;
                HasNext = pagedResult.hasNext;
            }

            CurrentPage = pageNumber;
            return Page();
        }

        public IActionResult OnPostSearch()
        {
            IsSearching = true;
            return RedirectToPage(new
            {
                pageNumber = 1,
                SearchWeight,
                SearchPantherTypeName,
                IsSearching = true
            });
        }

        public IActionResult OnPostClearSearch()
        {
            SearchWeight = null;
            SearchPantherTypeName = null;
            IsSearching = false;
            return RedirectToPage(new { pageNumber = 1 });
        }
    }
}
