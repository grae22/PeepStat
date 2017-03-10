<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta http-equiv="Refresh" content="60" />
    <title>TeamTracker</title>
	<link rel="icon" href="team.png">
  </head>
  <body>
  <img src="Logo.png" style="width:24px;height:24px;">
  <font face="Arial" size="5" color="black">
  <b>Support Team Availability Matrix</b>
  </font>
    <form id="Body" runat="server">
      <asp:table id="StatusTable" runat="server" CellPadding="5" />
    </form>
	v1.0
  </body>
</html>
