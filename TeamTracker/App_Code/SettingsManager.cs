using System.Collections.Generic;
using System.Data.SqlClient;

namespace TeamTracker
{
  public class SettingsManager
  {
    //-------------------------------------------------------------------------
    
    public Dictionary<string, string> Setting { get; private set; }

    //-------------------------------------------------------------------------

    public SettingsManager()
    {
      GetSettingsFromDb();
    }

    //-------------------------------------------------------------------------
    
    void GetSettingsFromDb()
    {
      Setting = new Dictionary<string, string>();

      using( var connection = new SqlConnection( Database.DB_CONNECTION_STRING ) )
      {
        connection.Open();

        SqlDataReader reader =
          new SqlCommand(
            "SELECT [Key], Value FROM Setting",
            connection ).ExecuteReader();

        using( reader )
        {
          while( reader.Read() )
          {
            Setting.Add(
              reader.GetString( 0 ),
              reader.GetString( 1 ) );
          }
        }
      }

      // Apply some default settings if any are missing.
      if( !Setting.ContainsKey( "PageHeader" ) ) Setting.Add( "PageHeader", "### Missing Setting ###" );
      if( !Setting.ContainsKey( "DefaultContactType" ) ) Setting.Add( "DefaultContactType", "Phone" );
      if( !Setting.ContainsKey( "StatusRefreshRate" ) ) Setting.Add( "StatusRefreshRate", "60" );
      if( !Setting.ContainsKey( "DailyStatusResetTime" ) ) Setting.Add( "DailyStatusResetTime", "00:00" );
    }

    //-------------------------------------------------------------------------
  }
}