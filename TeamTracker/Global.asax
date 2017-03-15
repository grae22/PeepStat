<%@ Application Language="C#" %>

<script RunAt="server">

  void Application_Start( object sender, EventArgs e )
  {
    TeamTracker.TaskScheduler.Start();
  }

  void Application_End( object sender, EventArgs e )
  {
  }

  void Application_Error( object sender, EventArgs e )
  {
  }

  void Session_Start( object sender, EventArgs e )
  {
  }

  void Session_End( object sender, EventArgs e )
  {
  }

</script>
