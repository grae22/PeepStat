﻿using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using TeamTracker;

public partial class _Default : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected string StatusRefreshRate { get; private set; }

  const string IMAGE_PATH = "Resources/";
  const string IMAGE_EXT = ".png";
  const string IMAGE_PATH_NO = IMAGE_PATH + "no.png";
  const string IMAGE_PATH_YES = IMAGE_PATH + "yes.png";
  const string IMAGE_PATH_WAND = IMAGE_PATH + "wand.png";
  const string IMAGE_PATH_DROPDOWN = IMAGE_PATH + "dropdown.png";
  const string IMAGE_PATH_PERSON = IMAGE_PATH + "person.png";
  const string IMAGE_PATH_CONTACT = IMAGE_PATH + "contact.png";

  SettingsManager Settings = new SettingsManager();
  Dictionary<int, Status> StatusTypes;
  Dictionary<int, Person> People;
  int EditPersonId = -1;

  //---------------------------------------------------------------------------
 
  protected void Page_Load( object sender, EventArgs e )
  {
    if( Request.QueryString[ "EditPersonId" ] != null )
    {
      int.TryParse( Request.QueryString[ "EditPersonId" ], out EditPersonId );
    }

    GetSettingsFromDb();

    using( SqlConnection connection = Database.OpenConnection() )
    {
      StatusTypes = Status.Load( connection );
      People = Person.Load( connection, StatusTypes );
    }

    PageHeader.Text = Settings.Setting[ "PageHeader" ];

    BuildUiTable( StatusTable, People, StatusTypes );
  }

  //---------------------------------------------------------------------------

  void GetSettingsFromDb()
  {
    StatusRefreshRate = Settings.Setting[ "StatusRefreshRate" ];
  }

  //---------------------------------------------------------------------------
  
  Table BuildUiTable( Table table,
                      Dictionary<int, Person> people,
                      Dictionary<int, Status> statusTypes )
  {
    table.Rows.Clear();

    // Build a dictonary of status-types sorted by sort-order.
    List<Status> sortedStatusTypes = new List<Status>( statusTypes.Values );
    sortedStatusTypes.Sort();

    // Compile sorted list of people.
    List<Person> sortedPeople = new List<Person>( people.Values );
    sortedPeople.Sort();    

    // Table general.
    table.BorderWidth = 1;

    // Table header.
    var header = new TableRow();
    table.Rows.Add( header );

    // Header cell for 'Team member'.
    var personImage = new Image();
    personImage.ImageUrl = IMAGE_PATH_PERSON;

    header.Cells.Add( new TableCell() );
    header.Cells[ 0 ].Controls.Add( personImage );

    // Header cell for 'Contact'.
    var contactImage = new Image();
    contactImage.ImageUrl = IMAGE_PATH_CONTACT;

    header.Cells.Add( new TableCell() );
    header.Cells[ 1 ].Controls.Add( contactImage );

    // Add each status type to the header.
    Dictionary<Status, int> statusToColumnIndex = new Dictionary<Status, int>();

    foreach( Status status in sortedStatusTypes )
    {
      statusToColumnIndex.Add(
        status,
        AddCellToHeaderRow( header, status.Name ) );
    }

    // Add each person and their statuses as a row.
    foreach( Person person in sortedPeople )
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
        row,
        1,
        HorizontalAlign.Left );

      foreach( Status status in statusTypes.Values )
      {
        string tooltip = "";

        if( person.Contacts.ContainsKey( status ) )
        {
          tooltip = person.Contacts[ status ].Address;
        }

        if( canEditThisPerson )
        {
          AddStatusToRow(
            person,
            status,
            tooltip,
            person.Statuses.Contains( status ),
            row,
            statusToColumnIndex[ status ] );
        }
        else
        {
          AddImageToRow(
            person.Statuses.Contains( status ) ? IMAGE_PATH_YES : IMAGE_PATH_NO,
            tooltip,
            row,
            statusToColumnIndex[ status ] );
        }
      }

      AddSelectAllOrNoneToRow(
        person.Id, 
        row,
        row.Cells.Count,
        !canEditThisPerson );

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

    // Displayed link.
    Status sipStatus = Status.GetByName( "phone", StatusTypes.Values );

    if( sipStatus != null &&
        person.Contacts.ContainsKey( sipStatus ) )
    {
      var link = new HyperLink();
      link.NavigateUrl =
        string.Format(
          "<a href='{0}:{1}'>{1}</a>",
          sipStatus.HyperlinkPrefix,
          person.Contacts[ sipStatus ].Address );
      link.Text = link.NavigateUrl;

      row.Cells[ column ].Controls.Add( link );
    }

    // Links in dropdown.
    List<string> dropdownContacts = new List<string>();

    foreach( Contact contact in person.Contacts.Values )
    {
      if( contact.Type == sipStatus ||
          contact.Address.Length == 0 )
      {
        continue;
      }

      dropdownContacts.Add( contact.Type.Name );
      dropdownContacts.Add( contact.Address );
      dropdownContacts.Add( contact.Type.HyperlinkPrefix );
    }

    // Contacts dropdown.
    if( dropdownContacts.Count > 0 )
    {
      var contactsImage = new Image();
      contactsImage.ID = "contact_" + person.Id;
      contactsImage.ImageUrl = IMAGE_PATH_DROPDOWN;
      contactsImage.Width = 16;
      contactsImage.Height = 16;
      contactsImage.ImageAlign = ImageAlign.AbsMiddle;
      contactsImage.Style.Add( "padding-left", "2px" );
      contactsImage.Style.Add( "cursor", "pointer" );
      contactsImage.Attributes.Add(
        "onclick",
        string.Format( 
          "ShowContactInfo( '{0}', {1}, '{2}', '{3}' )", 
          contactsImage.ID, 
          person.Id, 
          contactsImage.ID, 
          string.Join( ";", dropdownContacts ) ) ); 
 
      row.Cells[ column ].Controls.Add( contactsImage );
    }
  }
  
  //---------------------------------------------------------------------------

  void AddStatusToRow( Person person,
                       Status status,
                       string tooltip,
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
    button.ID = string.Format( "{0}~{1}", person.Id, status.Id );
    button.ImageUrl = statusActive ? IMAGE_PATH_YES : IMAGE_PATH_NO;
    button.ToolTip = tooltip;
    button.Attributes.Add( "status", statusActive ? "active" : "" );
    button.Style.Add( "cursor", "pointer" );
    button.Click += HandleStatusClick;

    cell.Controls.Add( button );
  }

  //---------------------------------------------------------------------------
  
  void AddImageToRow( string imagePath,
                      string tooltip,
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
    image.ToolTip = tooltip;

    cell.Controls.Add( image );
  }

  //---------------------------------------------------------------------------

  void AddSelectAllOrNoneToRow( int peopleId,
                                TableRow row,
                                int column,
                                bool usePlaceholder )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      row.Cells.Add( cell );

      if( usePlaceholder == false )
      {
        var button = new ImageButton();
        button.ID = "allOrNone_" + peopleId.ToString();
        button.ImageUrl = IMAGE_PATH_WAND;
        button.Click += OnSelectAllOrNoneClick;

        cell.Controls.Add( button );
      }
      else
      {
        var div = new HtmlGenericControl( "div" );
        div.Attributes.Add( "class", "selectAllOrNonePlaceholder" );

        cell.Controls.Add( div );
      }
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
    Server.Transfer( Request.RawUrl );
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
    Server.Transfer( Request.RawUrl );
  }

  //---------------------------------------------------------------------------

  protected void SetEditPersonId( int id )
  {
    EditPersonId = id;
    Page_Load( null, null );
  }

  //---------------------------------------------------------------------------
}
