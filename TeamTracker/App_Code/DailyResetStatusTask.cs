using System;
using Quartz;

namespace TeamTracker
{
  class DailyResetStatusTask : IJob
  {
    //-------------------------------------------------------------------------

    public void Execute( IJobExecutionContext context )
    {
      try
      {
        Database.ExecSql( "TRUNCATE TABLE PeopleStatus" );
        Log.LogToFile( "Status reset performed." );
      }
      catch( Exception ex )
      {
        Log.LogToFile( ex );
      }
    }

    //-------------------------------------------------------------------------
  }
}