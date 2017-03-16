<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<script lang="text/javascript">
  function EditUserWithId( id )
  {
    window.location = "Default.aspx?EditPersonId=" + id;
  }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta http-equiv="Refresh" content="60;url=Default.aspx" />
    <title>TeamTracker</title>
    <link href="TeamTracker.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="resources/team.png" />
  </head>
  <body>
    <div style="display:block;">
      <img src="resources/Logo.png" style="width: 24px; height: 24px;" align="top"/>
      <asp:Label
        ID="PageHeader"
        runat="server"
        class="pageHeader" />
    </div>
    <div style="display:inline-block;">
      <form id="Body" runat="server">
        <asp:Table ID="StatusTable" runat="server" CellPadding="5" />
      </form>
      <div class="pageFooter">
        <table style="width:100%;">
          <tr>
            <td>TeamTracker v1.0 © GB & JM 2017</td>
            <td style="text-align:right;">
              <a href="SettingsLogin.aspx" style="color:#5caeb4;">Settings</a>
            </td>
          </tr>
        </table>
      </div>
    </div>
  </body>
</html>
