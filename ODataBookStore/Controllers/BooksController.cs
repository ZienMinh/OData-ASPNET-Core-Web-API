using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataBookStore.EDM;
using System;

namespace ODataBookStore.Controllers
{
	//[Route("api/[Controller]")]
	//[ApiController]
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

			_context.Books.Add(book);
			_context.SaveChanges();
			return Created(book);
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
