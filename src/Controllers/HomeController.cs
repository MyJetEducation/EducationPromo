using EducationPromo.Models;
using EducationPromo.Postgres;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationPromo.Controllers
{
	[Route("/api/home")]
	public class HomeController : ControllerBase
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DatabaseContext _databaseContext;

		public HomeController(ILogger<HomeController> logger, DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
		{
			_logger = logger;
			_databaseContext = new DatabaseContext(dbContextOptionsBuilder.Options);
		}

		[Route("add")]
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] EmailRequest request)
		{
			if (!ModelState.IsValid)
				return new OkObjectResult(-1); //not valid

			string requestEmail = request.Email;
			if (await AlreadyExists(requestEmail))
				return new OkObjectResult(-2); //already been added

			try
			{
				DbSet<EmailEntity> databaseContextEntities = _databaseContext.Entities;
				databaseContextEntities.Add(new EmailEntity(requestEmail));
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				_logger.LogError(e, e.Message);
				throw;
			}

			return new OkObjectResult(0);
		}

		private async Task<bool> AlreadyExists(string email)
		{
			try
			{
				return await _databaseContext.Entities.AnyAsync(entity => entity.Email == email);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				_logger.LogError(e, e.Message);
				throw;
			}
		}
	}
}