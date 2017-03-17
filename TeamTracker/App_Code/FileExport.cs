using System;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace TeamTracker
{
  public class FileExport
  {
    //-------------------------------------------------------------------------

    public static string Export( string filename )
    {
      StringBuilder output = new StringBuilder();
      StringBuilder content = new StringBuilder();

      try
      {
        new FileExport( output, content );

        File.WriteAllText( filename, content.ToString() );

        output.Append( "Export complete." );
      }
      catch( Exception ex )
      {
        output.Append( "Error: " + ex.Message );
      }

      return output.ToString();
    }

    //=========================================================================

    StringBuilder LogOutput;

    //-------------------------------------------------------------------------

    private FileExport( StringBuilder output,
                        StringBuilder content )
    {
      LogOutput = output;

      content.Append( GetDivider( false ) );
      content.Append( Environment.NewLine );
      content.Append( "Version=1.0" + Environment.NewLine + Environment.NewLine );

      content.Append( GetDivider( false ) );
      content.Append( "# NOTE:" + Environment.NewLine );
      content.Append( "# By default all data is erased from the DB when importing, uncomment the" + Environment.NewLine );
      content.Append( "# 'append' keyword in each section to append data instead." + Environment.NewLine );
      content.Append( GetDivider( false ) );
      content.Append(  Environment.NewLine );

      content.Append( GetSettings() );

      content.Append( GetDivider() );
      content.Append( "# WARNING:" + Environment.NewLine );
      content.Append( "# Deleting the data in this section (i.e. not appending) will cause the People" + Environment.NewLine );
      content.Append( "# data to be deleted, too." + Environment.NewLine + Environment.NewLine );
      content.Append( GetStatusTypes() );

      content.Append( GetDivider() );
      content.Append( GetPeople() );

      content.Append( GetDivider() );
    }

    //-------------------------------------------------------------------------

    void Log( string msg )
    {
      LogOutput.Append( msg + "<br />" );
    }

    //-------------------------------------------------------------------------

    string GetDivider( bool padWithBlankLines = true )
    {
      string divider =
        "#------------------------------------------------------------------------------";

      if( padWithBlankLines )
      {
        return
          Environment.NewLine +
          divider +
          Environment.NewLine +
          Environment.NewLine;
      }
      else
      {
        return divider + Environment.NewLine;
      }
    }

    //-------------------------------------------------------------------------

    string GetAsText( string sectionName,
                      SqlDataReader reader )
    {
      Log( "Exporting section " + sectionName + "..." );

      string text =
        ':' + sectionName + Environment.NewLine +
        "#append" + Environment.NewLine;

      // Column headers.
      for( int i = 0; i < reader.FieldCount; i++ )
      {
        text += reader.GetName( i );

        if( i < reader.FieldCount - 1 )
        {
          text += ',';
        }
      }

      text += Environment.NewLine;

      // Data.
      while( reader.Read() )
      {
        for( int i = 0; i < reader.FieldCount; i++ )
        {
          text += reader.GetValue( i ).ToString();

          if( i < reader.FieldCount - 1 )
          {
            text += ',';
          }
        }

        text += Environment.NewLine;
      }

      return text;
    }

    //-------------------------------------------------------------------------

    string GetSettings()
    {
      using( SqlConnection connection = Database.OpenConnection() )
      {
        SqlDataReader reader =
          new SqlCommand(
            "SELECT [key], value FROM Setting",
            connection ).ExecuteReader();

        return GetAsText( "Settings", reader );
      }
    }

    //-------------------------------------------------------------------------

    string GetStatusTypes()
    {
      using( SqlConnection connection = Database.OpenConnection() )
      {
        SqlDataReader reader =
          new SqlCommand(
            "SELECT name, sortOrder, hyperlinkPrefix FROM StatusTypes",
            connection ).ExecuteReader();

        return GetAsText( "StatusTypes", reader );
      }
    }

    //-------------------------------------------------------------------------

    string GetPeople()
    {
      using( SqlConnection connection = Database.OpenConnection() )
      {
        SqlDataReader reader =
          new SqlCommand(
            "SELECT name, statusName, address FROM ExportPeopleView",
            connection ).ExecuteReader();

        return GetAsText( "People", reader );
      }
    }

    //-------------------------------------------------------------------------
  }
}