using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OA.Entities;

var host = new HostBuilder()
	.ConfigureFunctionsWebApplication()
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();

		// Connect database
		string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:OAConnection");
		services.AddDbContext<OAContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});
	})
	.Build();

host.Run();
