using DomainModel.DtoModels;
using DomainModel.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacationScheduler.Jobs
{
	public class CreateFirstUser : IJob
	{
		HttpClient _httpClient = new HttpClient();
		private readonly string BaseApiUrl = "https://localhost:7058/api/User";
		public async Task Execute(IJobExecutionContext context)
		{
			if (GetUsers().Result.Count == 0)
			{
				await Console.Out.WriteLineAsync("Creating first user.");
				await AddUser(await CreateUser());
				await Console.Out.WriteLineAsync("First user succesfully created!");
			}
		}
		private async Task AddUser(User user)
		{
			var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseApiUrl}/addUserViaScheduler");
			httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
			await _httpClient.SendAsync(httpPostRequest);
		}
		private async Task<List<User>> GetUsers()
		{
			return await _httpClient.GetFromJsonAsync<List<User>>($"{BaseApiUrl}/getUsersViaScheduler");
		}
		private async Task<User> CreateUser()
		{
			User user = new User();
			user.UserName = "Admin";
			user.FirstName = "Adminko";
			user.LastName = "Adminkovic";
			user.Email = "admin@nais.hr";
			user.Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";
			user.Role = "Admin";
			return user;
		}
	}
}
