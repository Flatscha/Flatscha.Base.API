using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Flatscha.Base.API.Extensions;
using Flatscha.Base.API.ExampleAPI.Endpoints;
using Flatscha.Base.API.ExampleAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBaseApi("Example API");

builder.Services.AddExampleApi(builder.Configuration);

builder.Services.AddAuthorization();

builder.Logging.ClearProviders();
builder.Logging.AddNLogWeb();

builder.Host.UseWindowsService();

var app = builder.Build();

app.MapEndpoints();

app.UseBaseApi();

app.UseRouting();
app.UseAuthorization();

app.Run();

public partial class Program { }