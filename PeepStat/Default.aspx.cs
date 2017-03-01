using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
  //---------------------------------------------------------------------------

  protected void Page_Load( object sender, EventArgs e )
  {
    Dictionary<string, Person> people = new Dictionary<string, Person>();
    SortedDictionary<string, int> statusTypes = new SortedDictionary<string, int>();

    SqlConnection connection =
      new SqlConnection(
        @"Server=GRAEMEPC\SQLEXPRESS;Database=PeepStat;Trusted_Connection=True;" );

    SqlCommand command =
      new SqlCommand(
        "SELECT * FROM PeopleStatusView ORDER BY PersonName",
        connection );

    connection.Open();

    SqlDataReader reader = command.ExecuteReader();

    while( reader.Read() )
    {
      string name = null;
      string statusType = null;
      int personId = -1;
      int statusTypeId = -1;

      try
      {
        name = reader.GetString( 0 );
      }
      catch( Exception )
      {
        // Ignore.
      }

      try
      {
        statusType = reader.GetString( 1 );
      }
      catch( Exception )
      {
        // Ignore.
      }      

      try
      {
        personId = reader.GetInt32( 2 );
      }
      catch( Exception )
      {
        // Ignore.
      }      

      try
      {
        statusTypeId = reader.GetInt32( 3 );
      }
      catch( Exception )
      {
        // Ignore.
      }      

      if( name != null &&
          people.ContainsKey( name ) == false )
      {
        people.Add( name, new Person() );
      }

      Person person = null;

      if( name != null )
      {
        person = people[ name ];

        person.Id = personId;
        person.Name = name;
      }

      if( statusType != null )
      {
        if( person != null )
        {
          person.Status.Add( statusType );
        }

        if( statusTypes.ContainsKey( statusType ) == false )
        {
          statusTypes.Add( statusType, statusTypeId );
        }
      }
    }

    reader.Close();
    connection.Close();

    Body.Controls.Add( BuildTable( people, statusTypes ) );
  }

  //---------------------------------------------------------------------------

  class Person
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Status { get; set; }

    public Person()
    {
      Status = new List<string>();
    }

    public override string ToString()
    {
      var s = "";
      Status.ForEach( ( statusType ) => s += statusType + ' ' );
      return s;
    }
  }

  //---------------------------------------------------------------------------

  Table BuildTable( Dictionary<string, Person> people,
                    SortedDictionary<string, int> statusTypes )
  {
    var table = new Table();
    table.BorderWidth = 1;

    var header = new TableRow();
    table.Rows.Add( header );

    // Header cell for 'Name'.
    header.Cells.Add( new TableCell() );
    header.Cells[ 0 ].Text = "Name";
    header.Cells[ 0 ].Font.Bold = true;

    // Add each status type to the header.
    Dictionary<string, int> statusToColumnIndex = new Dictionary<string, int>();

    foreach( string status in statusTypes.Keys )
    {
      statusToColumnIndex.Add(
        status,
        AddCellToHeaderRowIfNotFound( header, status ) );
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

      foreach( string status in statusTypes.Keys )
      {
        int columnIndex = statusToColumnIndex[ status ];

        AddStatusToRow(
          person.Id,
          statusTypes[ status ],
          person.Status.Contains( status ),
          row,
          columnIndex );
      }

      table.Rows.Add( row );
    }

    return table;
  }

  //---------------------------------------------------------------------------

  int AddCellToHeaderRowIfNotFound( TableRow row, string text )
  {
    for( int i = 0; i < row.Cells.Count; i++ )
    {
      TableCell cell = row.Cells[ i ];

      if( cell.Text == text )
      {
        return i;
      }
    }

    var newCell = new TableCell();
    newCell.Text = text;
    newCell.Font.Bold = true;

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

  void AddStatusToRow( int peopleId,
                       int statusId,
                       bool statusActive,
                       TableRow row,
                       int column,
                       HorizontalAlign align = HorizontalAlign.Center )
  {
    while( column > row.Cells.Count - 1 )
    {
      var cell = new TableCell();
      cell.HorizontalAlign = align;
      row.Cells.Add( cell );

      var button = new ImageButton();
      button.ID = peopleId.ToString() + '~' + statusId.ToString();
      //button.Text = text;
      button.ImageUrl =
        statusActive ?
        "https://cdn2.iconfinder.com/data/icons/basicset/tick_32.png" :
        "https://cdn2.iconfinder.com/data/icons/basicset/delete_32.png";
      button.ToolTip = statusActive ? "active" : "";
      button.Click += HandleStatusClick;

      cell.Controls.Add( button );
    }

    //row.Cells[ column ].Text = text;
  }

  //---------------------------------------------------------------------------

  void HandleStatusClick( object sender, EventArgs args )
  {
    if( sender is ImageButton == false )
    {
      return;
    }

    ImageButton button = (ImageButton)sender;

    string[] buttonId = button.ID.Split( '~' );

    int personId = int.Parse( buttonId[ 0 ] );
    int statusTypeId = int.Parse( buttonId[ 1 ] );

    SqlConnection connection =
      new SqlConnection(
        @"Server=GRAEMEPC\SQLEXPRESS;Database=PeepStat;Trusted_Connection=True;" );

    connection.Open();

    SqlCommand command = null;

    if( button.ToolTip == "active" )
    {
      command =
        new SqlCommand(
          string.Format(
            "DELETE FROM PeopleStatus WHERE peopleId={0} AND statusTypeId={1}",
            personId,
            statusTypeId ),
          connection );
    }
    else
    {
      command =
        new SqlCommand(
          string.Format(
            "INSERT INTO PeopleStatus ( peopleId, statusTypeId ) VALUES( {0}, {1} )",
            personId,
            statusTypeId ),
          connection );
    }

    command.ExecuteNonQuery();
    connection.Close();

    Response.Redirect( Request.RawUrl );
  }

  //---------------------------------------------------------------------------
}