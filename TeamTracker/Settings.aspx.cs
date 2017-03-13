using System;

public partial class Settings : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
    // Don't allow people to skip the login page.
    if( Session[ SettingsLogin.SES_SETTINGS_LOGGED_IN ] == null ||
        (bool)Session[ SettingsLogin.SES_SETTINGS_LOGGED_IN ] == false )
    {
      Response.Redirect( "Default.aspx" );
    }

    dataSource.ConnectionString = Database.DB_CONNECTION_STRING;
    NewSetting.Click += OnNewClick;
  }

  //---------------------------------------------------------------------------

  void OnNewClick( object sender, EventArgs e )
  {
    Database.ExecSql( "DELETE FROM Setting WHERE [Key]='New Key'" );
    Database.ExecSql( "INSERT INTO Setting VALUES ( 'New Key', 'New Value' )" );
    
    settingsView.DataBind();
  }

  //---------------------------------------------------------------------------
}