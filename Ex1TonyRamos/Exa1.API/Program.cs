using Exa1.API.Contratos.Repositorio;
using Exa1.API.Implementacion.Repositorio;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddTransient<IProveedorRepositorio, ProveedorRepositorio>();
        services.AddTransient<IProductoRepositorio, ProductoRepositorio>();
    })
    .Build();

host.Run();
