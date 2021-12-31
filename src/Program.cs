using Autofac;
using Autofac.Extensions.DependencyInjection;
using EducationPromo.Postgres;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EducationPromo
{
	public class Program
	{
		private const string ConnectionStringVariable = "CONNECTIONSTRING";

		public static void Main(string[] args)
		{
			Console.Title = "MyJetEducation EducationPromo";

			string connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				ShowError($"Error! Environment variable \"{ConnectionStringVariable}\" not set.");
				return;
			}

			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Host
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.UseSerilog((_, configuration) => configuration.WriteTo.Console())
				.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.Register(_ =>
					new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(connectionString).Options)));

			builder.Services.AddRazorPages();

			WebApplication app = builder.Build();

			app.Services.GetService<DatabaseContext>()?.Database.Migrate();

			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.MapControllers();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
			});

			app.Run();
		}

		private static void ShowError(string message)
		{
			Console.WriteLine(message);
			throw new Exception(message);
		}
	}
}