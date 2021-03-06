﻿using System;
using System.IO;
using System.Web;
using TeamTracker;

public partial class FileImportExport : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
    // Bounce back to main page if session has expired.
    if( Session[ SessionVars.SES_SETTINGS_LOGGED_IN ] == null ||
        (bool)Session[ SessionVars.SES_SETTINGS_LOGGED_IN ] == false )
    {
      Server.Transfer( "Default.aspx" );
    }
  }

  //---------------------------------------------------------------------------

  protected void PerformImport( object sender, EventArgs e )
  {
    string filename = HttpRuntime.CodegenDir + "/ToImport";

    FileUploader.SaveAs( filename );

    Result.Text =
      string.Format(
        "{0} ({1} bytes)<br /><br />",
        FileUploader.PostedFile.FileName,
        FileUploader.PostedFile.ContentLength );

    Result.Text = FileImport.Import( filename );

    // We call this in case the daily status reset time has changed.
    TaskScheduler.Start();
  }

  //---------------------------------------------------------------------------

  protected void PerformExport( object sender, EventArgs e )
  {
    string results = "";

    try
    {
      Result.Text = "";

      string filename = HttpRuntime.CodegenDir + "/TeamTrackerExport.txt";

      results = FileExport.Export( filename );
    
      if( File.Exists( filename ) )
      {
        FileInfo info = new FileInfo( filename );

        Response.Clear();
        Response.ClearHeaders();
        Response.ClearContent();
        Response.AddHeader( "Content-Disposition", "attachment; filename=" + info.Name );
        Response.AddHeader( "Content-Length", info.Length.ToString() );
        Response.ContentType = "text/plain";
        Response.Flush();
        Response.TransmitFile( info.FullName );
        Response.Flush();
        Response.SuppressContent = true;

        try
        {
          File.Delete( filename );
        }
        catch( Exception )
        {
          // Ignore.
        }
      }
    }
    catch( Exception ex )
    {
      Result.Text = ex.Message;
    }
    finally
    {
      Result.Text += "<br /><br />" + results;
    }
  }

  //---------------------------------------------------------------------------
}