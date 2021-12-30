using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EducationPromo.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly DatabaseContext _databaseContext;

		[BindProperty]
		[Required]
		[StringLength(320, MinimumLength = 3)]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		public IndexModel(ILogger<IndexModel> logger, DatabaseContext databaseContext)
		{
			_logger = logger;
			_databaseContext = databaseContext;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (await AlreadyExists())
			{
				ViewData["error"] = "Email address has already been added";
				return Page();
			}

			try
			{
				DbSet<EmailEntity> databaseContextEntities = _databaseContext.Entities;
				databaseContextEntities.Add(new EmailEntity(Email));
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				_logger.LogError(e, e.Message);
				throw;
			}

			ViewData["message"] = "Email address added";

			return Page();
		}

		private async Task<bool> AlreadyExists()
		{
			try
			{
				return await _databaseContext.Entities.AnyAsync(entity => entity.Email == Email);
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