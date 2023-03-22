using Bookstore.Entities;
using Bookstore.Services.DTO.Authors;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.DTO.Genres;
using Bookstore.Services.DTO.Languages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class BookstoreServices
    {
        private readonly BookstoreDbContext _dbContext;
        public BookstoreServices(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public async Task<List<BookDTO>> GetAll()
        {
            var books = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Language)
            .ToListAsync();
            return books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                CoverImage = b.CoverImage,
                Publisher = b.Publisher,
                ISBN = b.Isbn,
                PublishingYear = b.PublishingYear,
                Language = new LanguageDTO()
                {
                    Id = b.Language.Id,
                    Name = b.Language.Name
                },
                Authors = b.Authors.Select(ba => new AuthorsDTO
                {
                    Id = ba.Id,
                    FullName = $"{ba.FirstName} {ba.LastName}"
                }).ToList(),
                Genres = b.Genres.Select(bg => new GenresDTO
                {
                    Id = bg.Id,
                    Name = bg.Name
                }).ToList()
            }).ToList();
        }
        public async Task<List<BookDTO>> Search(string searchTerm)
        {
            var books = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Language)
            .Where(b => b.Title.Contains(searchTerm))
            .ToListAsync();
            return books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                CoverImage = b.CoverImage,
                Publisher = b.Publisher,
                ISBN = b.Isbn,
                PublishingYear = b.PublishingYear,
                Language = new LanguageDTO()
                {
                    Id = b.Language.Id,
                    Name = b.Language.Name
                },
                Authors = b.Authors.Select(ba => new AuthorsDTO
                {
                    Id = ba.Id,
                    FullName = $"{ba.FirstName} {ba.LastName}"
                }).ToList(),
                Genres = b.Genres.Select(bg => new GenresDTO
                {
                    Id = bg.Id,
                    Name = bg.Name
                }).ToList()
            }).ToList();
        }
        public async Task<BookDTO> GetById(int bookId)
        {
            var book = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Language)
            .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return null;
            }
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                CoverImage = book.CoverImage,
                Publisher = book.Publisher,
                ISBN = book.Isbn,
                PublishingYear = book.PublishingYear,
                Language = new LanguageDTO()
                {
                    Id = book.Language.Id,
                    Name = book.Language.Name
                },
                Authors = book.Authors.Select(ba => new AuthorsDTO
                {
                    Id = ba.Id,
                    FullName = $"{ba.FirstName} {ba.LastName}"
                }).ToList(),
                Genres = book.Genres.Select(bg => new GenresDTO
                {
                    Id = bg.Id,
                    Name = bg.Name
                }).ToList()
            };
        }

    }
}
