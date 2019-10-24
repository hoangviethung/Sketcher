using Quartz;
using Quartz.Impl;

namespace MainProject.Framework.ScheduleTask
{
    public class QuartzTask
    {
        public static void StartScheduler()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            IScheduler scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();
            
            var dailyTrigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule
                (s => s.WithIntervalInHours(24)
                     .OnEveryDay()
                     .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 1))
                ).Build();

            var job1 = JobBuilder.Create<DeleteImagesNotUseJob>().WithIdentity("job1", "group1").Build();

            scheduler.ScheduleJob(job1, dailyTrigger);
        }
    }
}
