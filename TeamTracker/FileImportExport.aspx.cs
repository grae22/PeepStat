using System;
using System.Web;
using TeamTracker;

public partial class FileImportExport : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
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

    Result.Text += FileImport.Import( filename );
  }

  //---------------------------------------------------------------------------
}