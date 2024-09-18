using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataBookStore.EDM;
using System;

namespace ODataBookStore.Controllers
{
	public class BooksController : ODataController
	{
		private ApplicationDbContext _context;

		public BooksController(ApplicationDbContext context)
		{
			_context = context;
		}

		[EnableQuery(PageSize = 10)]
		public async Task<IActionResult> Get()
		{
			var books = await _context.Books.ToListAsync();
			return Ok(books);
		}

		[EnableQuery]
		public async Task<IActionResult> Get(int key, string version)
		{
			var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == key);

			if (book == null)
			{
				return NotFound();
			}

			return Ok(book);
		}

		[EnableQuery]
		public async Task<IActionResult> Post([FromBody] Book book)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Books.Add(book);
			await _context.SaveChangesAsync();

			return Created(book);
		}

		[EnableQuery]
		public async Task<IActionResult> Delete(int key)
		{
			var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == key);

			if (book == null)
			{
				return NotFound();
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
