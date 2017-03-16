﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Settings</title>
    <link href="TeamTracker.css" rel="stylesheet" type="text/css" />
  </head>
  <body>
    <p><a href="Default.aspx">Back</a></p>
    <form id="SettingsForm" runat="server">
      <div>
        <asp:SqlDataSource
          ID="dataSource"
          Runat="server"
          SelectCommand="SELECT * FROM Setting"
          UpdateCommand="UPDATE Setting SET value=@value WHERE id=@id"
          DataSourceMode="DataSet">
        </asp:SqlDataSource>
        <asp:GridView
          ID="settingsView"
          Runat="server"
          DataSourceID="dataSource"
          AutoGenerateColumns="false"
          DataKeyNames="id"
          ShowHeader="true"
          ShowFooter="false">
          <AlternatingRowStyle BackColor="LightBlue" />
          <Columns>
            <asp:CommandField
              ShowEditButton="true"
              ShowDeleteButton="false" />
            <asp:BoundField
              HeaderText="Key"
              ReadOnly="true"
              DataField="key" />
            <asp:BoundField
              HeaderText="Value"
              ReadOnly="false"
              DataField="value" />
          </Columns>
        </asp:GridView>
      </div>
    </form>
    <p><a href="FileImportExport.aspx">Import/Export</a></p>
  </body>
</html>
