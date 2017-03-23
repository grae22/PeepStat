<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileImportExport.aspx.cs" Inherits="FileImportExport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta http-equiv="Refresh" content="300;url=Settings.aspx" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>File Import/Export</title>
    <link href="TeamTracker.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="resources/team.png" />
  </head>
  <body>
    <p><a href="Settings.aspx">Back to Settings</a></p>
    <form id="ImportExportForm" runat="server">
      <div>
        <p>
          <span>
            Export:
            <asp:Button
              ID="Export"
              runat="server"
              Text="Export"
              OnClick="PerformExport" />
          </span>
        </p>
        <p>
          <span>
            Import:
            <asp:FileUpload ID="FileUploader" runat="server" />
            <asp:Button
              ID="Import"
              runat="server"
              Text="Upload & Import"
              OnClick="PerformImport" />
          </span>
        </p>
        <p>
          <asp:Label ID="Result" runat="server" />
        </p>
      </div>
    </form>
  </body>
</html>
