using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using VacationScheduler.Jobs;

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

			// call jobs
			IJobDetail job = JobBuilder.Create<SetVacationStatusJob>()
				.WithIdentity("job1", "group1")
				.Build();

            IJobDetail job2 = JobBuilder.Create<SetRemainingVacationJob>()
                .WithIdentity("job2", "group2")
                .Build();

			IJobDetail job3 = JobBuilder.Create<FillHolidayTableJob>()
				.WithIdentity("job3", "group3")
				.Build();

			IJobDetail job4 = JobBuilder.Create<CreateFirstUser>()
				.WithIdentity("job4", "group4")
				.Build();

			// Trigger the job
			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("trigger1", "group1")
				.StartNow()
				.WithSimpleSchedule(x => x
					.WithIntervalInHours(24)
					.RepeatForever())
				.Build();

            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("trigger2", "group2")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

			ITrigger trigger3 = TriggerBuilder.Create()
			   .WithIdentity("trigger3", "group3")
			   .StartNow()
			   .WithSimpleSchedule(x => x
				   .WithIntervalInHours(24)
				   .RepeatForever())
			   .Build();

			ITrigger trigger4 = TriggerBuilder.Create()
				.WithIdentity("trigger4", "group4")
				.StartNow()
				.Build();

			// Tell Quartz to schedule the job using our trigger
			await scheduler.ScheduleJob(job, trigger);
            await scheduler.ScheduleJob(job2, trigger2);
			await scheduler.ScheduleJob(job3, trigger3);
			await scheduler.ScheduleJob(job4, trigger4);

			// some sleep
			await Task.Delay(TimeSpan.FromSeconds(120));

			Console.ReadKey();
		}
	}
}