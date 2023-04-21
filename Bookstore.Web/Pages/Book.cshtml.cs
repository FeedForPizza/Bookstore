using Bookstore.Services;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Web.Pages
{
    public class BookModel : PageModel
    {
        private readonly BookstoreServices _booksService;
        private readonly OpenLibraryService _openLibraryService;
        private readonly GoogleBooksService _googleBooksService;
        private readonly OrdersService _ordersService;
        public BookDTO? Book { get; set; }
        public OpenLibraryBookDto OpenLibraryDetails { get; set; }
        public Item GoogleDetails { get; set; }
		public bool IsAddedToCart { get; private set; }

		public BookModel(BookstoreServices booksService, OpenLibraryService
	   openLibraryService, GoogleBooksService googleBooksService, OrdersService ordersService)
		{
			_booksService = booksService;
			_openLibraryService = openLibraryService;
			_googleBooksService = googleBooksService;
			_ordersService = ordersService;
		}
		public async Task<IActionResult> OnGetAsync(int id)
        {
            Book = await _booksService.GetById(id);
            if (Book == null)
            {
                return NotFound();
            }
            this.OpenLibraryDetails = await
           _openLibraryService.GetBookAsync(Book.ISBN);
            this.GoogleDetails = await
_googleBooksService.SearchBookByISBN(Book.ISBN);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int bookId)
        {
            await _ordersService.AddBook(User.Identity.Name, bookId);
            this.IsAddedToCart = true;
            return RedirectToPage("/Cart");
        }
    }

}
