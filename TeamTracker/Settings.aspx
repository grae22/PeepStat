<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Settings</title>
  </head>
  <body>
    <a href="Default.aspx">Back</a>
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
  </body>
</html>
