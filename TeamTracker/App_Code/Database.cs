using System.Data.SqlClient;

namespace TeamTracker
{
  public class Database
  {
    //-------------------------------------------------------------------------

    public const string DB_SERVER_NAME = "localhost";
    public const string DB_NAME = "TeamTracker";
    public const string DB_USERNAME = "TeamTrackerUser";
    public const string DB_PASSWORD = "TeamTrackerUser";

    public static readonly string DB_CONNECTION_STRING =
      string.Format(
        "Server={0};Database={1};User Id={2};Password={3};",
        DB_SERVER_NAME,
        DB_NAME,
        DB_USERNAME,
        DB_PASSWORD );

    //-------------------------------------------------------------------------

    public static int ExecSql( string command )
    {
      using( SqlConnection connection = OpenConnection() )
      {
        return new SqlCommand( command, connection ).ExecuteNonQuery();
      }
    }

    //-------------------------------------------------------------------------

    public static object ExecScalar( string command )
    {
      using( SqlConnection connection = OpenConnection() )
      {
        return new SqlCommand( command, connection ).ExecuteScalar();
      }
    }

    //-------------------------------------------------------------------------

    public static SqlConnection OpenConnection()
    {
      var connection = new SqlConnection( DB_CONNECTION_STRING );
      connection.Open();
      return connection;
    }

    //-------------------------------------------------------------------------
  }
}