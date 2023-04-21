﻿using Bookstore.Services;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.DTO.CreateBook;
using Bookstore.Services.DTO.Order;
using Bookstore.Services.DTO.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookstoreServices _booksService;
        public BooksController(BookstoreServices booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _booksService.GetAll();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var book = await _booksService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPost]
        [Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<BookDTO>> CreateBook([FromBody] CreateBookDto
bookDto)
        {
            var createdBook = await _booksService.Create(bookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id },
           createdBook);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "operator, admin")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDTO
        bookDto)
        {
            var updatedBook = await _booksService.Update(id, bookDto);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPost("{id}/cover")]
        [Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<BookDTO>> UploadCoverImage(int id, IFormFile
        coverImage)
        {
            if (coverImage == null)
            {
                return BadRequest("Cover image is required.");
            }
            if (coverImage.Length == 0)
            {
                return BadRequest("Cover image is empty.");
            }
            byte[] coverImageData;
            using (var memoryStream = new MemoryStream())
            {
                await coverImage.CopyToAsync(memoryStream);
                coverImageData = memoryStream.ToArray();
            }
            var updatedBook = await _booksService.UpdateCover(id, coverImageData);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var isDeleted = await _booksService.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("{id}/purchases")]
        [Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetBookPurchases(int
        id)
        {
            var purchases = await _booksService.GetBookPurchases(id);
            if (purchases == null)
            {
                return NotFound();
            }
            return Ok(purchases);
        }
        [HttpGet("revenue-summary")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<RevenueSummaryDto>>>
        GetRevenueSummary()
        {
            var revenueSummary = await _booksService.GetRevenueSummary();
            return Ok(revenueSummary);
        }

    }
}
