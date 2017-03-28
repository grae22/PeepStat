using System;
using System.Data.SqlClient;
using TeamTracker;

public partial class SettingsLogin : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected string DbConnectionString = Database.DB_CONNECTION_STRING;

  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
  }

  //---------------------------------------------------------------------------

  protected void OnLoginClick( object sender, EventArgs e )
  {
    string password = Request.Form[ "password" ];

    if( ValidatePassword( password ) )
    {
      Session[ SessionVars.SES_SETTINGS_LOGGED_IN ] = true;
      Server.Transfer( "Settings.aspx" );
    }
  }

  //---------------------------------------------------------------------------

  bool ValidatePassword( string password )
  {
    if( Database.ExecScalar( "SELECT id FROM Setting WHERE [Key]='SettingsPassword'" ) == null )
    {
      Database.ExecSql( "INSERT INTO Setting ( [Key], Value ) VALUES ( 'SettingsPassword', 'admin' )" );
    }

    using( SqlConnection connection = Database.OpenConnection() )
    {
      int rowCount =
        (int)
        new SqlCommand(
          string.Format(
            "SELECT COUNT(*) " +
              "FROM Setting " +
              "WHERE [Key]='SettingsPassword' AND Value='{0}'",
            password ),
          connection ).ExecuteScalar();

      return rowCount == 1;
    }
  }

  //---------------------------------------------------------------------------
}