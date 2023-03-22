using Bookstore.Services;
using Bookstore.Services.DTO.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BookstoreServices _booksService;
        public string SearchTerm { get; set; }
        public IList<BookDTO> Books { get; set; }

        public IndexModel(BookstoreServices booksService)
        {
            _booksService = booksService;
        }

        

        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                Books = await _booksService.GetAll();
            }
            else
            {
                Books = await _booksService.Search(searchTerm);
            }
            return Page();
        }

    }
}