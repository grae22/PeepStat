using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  const string IMAGE_PATH = "Resources/";
  const string IMAGE_EXT = ".png";
  const string IMAGE_PATH_SETTINGS = IMAGE_PATH + "settings.png";
  const string IMAGE_PATH_NO = IMAGE_PATH + "no.png";
  const string IMAGE_PATH_YES = IMAGE_PATH + "yes.png";
  const string IMAGE_PATH_WAND = IMAGE_PATH + "wand.png";

  Dictionary<string, Status> StatusTypes;
  int EditPersonId = -1;

  //---------------------------------------------------------------------------
 
  protected void Page_Load( object sender, EventArgs e )
  {
    Dictionary<string, Person> people = new Dictionary<string, Person>();

    int.TryParse( Request.QueryString[ "EditPersonId" ], out EditPersonId );

    GetPageHeaderFromDb();
    GetStatusTypesFromDb();
    GetPeopleFromDb( out people );

    BuildUiTable( StatusTable,
                  people,
                  StatusTypes );
  }

  //---------------------------------------------------------------------------

  void GetPageHeaderFromDb()
  {
    using( var connection = new SqlConnection( Database.DB_CONNECTION_STRING ) )
    {
      connection.Open();

      SqlDataReader reader =
        new SqlCommand(
          "SELECT Value FROM Setting WHERE [Key]='PageHeader'",
          connection ).ExecuteReader();

      using( reader )
      {
        reader.Read();

        if( reader.IsDBNull( 0 ) == false )
        {
          PageHeader.Text = reader.GetString( 0 );
        }
      }
    }
  }

  //---------------------------------------------------------------------------

  void GetStatusTypesFromDb()
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

  void GetPeopleFromDb( out Dictionary<string, Person> people )
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
          string contact = null;
          string statusType = null;
          int personId = -1;
          int statusTypeId = -1;

          if( reader.IsDBNull( 0 ) == false ) name = reader.GetString( 0 );
          if( reader.IsDBNull( 1 ) == false ) statusType = reader.GetString( 1 );
          if( reader.IsDBNull( 2 ) == false ) personId = reader.GetInt32( 2 );
          if( reader.IsDBNull( 3 ) == false ) statusTypeId = reader.GetInt32( 3 );
          if( reader.IsDBNull( 4 ) == false ) contact = reader.GetString( 4 );

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
            person.Contact = ( contact == null ? "" : contact );
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
  
  Table BuildUiTable( Table table,
                      Dictionary<string, Person> people,
                      Dictionary<string, Status> statusTypes )
  {
    table.Rows.Clear();

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

    // Header cell for 'Team member'.
    header.Cells.Add( new TableCell() );
    header.Cells[ 0 ].Text = "Team member";
    header.Cells[ 0 ].Font.Bold = true;

    // Header cell for 'Contact'.
    header.Cells.Add( new TableCell() );
    header.Cells[ 1 ].Text = "Contact";
    header.Cells[ 1 ].Font.Bold = true;

    // Add each status type to the header.
    Dictionary<Status, int> statusToColumnIndex = new Dictionary<Status, int>();

    foreach( Status status in sortedStatusTypes )
    {
      statusToColumnIndex.Add(
        status,
        AddCellToHeaderRow( header, status.Name ) );
    }

    // Add settings link.
    var settingsImage = new Image();
    settingsImage.ImageUrl = IMAGE_PATH_SETTINGS;
    settingsImage.Attributes.Add( "onclick", "window.location='SettingsLogin.aspx'" );
    settingsImage.Style.Add( "cursor", "pointer" );

    header.Cells.Add( new TableCell() );
    TableCell settingsCell = header.Cells[ header.Cells.Count - 1 ];
    settingsCell.Controls.Add( settingsImage );
    
    // Add each person and their statuses as a row.
    foreach( Person person in people.Values )
    {
      bool canEditThisPerson = ( person.Id == EditPersonId );

      var row = new TableRow();
      
      AddPersonToRow(
        person,
        row,
        0,
        HorizontalAlign.Left );

      AddContactLinkCellToRow(
        person,
        person.Contact,
        row,
        1,
        HorizontalAlign.Left );

      foreach( Status status in statusTypes.Values )
      {
        if( canEditThisPerson )
        {
          AddStatusToRow(
            person.Id,
            status.Id,
            person.Status.Contains( status ),
            row,
            statusToColumnIndex[ status ] );
        }
        else
        {
          AddImageToRow(
            person.Status.Contains( status ) ? IMAGE_PATH_YES : IMAGE_PATH_NO,
            row,
            statusToColumnIndex[ status ] );
        }
      }

      if( canEditThisPerson )
      {
        AddSelectAllOrNoneToRow( person.Id, row, row.Cells.Count );
      }

      table.Rows.Add( row );
    }

    return table;
  }

  //---------------------------------------------------------------------------

  int AddCellToHeaderRow( TableRow row, string text )
  {
    var newCell = new TableCell();

    string imageFilename = IMAGE_PATH + text + IMAGE_EXT;

    if( File.Exists( Server.MapPath( imageFilename ) ) )
    {
      var image = new Image();
      image.ImageUrl = imageFilename;
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

  void AddPersonToRow( Person person,
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

    var label = new Label();
    label.Text = person.Name;
    label.Attributes.Add(
      "onclick",
      string.Format( "EditUserWithId( {0} );", person.Id ) );

    row.Cells[ column ].Controls.Add( label );
  }

  //---------------------------------------------------------------------------

  void AddContactLinkCellToRow( Person person,
                                string text,
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

    List<string> contacts = new List<string>();

    using( SqlConnection connection = new SqlConnection( Database.DB_CONNECTION_STRING ) )
    {
      connection.Open();

      SqlDataReader reader =
        new SqlCommand(
          string.Format(
            "SELECT contactName, contactAddress, hyperlinkPrefix " +
              "FROM PeopleContactView " +
              "WHERE peopleId={0}",
            person.Id ),
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          contacts.Add( reader.GetString( 0 ) );
          contacts.Add( reader.GetString( 1 ) );
          contacts.Add( reader.GetString( 2 ) );
        }
      }
    }

    var contactsImage = new Image();
    contactsImage.ID = "contact_" + text;
    contactsImage.ImageUrl = IMAGE_PATH + "team.png";
    contactsImage.Width = 16;
    contactsImage.Height = 16;
    contactsImage.AlternateText = "...";
    contactsImage.ImageAlign = ImageAlign.AbsMiddle;
    contactsImage.Style.Add( "padding-left", "2px" );
    contactsImage.Attributes.Add(
      "onclick",
      string.Format(
        "ShowContactInfo( '{0}', '{1}' )",
        contactsImage.ID,
        string.Join( ";", contacts ) ) );

    row.Cells[ column ].Controls.Add( contactsImage );
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
    button.ImageUrl = statusActive ? IMAGE_PATH_YES : IMAGE_PATH_NO;
    button.Attributes.Add( "status", statusActive ? "active" : "" );
    button.Style.Add( "cursor", "pointer" );
    button.Click += HandleStatusClick;

    cell.Controls.Add( button );
  }

  //---------------------------------------------------------------------------
  
  void AddImageToRow( string imagePath,
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

    var image = new Image();
    image.ImageUrl = imagePath;

    cell.Controls.Add( image );
  }

  //---------------------------------------------------------------------------
  void AddSelectAllOrNoneToRow( int peopleId,
                          TableRow row,
                          int column )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      row.Cells.Add( cell );

      var button = new ImageButton();
      button.ID = "allOrNone_" + peopleId.ToString();
      button.ImageUrl = IMAGE_PATH_WAND;
      button.Click += OnSelectAllOrNoneClick;

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

    if( button.Attributes[ "status" ] == "active" )
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

  void OnSelectAllOrNoneClick( object sender, EventArgs args )
  {
    if( sender is ImageButton == false )
    {
      return;
    }

    ImageButton button = (ImageButton)sender;

    // Extract the person id & status-type id.
    int personId = int.Parse( button.ID.Replace( "allOrNone_", "" ) );

    // First clear the person's active statuses.
    string command =
      string.Format(
        "DELETE FROM PeopleStatus WHERE peopleId={0}",
        personId );

    int rowsAffected = Database.ExecSql( command );

    // If the person had at least one active status then we make all their statuses active,
    // but if all their status were already active then want them all to be inactive now.
    if( rowsAffected < StatusTypes.Count )
    {
      foreach( Status status in StatusTypes.Values )
      {
        command =
          string.Format(
            "INSERT INTO PeopleStatus ( peopleId, statusTypeId ) VALUES( {0}, {1} )",
            personId,
            status.Id );

        Database.ExecSql( command );
      }
    }

    // Refresh the page.
    Response.Redirect( Request.RawUrl );
  }

  //---------------------------------------------------------------------------  

  protected void SetEditPersonId( int id )
  {
    EditPersonId = id;
    Page_Load( null, null );
  }

  //---------------------------------------------------------------------------
}