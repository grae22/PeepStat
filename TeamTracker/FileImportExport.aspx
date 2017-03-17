<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileImportExport.aspx.cs" Inherits="FileImportExport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>File Import/Export</title>
    <link href="TeamTracker.css" rel="stylesheet" type="text/css" />
  </head>
  <body>
    <p><a href="Settings.aspx">Back to Settings</a></p>
    <form id="ImportExportForm" runat="server">
      <div>
        <asp:Button
          ID="Export"
          runat="server"
          Text="Export"
          OnClick="PerformExport" />
        <asp:FileUpload ID="FileUploader" runat="server" />
        <asp:Button
          ID="Import"
          runat="server"
          Text="Upload & Import"
          OnClick="PerformImport" />
        <br />
        <br />
        <asp:Label ID="Result" runat="server" />
      </div>
    </form>
  </body>
</html>
