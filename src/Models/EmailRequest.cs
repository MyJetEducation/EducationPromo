using System.ComponentModel.DataAnnotations;

namespace EducationPromo.Models
{
	public class EmailRequest
	{
		[Required]
		[StringLength(320, MinimumLength = 3)]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }
	}
}