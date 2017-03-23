﻿using System;
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
  }

  //---------------------------------------------------------------------------
}