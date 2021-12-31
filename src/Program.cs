using Autofac;
using Autofac.Extensions.DependencyInjection;
using EducationPromo.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EducationPromo
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

			builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
				containerBuilder.Register(_ =>
				{
					var databaseContext = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(builder.Configuration["PostgresConnectionString"]).Options);
					databaseContext.Database.Migrate();
					return databaseContext;
				}));

			builder.Services.AddRazorPages();

			WebApplication app = builder.Build();

			if (!app.Environment.IsDevelopment())
				app.UseExceptionHandler("/Error");

			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseRouting();
			app.MapControllers();
			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});

			app.Run();
		}
	}
}