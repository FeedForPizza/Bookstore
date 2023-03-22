using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.DTO.Books
{
    internal class BookSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
