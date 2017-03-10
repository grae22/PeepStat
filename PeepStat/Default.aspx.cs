using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  Dictionary<string, Status> StatusTypes;

  //---------------------------------------------------------------------------
 
  protected void Page_Load( object sender, EventArgs e )
  {
    Dictionary<string, Person> people = new Dictionary<string, Person>();

    PopulateStatusTypes();
    PopulatePeople( out people );

    BuildTable( StatusTable,
                people,
                StatusTypes );
  }

  //---------------------------------------------------------------------------

  void PopulateStatusTypes()
  {
    StatusTypes = new Dictionary<string, Status>();

    using( var connection = new SqlConnection( Database.DB_CONNECTION_STRING ) )
    {
      connection.Open();

      var reader =
        new SqlCommand(
          "SELECT * FROM StatusTypes",
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          Status status = new Status
          {
            Id = reader.GetInt32( 0 ),
            Name = reader.GetString( 1 ),
            SortOrder = reader.GetInt32( 2 )
          };

          StatusTypes.Add( status.Name, status );
        }
      }
    }
  }

  //---------------------------------------------------------------------------

  void PopulatePeople( out Dictionary<string, Person> people )
  {
    // Load people & status types from the db.
    people = new Dictionary<string, Person>();

    using( var connection = new SqlConnection( Database.DB_CONNECTION_STRING ) )
    {
      connection.Open();

      var reader =
        new SqlCommand(
          "SELECT * FROM PeopleStatusView ORDER BY PersonName",
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          // Read the values from the view.
          string name = null;
          string extension = null;
          string statusType = null;
          int personId = -1;
          int statusTypeId = -1;

          if( reader.IsDBNull( 0 ) == false )
          {
            name = reader.GetString( 0 );
          }

          if( reader.IsDBNull( 1 ) == false )
          {
            statusType = reader.GetString( 1 );
          }

          if( reader.IsDBNull( 2 ) == false )
          {
            personId = reader.GetInt32( 2 );
          }

          if( reader.IsDBNull( 3 ) == false )
          {
            statusTypeId = reader.GetInt32( 3 );
          }

          if( reader.IsDBNull( 4 ) == false )
          {
            extension = reader.GetString( 4 );
          }

          // Add the person to our collection.
          if( name != null &&
              people.ContainsKey( name ) == false )
          {
            people.Add( name, new Person() );
          }

          // Retrieve the person from our collection.
          Person person = null;

          if( name != null )
          {
            person = people[ name ];

            person.Id = personId;
            person.Name = name;
            person.Extension = ( extension == null ? "" : extension );
          }

          // Add the status to both the current person (if one) and our collection
          // of statue types.
          if( statusType != null )
          {
            if( person != null )
            {
              person.Status.Add( StatusTypes[ statusType ] );
            }
          }
        }
      }
    }
  }

  //---------------------------------------------------------------------------
  
  Table BuildTable( Table table,
                    Dictionary<string, Person> people,
                    Dictionary<string, Status> statusTypes )
  {
    // Build a dictonary of status-types sorted by sort-order.
    List<Status> sortedStatusTypes = new List<Status>();

    foreach( Status status in statusTypes.Values )
    {
      sortedStatusTypes.Add( status );
    }

    sortedStatusTypes.Sort();

    // Table general.
    table.BorderWidth = 1;

    // Table header.
    var header = new TableRow();
    table.Rows.Add( header );

    // Header cell for 'Name'.
    header.Cells.Add( new TableCell() );
    header.Cells[ 0 ].Text = "Name";
    header.Cells[ 0 ].Font.Bold = true;

    // Header cell for 'Extension'.
    header.Cells.Add( new TableCell() );
    header.Cells[ 1 ].Text = "Ext.";
    header.Cells[ 1 ].Font.Bold = true;

    // Add each status type to the header.
    Dictionary<Status, int> statusToColumnIndex = new Dictionary<Status, int>();

    foreach( Status status in sortedStatusTypes )
    {
      statusToColumnIndex.Add(
        status,
        AddCellToHeaderRow( header, status.Name ) );
    }
    
    // Add each person and their statuses as a row.
    foreach( Person person in people.Values )
    {
      var row = new TableRow();
      
      AddTextCellToRow(
        person.Name,
        row,
        0,
        HorizontalAlign.Left );

      AddSipLinkCellToRow(
        person.Extension,
        row,
        1,
        HorizontalAlign.Left );

      foreach( Status status in statusTypes.Values )
      {
        AddStatusToRow(
          person.Id,
          status.Id,
          person.Status.Contains( status ),
          row,
          statusToColumnIndex[ status ] );
      }

      AddSelectAllToRow( person.Id, row, row.Cells.Count );
      AddSelectNoneToRow( person.Id, row, row.Cells.Count );

      table.Rows.Add( row );
    }

    return table;
  }

  //---------------------------------------------------------------------------

  int AddCellToHeaderRow( TableRow row, string text )
  {
    var newCell = new TableCell();

    if( File.Exists( Server.MapPath( text + ".png" ) ) )
    {
      var image = new Image();
      image.ImageUrl = text + ".png";
      newCell.Controls.Add( image );
    }
    else
    {
      newCell.Text = text;
      newCell.Font.Bold = true;
    }

    row.Cells.Add( newCell );

    return row.Cells.Count - 1;
  }

  //---------------------------------------------------------------------------

  void AddTextCellToRow( string text,
                         TableRow row,
                         int column,
                         HorizontalAlign align = HorizontalAlign.Center )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      cell.HorizontalAlign = align;
      row.Cells.Add( cell );
    }

    row.Cells[ column ].Text = text;
  }

  //---------------------------------------------------------------------------

  void AddSipLinkCellToRow( string text,
                            TableRow row,
                            int column,
                            HorizontalAlign align = HorizontalAlign.Center )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      cell.HorizontalAlign = align;

      row.Cells.Add( cell );
    }

    var link = new HyperLink();
    link.NavigateUrl = string.Format( "<a href='sip:{0}'>{0}</a>", text );
    link.Text = link.NavigateUrl;
    
    row.Cells[ column ].Controls.Add( link );
  }
  
  //---------------------------------------------------------------------------

  void AddStatusToRow( int peopleId,
                       int statusId,
                       bool statusActive,
                       TableRow row,
                       int column,
                       HorizontalAlign align = HorizontalAlign.Center )
  {
    while( column > row.Cells.Count - 1 )
    {
      row.Cells.Add( new TableCell() );
    }

    TableCell cell = row.Cells[ column ];
    cell.HorizontalAlign = align;

    var button = new ImageButton();
    button.ID = string.Format( "{0}~{1}", peopleId, statusId );
    button.ImageUrl =
      statusActive ?
      "yes.png" :
      "no.png";
    button.ToolTip = statusActive ? "active" : "";
    button.Click += HandleStatusClick;

    cell.Controls.Add( button );
  }

  //---------------------------------------------------------------------------
  
  void AddSelectAllToRow( int peopleId,
                          TableRow row,
                          int column )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      row.Cells.Add( cell );

      var button = new Button();
      button.ID = "all_" + peopleId.ToString();
      button.Text = "All";
      button.Click += OnSelectAllClick;

      cell.Controls.Add( button );
    }
  }

  //---------------------------------------------------------------------------
  
  void AddSelectNoneToRow( int peopleId,
                           TableRow row,
                           int column )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      row.Cells.Add( cell );

      var button = new Button();
      button.ID = "none_" + peopleId.ToString();
      button.Text = "None";
      button.Click += OnSelectNoneClick;

      cell.Controls.Add( button );
    }
  }

  //---------------------------------------------------------------------------

  void HandleStatusClick( object sender, EventArgs args )
  {
    if( sender is ImageButton == false )
    {
      return;
    }

    ImageButton button = (ImageButton)sender;

    // Extract the person id & status-type id.
    string[] buttonId = button.ID.Split( '~' );

    int personId = int.Parse( buttonId[ 0 ] );
    int statusTypeId = int.Parse( buttonId[ 1 ] );

    // If the status was active then remove it from the PeopleStatus table,
    // otherwise add it (to make the status active).
    string command;

    if( button.ToolTip == "active" )
    {
      command =
        string.Format(
          "DELETE FROM PeopleStatus WHERE peopleId={0} AND statusTypeId={1}",
          personId,
          statusTypeId );
    }
    else
    {
      command =
        string.Format(
          "INSERT INTO PeopleStatus ( peopleId, statusTypeId ) VALUES( {0}, {1} )",
          personId,
          statusTypeId );
    }

    Database.ExecSql( command );

    // Refresh the page.
    Response.Redirect( Request.RawUrl );
  }

  //---------------------------------------------------------------------------

  void OnSelectAllClick( object sender, EventArgs args )
  {
    if( sender is Button == false )
    {
      return;
    }

    Button button = (Button)sender;

    // Extract the person id & status-type id.
    int personId = int.Parse( button.ID.Replace( "all_", "" ) );

    // If the status was active then remove it from the PeopleStatus table,
    // otherwise add it (to make the status active).
    string command;

    foreach( Status status in StatusTypes.Values )
    {
      command =
        string.Format(
          "DELETE FROM PeopleStatus WHERE peopleId={0} AND statusTypeId={1}",
          personId,
          status.Id );

      Database.ExecSql( command );

      command =
        string.Format(
          "INSERT INTO PeopleStatus ( peopleId, statusTypeId ) VALUES( {0}, {1} )",
          personId,
          status.Id );

      Database.ExecSql( command );
    }

    // Refresh the page.
    Response.Redirect( Request.RawUrl );
  }

  //---------------------------------------------------------------------------

  void OnSelectNoneClick( object sender, EventArgs args )
  {
    if( sender is Button == false )
    {
      return;
    }

    Button button = (Button)sender;

    // Extract the person id & status-type id.
    int personId = int.Parse( button.ID.Replace( "none_", "" ) );

    // If the status was active then remove it from the PeopleStatus table,
    // otherwise add it (to make the status active).
    string command;

    foreach( Status status in StatusTypes.Values )
    {
      command =
        string.Format(
          "DELETE FROM PeopleStatus WHERE peopleId={0} AND statusTypeId={1}",
          personId,
          status.Id );

      Database.ExecSql( command );
    }

    // Refresh the page.
    Response.Redirect( Request.RawUrl );
  }

  //---------------------------------------------------------------------------
}