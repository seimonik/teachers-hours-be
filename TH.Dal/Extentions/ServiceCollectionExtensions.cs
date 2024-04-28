using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace TH.Dal.Extentions;

public static class ServiceCollectionExtensions
{
	private static void ReloadTypes(this DatabaseFacade database)
	{
		var connection = new NpgsqlConnection(database.GetConnectionString());
		connection.Open();
		connection.ReloadTypes();
	}

	public static IServiceProvider ApplyMigrations<T>(this IServiceProvider serviceProvider) where T : DbContext
	{
		if (serviceProvider == null)
		{
			throw new ArgumentNullException(nameof(serviceProvider));
		}

		using IServiceScope serviceScope = serviceProvider.CreateScope();
		using T dbContext = serviceScope.ServiceProvider.GetRequiredService<T>();

		if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
		{
			dbContext.Database.Migrate();
			dbContext.Database.ReloadTypes();
		}

		return serviceProvider;
	}
}
