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
          UpdateCommand="UPDATE Setting SET [key]=@key, value=@value WHERE id=@id"
          DeleteCommand="DELETE FROM Setting WHERE id=@id"
          DataSourceMode="DataSet">
        </asp:SqlDataSource>
        <asp:GridView
          ID="settingsView"
          Runat="server"
          DataSourceID="dataSource"
          AutoGenerateColumns="false"
          DataKeyNames="id"
          ShowHeader="true"
          ShowFooter="true">
          <AlternatingRowStyle BackColor="LightBlue" />
          <Columns>
            <asp:CommandField
              ShowEditButton="true"
              ShowDeleteButton="true" />
            <asp:BoundField
              HeaderText="Key"
              ReadOnly="false"
              DataField="key" />
            <asp:BoundField
              HeaderText="Value"
              ReadOnly="false"
              DataField="value" />
          </Columns>
        </asp:GridView>
        <asp:Button ID="NewSetting" runat="server" Text="New" />
      </div>
    </form>
  </body>
</html>
