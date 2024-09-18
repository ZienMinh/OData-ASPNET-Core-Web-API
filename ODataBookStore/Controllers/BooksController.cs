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
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get()
		{
			var books = _context.Books.Include(b => b.Press).AsQueryable();
			return Ok(books);
		}

		[HttpGet("{key}")]
		[EnableQuery]
		public IActionResult Get(int key, string version)
		{
			var book = _context.Books.FirstOrDefault(b => b.Id == key);
			return Ok(book);
		}

		[HttpPost]
		[EnableQuery]
		public IActionResult Post([FromBody] Book book)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				if (book.Press != null)
				{
					if (book.Press.Id == 0)
					{
						_context.Presses.Add(book.Press);
					}
					else
					{
						var existingPress = _context.Presses.Find(book.Press.Id);
						if (existingPress == null)
						{
							return BadRequest("Invalid Press ID");
						}
						_context.Entry(existingPress).CurrentValues.SetValues(book.Press);
					}
				}

				_context.Books.Add(book);
				_context.SaveChanges();

				return Created(book);
			}
			catch (Exception ex)
			{
				// Log the full exception
				Console.WriteLine($"Error: {ex}");
				return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
			}
		}

		[HttpDelete]
		[EnableQuery]
		public IActionResult Delete([FromBody] int key)
		{
			var book = _context.Books.FirstOrDefault(b => b.Id == key);
			if (book == null) return NotFound();

			_context.Books.Remove(book);
			_context.SaveChanges();

			return Ok();

		}
	}
}
