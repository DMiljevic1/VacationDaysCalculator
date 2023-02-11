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
			if (currentDate.Day.Equals(1) && currentDate.Month.Equals(1))
			{
				await Console.Out.WriteLineAsync("Updating Holiday Table.");
				InsertHolidaysInDatabase(await GetHolidaysForCurrentYear());
				await Console.Out.WriteLineAsync("Holiday Table Updated!");
			}
		}
		private async Task<List<HolidayDetails>> GetHolidaysForCurrentYear()
		{
			int currentYear = DateTime.Now.Year;
			return await _httpClient.GetFromJsonAsync<List<HolidayDetails>>($"{BaseUrlForHolidayApi}/{currentYear}/HR");
		}
		private async Task InsertHolidaysInDatabase(List<HolidayDetails> holidays)
		{
			var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
			httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(holidays), Encoding.UTF8, "application/json");
			await _httpClient.SendAsync(httpPostRequest);
		}
	}
}
