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
	public class FillHolidayTableJob : IJob
	{
		HttpClient _httpClient = new HttpClient();
		private readonly string BaseUrlForHolidayApi = "https://date.nager.at/api/v3/PublicHolidays";
		private readonly string BaseApiUrl = "https://localhost:7058/api/Holiday";
		public async Task Execute(IJobExecutionContext context)
		{
			var currentDate = DateTime.Today.Date;
			var currentYear = DateTime.Today.Year;
			var nextYear = currentYear + 1;
			if(GetHolidaysFromDatabase().Result.Count == 0)
			{
				await Console.Out.WriteLineAsync("Updating Holiday Table.");
				await InsertHolidaysInDatabase(await GetHolidays(currentYear));
				await Console.Out.WriteLineAsync("Holiday Table Updated!");
			}
			if (currentDate.Day.Equals(1) && currentDate.Month.Equals(7))
			{
				await Console.Out.WriteLineAsync("Updating Holiday Table.");
				await InsertHolidaysInDatabase(await GetHolidays(nextYear));
				await Console.Out.WriteLineAsync("Holiday Table Updated!");
			}
		}
		private async Task<List<HolidayDetails>> GetHolidays(int year)
		{
			return await _httpClient.GetFromJsonAsync<List<HolidayDetails>>($"{BaseUrlForHolidayApi}/{year}/HR");
		}
		private async Task InsertHolidaysInDatabase(List<HolidayDetails> holidays)
		{
			var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
			httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(holidays), Encoding.UTF8, "application/json");
			await _httpClient.SendAsync(httpPostRequest);
		}
		private async Task<List<Holiday>> GetHolidaysFromDatabase()
		{
			return await _httpClient.GetFromJsonAsync<List<Holiday>>(BaseApiUrl);
		}
	}
}
