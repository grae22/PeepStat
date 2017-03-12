using System;
using Quartz;

public class DailyResetStatusTask : IJob
{
  //---------------------------------------------------------------------------

  public void Execute( IJobExecutionContext context )
  {
    try
    {
      Database.ExecSql( "TRUNCATE TABLE PeopleStatus" );
    }
    catch( Exception )
    {
      // Ignore it.
    }
  }

  //---------------------------------------------------------------------------
}