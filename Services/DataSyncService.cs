using System;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Services;

public class DataSyncService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly TimeSpan fetchInterval = TimeSpan.FromSeconds(20);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<BackendSvcHttpClient>();
            try
            {
                // Fetch from backend service
                var users = await httpClient.GetUsers();

                if (users != null && users.Count > 0)
                {
                    Console.WriteLine($"{users.Count} users fetched from backend service.");

                    var newUserEmails = users.Select(u => u.Email).ToList();
                    var usersToDelete = await DB.Find<User>()
                        .Match(u => !newUserEmails.Contains(u.Email))
                        .ExecuteAsync();


                    // Delete each user not in the new list
                    if (usersToDelete.Count != 0)
                    {
                        var IdsOfUsersToDelete = usersToDelete.Select(u => u.ID).ToList();
                        await DB.DeleteAsync<User>(IdsOfUsersToDelete);
                    }

                    foreach (var user in users)
                    {
                        await DB.Update<User>()
                            .Match(u => u.Email == user.Email)
                            .ModifyWith(user)
                            .Option(x => x.IsUpsert = true)
                            .ExecuteAsync();
                    }

                }

                // -------------------------------------------------
                var forms = await httpClient.GetForms();
                if (forms != null && forms.Count > 0)
                {
                    Console.WriteLine($"{forms.Count} forms fetched from backend service.");
                    var newFormTemplates = forms.Select(f => f.FormTemplateId).ToList();
                    var formsToDelete = await DB.Find<Form>()
                        .Match(f => !newFormTemplates.Contains(f.FormTemplateId))
                        .ExecuteAsync();

                    if (formsToDelete.Count != 0)
                    {
                        var IdsOfFormsToDelete = formsToDelete.Select(f => f.ID).ToList();
                        await DB.DeleteAsync<Form>(IdsOfFormsToDelete);
                    }

                    foreach (var form in forms)
                    {
                        await DB.Update<Form>()
                            .Match(f => f.FormTemplateId == form.FormTemplateId)
                            .ModifyWith(form)
                            .Option(x => x.IsUpsert = true)
                            .ExecuteAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while syncing users/forms: {ex.Message}");
            }
            // Wait for the next fetch
            await Task.Delay(fetchInterval, stoppingToken);
        }
    }
}
