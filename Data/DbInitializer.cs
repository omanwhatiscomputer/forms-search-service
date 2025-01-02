using System;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;


namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGODB_CONN_STRING")));
        await DB.Index<User>().DropAllAsync();
        await DB.Index<Form>().DropAllAsync();
        await DB.Index<User>()
            .Key(u => u.NormalizedName, KeyType.Text)
            .Key(u => u.Email, KeyType.Text)
            .Key(u => u.FirstName, KeyType.Text)
            .Key(u => u.LastName, KeyType.Text)
            .CreateAsync();
        await DB.Index<Form>()
            .Key(f => f.Title, KeyType.Text)
            .Key(f => f.Description, KeyType.Text)
            .CreateAsync();



        // using var scope = app.Services.CreateScope();
        // var httpClient = scope.ServiceProvider.GetRequiredService<BackendSvcHttpClient>();

        // var users = await httpClient.GetUsers();
        // // TODO: Add forms

        // Console.WriteLine(users.Count + " returned from backend service");
        // if (users.Count > 0) await DB.SaveAsync(users);

        await DB.DeleteAsync<User>(_ => true);
        await DB.DeleteAsync<Form>(_ => true);
    }
}
