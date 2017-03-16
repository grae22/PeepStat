using System;
using System.Data.SqlClient;
using TeamTracker;

public partial class SettingsLogin : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  public static readonly string SES_SETTINGS_LOGGED_IN = "SesSettingsLoggedIn";

  protected string DbConnectionString = Database.DB_CONNECTION_STRING;

  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
    if( Database.ExecSql( "SELECT id FROM Setting WHERE [Key]='SettingsPassword'" ) == 0 )
    {
      Database.ExecSql( "INSERT INTO Setting ( [Key], Value ) VALUES ( 'SettingsPassword', 'admin' )" );
    }
  }

  //---------------------------------------------------------------------------

  protected void OnLoginClick( object sender, EventArgs e )
  {
    if( Database.ExecSql( "SELECT id FROM Setting WHERE [Key]='SettingsPassword'" ) == 0 )
    {
      Database.ExecSql( "INSERT INTO Setting ( [Key], Value ) VALUES ( 'SettingsPassword', 'admin' )" );
    }

    string password = Request.Form[ "password" ];

    if( ValidatePassword( password ) )
    {
      Session[ SES_SETTINGS_LOGGED_IN ] = true;
      Response.Redirect( "Settings.aspx" );
    }
  }

  //---------------------------------------------------------------------------

  bool ValidatePassword( string password )
  {
    using( SqlConnection connection = new SqlConnection( Database.DB_CONNECTION_STRING ) )
    {
      connection.Open();

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