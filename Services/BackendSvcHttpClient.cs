using System;
using SearchService.Entities;

namespace SearchService.Services;

public class BackendSvcHttpClient(HttpClient httpClient)
{
    public async Task<List<User>> GetUsers()
    {
        var backendUrl = Environment.GetEnvironmentVariable("BACKEND_SVC_BASE_URL");
        var url = backendUrl + "/api/user/all";
        var users = await httpClient.GetFromJsonAsync<List<User>>(url);

        return users ?? [];

    }
    public async Task<List<Form>> GetForms()
    {
        var backendUrl = Environment.GetEnvironmentVariable("BACKEND_SVC_BASE_URL");
        var url = backendUrl + "/api/form/all";

        var forms = await httpClient.GetFromJsonAsync<List<Form>>(url);

        return forms ?? [];
        
    }


}
