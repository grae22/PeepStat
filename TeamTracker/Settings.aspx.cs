using System;
using System.Web.UI.WebControls;
using TeamTracker;

public partial class Settings : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
    // Bounce back to main page if session has expired.
    if( Session[ SettingsLogin.SES_SETTINGS_LOGGED_IN ] == null ||
        (bool)Session[ SettingsLogin.SES_SETTINGS_LOGGED_IN ] == false )
    {
      Server.Transfer( "Default.aspx" );
    }

    dataSource.ConnectionString = Database.DB_CONNECTION_STRING;

    settingsView.PreRender += Page_LoadComplete;
  }

  //---------------------------------------------------------------------------

  protected void Page_LoadComplete( object sender, EventArgs e )
  {
    foreach( GridViewRow row in settingsView.Rows )
    {
      // Key contains 'password'?
      if( row.Cells[ 1 ].Text.ToLower().Contains( "password" ) )
      {
        // If the 'value' cell doesn't have any controls (i.e. a textbox during edit mode).
        if( row.Cells[ 2 ].Controls.Count == 0 )
        {
          row.Cells[ 2 ].Text = "*****";
        }
      }
    }
  }

  //---------------------------------------------------------------------------
}