using DomainModel.Models;

namespace VacationDaysCalculatorBlazorServer.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseApiUrl = "https://localhost:44324/api/User";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>(BaseApiUrl);
        }
    }
}
