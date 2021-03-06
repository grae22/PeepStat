﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewLog.aspx.cs" Inherits="ViewLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Refresh" content="300;url=ViewLog.aspx" />
    <title>View Log</title>
    <link href="TeamTracker.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="resources/team.png" />
  </head>
  <body>
    <form id="ViewLogForm" runat="server">
      <a href="Settings.aspx">Back</a>
      <asp:LinkButton
        ID="ClearLogLink"
        runat="server"
        OnClick="ClearLog"
        Text="Clear Log" />
    </form>
    <p><div id="LogContent" runat="server" /></p>
  </body>
</html>
