using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TH.Dal.Enums;

namespace TH.Dal.Extentions;

public static class ServiceCollectionExtensions
{
	private static void ReloadTypes(string connectionString)
	{
		var connection = new NpgsqlConnection(connectionString);
		connection.Open();
		connection.ReloadTypes();
	}

	public static IServiceProvider ApplyMigrations<T>(this IServiceProvider serviceProvider, string connectionString) where T : DbContext
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
			ReloadTypes(connectionString);
		}

		return serviceProvider;
	}

	public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection serviceCollection, string connectionString) where T : DbContext
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
		dataSourceBuilder.MapEnum<DocumentTypes>();
		var dataSource = dataSourceBuilder.Build();

		serviceCollection.AddDbContext<T>(options => options.UseNpgsql(dataSource));

		return serviceCollection;
	}
}
