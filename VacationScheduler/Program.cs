using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace VacationScheduler
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

			// Grab the Scheduler instance from the Factory
			StdSchedulerFactory factory = new StdSchedulerFactory();
			IScheduler scheduler = await factory.GetScheduler();

			// and start it off
			await scheduler.Start();

			// define the job and tie it to our HelloJob class
			IJobDetail job = JobBuilder.Create<SetVacationStatusJob>()
				.WithIdentity("job1", "group1")
				.Build();

			// Trigger the job to run now, and then repeat every 10 seconds
			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("trigger1", "group1")
				.StartNow()
				.WithSimpleSchedule(x => x
					.WithIntervalInMinutes(1)
					.RepeatForever())
				.Build();

			// Tell Quartz to schedule the job using our trigger
			await scheduler.ScheduleJob(job, trigger);

			// some sleep to show what's happening
			await Task.Delay(TimeSpan.FromSeconds(120));

			Console.ReadKey();
		}

		// simple log provider to get something to the console
		private class ConsoleLogProvider : ILogProvider
		{
			public Logger GetLogger(string name)
			{
				return (level, func, exception, parameters) =>
				{
					if (level >= LogLevel.Info && func != null)
					{
						Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
					}
					return true;
				};
			}

			public IDisposable OpenNestedContext(string message)
			{
				throw new NotImplementedException();
			}

			public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
			{
				throw new NotImplementedException();
			}
		}
	}

	public class SetVacationStatusJob : IJob
	{
		HttpClient _httpClient = new HttpClient();
		private readonly string BaseApiUrl = "https://localhost:7058/api/Employee";

		public async Task Execute(IJobExecutionContext context)
		{
			await Console.Out.WriteLineAsync("Greetings from HelloJob!");
			var currentDate = DateTime.Today.Date;
			await SetVacationStatus(currentDate);
		}

		public async Task SetVacationStatus(DateTime currentDate)
		{
			var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, BaseApiUrl + "/updateVacationStatus");
			httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(currentDate), Encoding.UTF8, "application/json");
			await _httpClient.SendAsync(httpPutRequest);
		}
	}
}