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
	public class PressesController : ODataController
	{
		private ApplicationDbContext _context;

		public PressesController(ApplicationDbContext context)
		{
			_context = context;
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_context.Presses);
		}

	}
}
