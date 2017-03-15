using Quartz;
using Quartz.Impl;

namespace TeamTracker
{
  public class TaskScheduler
  {
    //-------------------------------------------------------------------------

    public static void Start()
    {
      IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
      scheduler.Start();

      IJobDetail job = JobBuilder.Create<DailyResetStatusTask>().Build();

      ITrigger trigger = TriggerBuilder.Create()
        .WithDailyTimeIntervalSchedule
          ( s =>
              s.WithIntervalInHours( 24 )
               .OnEveryDay()
               .StartingDailyAt( TimeOfDay.HourAndMinuteOfDay( 0, 0 ) )
          )
        .Build();

      scheduler.ScheduleJob( job, trigger );
    }

    //-------------------------------------------------------------------------
  }
}