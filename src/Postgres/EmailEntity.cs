namespace EducationPromo.Postgres
{
	public class EmailEntity
	{
		public EmailEntity(string email)
		{
			Email = email;
			Date = DateTime.UtcNow;
		}

		public int Id { get; set; }

		public DateTime Date { get; set; }

		public string Email { get; set; }
	}
}