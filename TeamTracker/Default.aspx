<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<script lang="text/javascript">
  function EditUserWithId( id )
  {
    window.location = "Default.aspx?EditPersonId=" + id;
  }

  function ShowContactInfo( personId,
                            parentControlName,
                            contactsDelimStr )
  {
    var panel = document.getElementById( "ContactPanel" );
    var parent = document.getElementById( parentControlName );
    var parentRect = parent.getBoundingClientRect();

    if( panel.style.display == 'block' &&
        panel.attributes[ "personId" ] == personId )
    {
      panel.style.display = 'none';
      return;
    }

    panel.style.display = 'block';
    panel.style.left = parentRect.right + 'px';
    panel.style.top = parentRect.top + 'px';

    var contacts = contactsDelimStr.split( ';' );

    while( panel.childNodes.length > 0 )
    {
      if( panel.lastChild.className != "w3-closebtn" )
      {
        panel.removeChild( panel.lastChild );
      }
    }

    for( i = 0; i < contacts.length - 2; i += 3 )
    {
      var link = document.createElement( 'a' );
      link.text = contacts[ i ] + " (" + contacts[ i + 1 ] + ')';
      link.href = contacts[ i + 2 ] + ':' + contacts[ i + 1 ];
      link.style.color = 'white';

      panel.appendChild( link );
      panel.appendChild( document.createElement( "br" ) );
    }

    panel.attributes[ "personId" ] = personId;
  }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta http-equiv="Refresh" content="60;url=Default.aspx" />
    <title>TeamTracker</title>
    <link rel="icon" href="resources/team.png" />
  </head>
  <body>
    <img src="resources/Logo.png" style="width: 24px; height: 24px;" />
    <font face="Arial" size="5" color="black">
      <b><asp:Label ID="PageHeader" runat="server" /></b>
    </font>
    <form id="Body" runat="server">
      <asp:Table ID="StatusTable" runat="server" CellPadding="5" />
    </form>
    <div
      id="ContactPanel"
      class="w3-panel"
      style="
        border:black;
        border-style:solid;
        border-width:1px;
        background-color:#00aaff;
        position:absolute;
        opacity:0.9;
        visibility:visible;
        display:none;
        width:250px;
        height:75px">
    </div>
    <font face="Arial" size="1" color="black">
    TeamTracker v1.0 © GB & JM 2017
    </font>
  </body>
</html>
