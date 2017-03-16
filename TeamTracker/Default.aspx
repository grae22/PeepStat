<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<script lang="text/javascript">

  var previousDropdownIconId = null;

  function EditUserWithId( id )
  {
    window.location = "Default.aspx?EditPersonId=" + id;
  }

  function ShowContactInfo( iconId,
                            personId,
                            parentControlName,
                            contactsDelimStr )
  {
    var icon = document.getElementById( iconId );
    var panel = document.getElementById( "ContactPanel" );
    var parent = document.getElementById( parentControlName );
    //var parent = document.getElementById( iconId );
    var parentRect = parent.getBoundingClientRect();

    if( panel.style.display == 'block' &&
        panel.attributes[ "personId" ] == personId )
    {
      panel.style.display = 'none';
      icon.src = 'resources/dropdown.png';
      previousDropdownIconId = null;
      return;
    }

    panel.style.display = 'block';
    panel.style.left = parentRect.right + 'px';
    panel.style.top = parentRect.top + 'px';
    //panel.style.left = ( parentRect.right - panel.clientWidth - 2 ) + 'px';
    //panel.style.top = parentRect.bottom + 'px';

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
      link.text = contacts[ i ];// + " (" + contacts[ i + 1 ] + ')';
      link.href = contacts[ i + 2 ] + ':' + contacts[ i + 1 ];
      //link.style.color = "black";

      panel.appendChild( link );
      panel.appendChild( document.createElement( "br" ) );
    }

    panel.attributes[ "personId" ] = personId;

    icon.src = "resources/dropup.png";

    if( previousDropdownIconId != null )
    {
      document.getElementById( previousDropdownIconId ).src = "resources/dropdown.png";
    }

    previousDropdownIconId = iconId;
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
      <img src="resources/Logo.png" style="width: 24px; height: 24px;" />
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
    <div
      id="ContactPanel"
      class="w3-panel"
      style="
        border:black;
        border-style:solid;
        border-width:1px;
        border-color:#87d7ff;
        background-color:#ffffff;
        color:#fe5d00;
        font-family:Arial;
        position:absolute;
        opacity:0.8;
        visibility:visible;
        display:none;
        width:50px;
        height:40px;
        padding: 4px;">
    </div>
  </body>
</html>
