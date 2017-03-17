using System.Data.SqlClient;

namespace TeamTracker
{
  public class DbTransaction
  {
    //-------------------------------------------------------------------------

    public SqlConnection Connection { get; private set; }

    string Name;

    //-------------------------------------------------------------------------

    public DbTransaction( string name )
    {
      Connection = Database.OpenConnection();
      Name = name;

      ExecNonQuery( "BEGIN TRANSACTION " + Name );
    }

    //-------------------------------------------------------------------------

    public void Commit()
    {
      ExecNonQuery( "COMMIT TRANSACTION " + Name );
    }

    //-------------------------------------------------------------------------
    
    public void Rollback()
    {
      ExecNonQuery( "ROLLBACK TRANSACTION " + Name );
    }

    //-------------------------------------------------------------------------

    public int ExecNonQuery( string command )
    {
      return
        new SqlCommand(
          command,
          Connection ).ExecuteNonQuery();
    }

    //-------------------------------------------------------------------------
  }
}