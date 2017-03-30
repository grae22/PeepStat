<%@ Application Language="C#" %>
<%@ Import Namespace="TeamTracker" %>

<script RunAt="server">

  void Application_Start( object sender, EventArgs e )
  {
    TaskScheduler.Start();
  }

  void Application_End( object sender, EventArgs e )
  {
  }

  void Application_Error( object sender, EventArgs e )
  {
    Exception ex = Server.GetLastError();
    Log.AddEntry( ex );
    Server.Transfer( "Error.aspx" );
  }

  void Session_Start( object sender, EventArgs e )
  {
  }

  void Session_End( object sender, EventArgs e )
  {
  }

</script>
