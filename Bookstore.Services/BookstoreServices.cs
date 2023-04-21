using AutoMapper;
using Bookstore.Entities;
using Bookstore.Services.DTO.Authors;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.DTO.CreateBook;
using Bookstore.Services.DTO.Genres;
using Bookstore.Services.DTO.Languages;
using Bookstore.Services.DTO.Order;
using Bookstore.Services.DTO.Revenue;
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
        private readonly IMapper _mapper;
        public BookstoreServices(BookstoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
        
        public async Task<BookDTO> Create(CreateBookDto bookDto)
        {
            var book = _mapper.Map<Bookstore.Entities.Book>(bookDto);
            book.Authors = _dbContext.Authors.Where(a =>
           bookDto.Authors.Contains(a.Id)).ToList();
            book.Genres = _dbContext.Genres.Where(g =>
           bookDto.Genres.Contains(g.Id)).ToList();
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BookDTO>(book);
        }
        public async Task<BookDTO> Update(int id, BookDTO bookDto)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }
            _mapper.Map(bookDto, book);
            _dbContext.Entry(book).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BookDTO>(book);
        }
        public async Task<BookDTO> UpdateCover(int id, byte[] coverImageData)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }
            book.CoverImage = coverImageData;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BookDTO>(book);
        }
        public async Task<bool> Delete(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<OrderDto>> GetBookPurchases(int bookId)
        {
            var book = await _dbContext.Books
            .Include(b => b.OrderDetails)
            .ThenInclude(od => od.Order)
            .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return null;
            }
            var orders = book.OrderDetails.Select(od => od.Order).ToList();
            return _mapper.Map<List<OrderDto>>(orders);
        }
        public async Task<List<RevenueSummaryDto>> GetRevenueSummary()
        {
            var books = await _dbContext.Books
            .Include(b => b.OrderDetails)
            .ThenInclude(od => od.Order)
            .ToListAsync();
            var revenueSummary = books.Select(book => new RevenueSummaryDto
            {
                BookId = book.Id,
                Revenues = book.OrderDetails
            .GroupBy(od => new {
                od.Order.OrderDateTime.Year,
                od.Order.OrderDateTime.Month
            })
            .Select(g => new RevenueDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalRevenue = g.Sum(od => od.UnitPrice * od.Quantity)
            })
            .OrderByDescending(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList()
            })
            .ToList();
            return revenueSummary;
        }

    }
}
