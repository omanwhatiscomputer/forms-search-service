using System.Net;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Data;
using SearchService.Services;

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    var cwd = Directory.GetCurrentDirectory();
    DotEnv.Load(Path.Combine(cwd, ".env"));
}
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://forms-frontend-psi.vercel.app")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); ;
        });
});


// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddHttpClient<BackendSvcHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddHostedService<DataSyncService>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();




app.MapControllers();

try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();


static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));