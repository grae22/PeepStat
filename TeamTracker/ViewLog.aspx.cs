using System;
using System.IO;
using System.Web;
using TeamTracker;

public partial class ViewLog : System.Web.UI.Page
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

    // Display log's content.
    string filePath = HttpContext.Current.Server.MapPath( Log.FILENAME );
    string buffer = "";

    if( File.Exists( filePath ) )
    {
      using( var reader = new StreamReader( filePath ) )
      {
        buffer = reader.ReadToEnd();
      }
    }

    LogContent.InnerHtml = buffer.Replace( Environment.NewLine, "<br />" );
  }

  //---------------------------------------------------------------------------
}