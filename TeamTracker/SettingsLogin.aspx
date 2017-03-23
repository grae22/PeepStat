<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettingsLogin.aspx.cs" Inherits="SettingsLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>
    <link href="TeamTracker.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="resources/team.png" />
  </head>
  <body>
    <p><a href="Default.aspx">Back</a></p>
    <form id="Login" runat="server">
      <div>
        <input runat="server" id="password" type="password" autofocus="autofocus" />
        <asp:Button runat="server" text="Login" onclick="OnLoginClick" />
      </div>
    </form>
  </body>
</html>
