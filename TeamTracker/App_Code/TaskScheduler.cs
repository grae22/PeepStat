using System;
using Quartz;
using Quartz.Impl;

namespace TeamTracker
{
  public class TaskScheduler
  {
    //-------------------------------------------------------------------------

    static ITrigger statusResetTrigger;

    //-------------------------------------------------------------------------

    public static void Start()
    {
      SettingsManager Settings = new SettingsManager();
      string resetTimeAsString = Settings.Setting[ "DailyStatusResetTime" ];
      DateTime resetTime = DateTime.Parse( resetTimeAsString );

      IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
      scheduler.Start();

      IJobDetail job = JobBuilder.Create<DailyResetStatusTask>().Build();

      ITrigger trigger = TriggerBuilder.Create()
        .WithDailyTimeIntervalSchedule
          ( s =>
              s.WithIntervalInHours( 24 )
               .OnEveryDay()
               .StartingDailyAt(
                 TimeOfDay.HourAndMinuteOfDay(
                   resetTime.Hour,
                   resetTime.Minute ) )
          )
        .Build();

      if( statusResetTrigger == null )
      {
        scheduler.ScheduleJob( job, trigger );
      }
      else
      {
        scheduler.RescheduleJob( statusResetTrigger.Key, trigger );
      }

      statusResetTrigger = trigger;

      Log.LogToFile( "Daily status reset scheduled for " + resetTime.ToShortTimeString() );
    }

    //-------------------------------------------------------------------------
  }
}