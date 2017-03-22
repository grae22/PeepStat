using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamTracker
{
  public class FileImport
  {
    //-------------------------------------------------------------------------

    public static string Import( string filename )
    {
      StringBuilder output = new StringBuilder();

      string[] lines = File.ReadAllLines( filename );

      if( lines.Length > 0 )
      {
        new FileImport( lines, output );
      }
      else
      {
        output.Append( "File empty, nothing imported." );
      }

      return output.ToString();
    }

    //=========================================================================

    StringBuilder LogOutput;
    DbTransaction Transaction;

    //-------------------------------------------------------------------------

    private FileImport( string[] lines,
                        StringBuilder output )
    {
      LogOutput = output;

      List<string> sanitisedLines = SanitiseLines( lines );

      if( GetVersion( sanitisedLines ) != "1.0" )
      {
        Log( "Invalid version, import aborted." );
        return;
      }

      try
      {
        Transaction = new DbTransaction( "Import" );

        ImportSettings( sanitisedLines );
        ImportStatusTypes( sanitisedLines );
        ImportPeople( sanitisedLines );

        Transaction.Commit();

        output.Append( "Import complete." );
      }
      catch( Exception ex )
      {
        if( Transaction != null )
        {
          Transaction.Rollback();
        }

        Log( "Error:" + ex.Message );
        Log( "All changes rolled back." );
      }
    }

    //-------------------------------------------------------------------------

    void Log( string msg )
    {
      LogOutput.Append( msg + "<br />" );
    }

    //-------------------------------------------------------------------------

    List<string> SanitiseLines( string[] lines )
    {
      List<string> result = new List<string>();

      // Remove blank lines, comments and certain chars.
      foreach( string l in lines )
      {
        string sanitisedLine = l.Trim( new char[] { ' ', '\n', '\r', '\t' } );

        if( sanitisedLine.Length == 0 ||
            sanitisedLine[ 0 ] == '#' )
        {
          continue;
        }

        sanitisedLine = sanitisedLine.Replace( "'", "''" );

        result.Add( sanitisedLine );
      }

      return result;
    }

    //-------------------------------------------------------------------------

    string GetVersion( List<string> lines )
    {
      string version = lines.FirstOrDefault( x => x.ToLower().Contains( "version=" ) );

      if( version == null )
      {
        Log( "Version not found." );
        return "0";
      }

      return version.Remove( 0, "version=".Length );
    }

    //-------------------------------------------------------------------------

    void ImportSettings( List<string> lines )
    {
      try
      {
        int lineIndex = 0;

        Log( "Importing Settings..." );

        // Find the settings.
        for( lineIndex = 0; lineIndex < lines.Count; lineIndex++ )
        {
          if( lines[ lineIndex ].ToLower().IndexOf( ":settings" ) == 0 )
          {
            break;
          }
        }

        if( lineIndex == lines.Count )
        {
          throw new Exception( "No Settings section not found." );
        }

        // Append data?
        lineIndex++;

        bool appendData =
          lineIndex < lines.Count &&
          lines[ lineIndex ].ToLower() == "append";

        if( appendData )
        {
          lineIndex++;
        }

        // Column headers.
        if( lineIndex > lines.Count - 1 ||
            lines[ lineIndex ].ToLower() != ( "key,value" )  )
        {
          throw new Exception( "Expected Settings column headers: 'key,value'." );
        }

        // Delete current data?
        if( appendData == false )
        {
          Transaction.ExecNonQuery( "TRUNCATE TABLE Setting" );
        }

        // Interate through data.
        int itemCount = 0;
        int importedItemCount = 0;

        for( lineIndex++; lineIndex < lines.Count; lineIndex++ )
        {
          // Encountered another section?
          if( lines[ lineIndex ][ 0 ] == ':' )
          {
            break;
          }

          string[] data = lines[ lineIndex ].Split( ',' );
          itemCount++;

          if( data.Length != 2 )
          {
            throw new Exception(
              string.Format(
                "Item {0} appears to be missing one or more fields, skipping.",
                itemCount ) );
          }

          int rowCount =
            Transaction.ExecNonQuery(
              string.Format(
                "INSERT INTO Setting ( [Key], Value ) " +
                "VALUES ( '{0}', '{1}' )",
                data[ 0 ],
                data[ 1 ] ) );

          if( rowCount == 0 )
          {
            throw new Exception(
              string.Format(
                "Failed to add item {0} to the DB.",
                itemCount ) );
          }
          else
          {
            importedItemCount++;
          }
        }

        Log( string.Format( "Imported {0} Settings.", importedItemCount ) );
      }
      catch( Exception ex )
      {
        throw new Exception( "Error while importing Settings: " + ex.Message );
      }
    }

    //-------------------------------------------------------------------------

    void ImportStatusTypes( List<string> lines )
    {
      try
      {
        int lineIndex = 0;

        Log( "Importing StatusTypes..." );

        // Find the status types.
        for( lineIndex = 0; lineIndex < lines.Count; lineIndex++ )
        {
          if( lines[ lineIndex ].ToLower().IndexOf( ":statustypes" ) == 0 )
          {
            break;
          }
        }

        if( lineIndex == lines.Count )
        {
          throw new Exception( "No StatusTypes section not found." );
        }

        // Append data?
        lineIndex++;

        bool appendData =
          lineIndex < lines.Count &&
          lines[ lineIndex ].ToLower() == "append";

        if( appendData )
        {
          lineIndex++;
        }

        // Column headers.
        if( lineIndex > lines.Count - 1 ||
            lines[ lineIndex ].ToLower() != ( "name,sortorder,hyperlinkprefix" )  )
        {
          throw new Exception(
            "Expected StatusTypes column headers: 'name,sortOrder,hyperlinkPrefix'." );
        }

        // Delete current data?
        if( appendData == false )
        {
          Transaction.ExecNonQuery( "TRUNCATE TABLE PeopleStatus" );
          Transaction.ExecNonQuery( "TRUNCATE TABLE PeopleContact" );
          Transaction.ExecNonQuery( "DELETE FROM People" );
          Transaction.ExecNonQuery( "DELETE FROM StatusTypes" );

          Transaction.ExecNonQuery( "DBCC CHECKIDENT ( '[People]', RESEED, 0 )" );
          Transaction.ExecNonQuery( "DBCC CHECKIDENT ( '[StatusTypes]', RESEED, 0 )" );
        }

        // Interate through data.
        int itemCount = 0;
        int importedItemCount = 0;

        for( lineIndex++; lineIndex < lines.Count; lineIndex++ )
        {
          // Encountered another section?
          if( lines[ lineIndex ][ 0 ] == ':' )
          {
            break;
          }

          string[] data = lines[ lineIndex ].Split( ',' );
          itemCount++;

          if( data.Length != 3 )
          {
            throw new Exception(
              string.Format(
                "Item {0} appears to be missing one or more fields, skipping.",
                itemCount ) );
          }

          int rowCount =
            Transaction.ExecNonQuery(
              string.Format(
                "INSERT INTO StatusTypes ( name, sortOrder, hyperlinkPrefix ) " +
                "VALUES ( '{0}', '{1}', '{2}' )",
                data[ 0 ],
                data[ 1 ],
                data[ 2 ] ) );

          if( rowCount == 0 )
          {
            throw new Exception(
              string.Format(
                "Failed to add item {0} to the DB.",
                itemCount ) );
          }
          else
          {
            importedItemCount++;
          }
        }

        Log( string.Format( "Imported {0} StatusTypes.", importedItemCount ) );
      }
      catch( Exception ex )
      {
        throw new Exception( "Error while importing StatusTypes: " + ex.Message );
      }
    }

    //-------------------------------------------------------------------------

    void ImportPeople( List<string> lines )
    {
      try
      {
        int lineIndex = 0;

        Log( "Importing People..." );

        // Find the People section.
        for( lineIndex = 0; lineIndex < lines.Count; lineIndex++ )
        {
          if( lines[ lineIndex ].ToLower().IndexOf( ":people" ) == 0 )
          {
            break;
          }
        }

        if( lineIndex == lines.Count )
        {
          throw new Exception( "No People section not found." );
        }

        // Append data?
        lineIndex++;

        bool appendData =
          lineIndex < lines.Count &&
          lines[ lineIndex ].ToLower() == "append";

        if( appendData )
        {
          lineIndex++;
        }

        // Column headers.
        if( lineIndex > lines.Count - 1 ||
            lines[ lineIndex ].ToLower() != ( "name,statusname,address" )  )
        {
          throw new Exception(
            "Expected StatusTypes column headers: 'name,statusName,address'." );
        }

        // Delete current data?
        if( appendData == false )
        {
          Transaction.ExecNonQuery( "TRUNCATE TABLE PeopleStatus" );
          Transaction.ExecNonQuery( "TRUNCATE TABLE PeopleContact" );
          Transaction.ExecNonQuery( "DELETE FROM People" );

          Transaction.ExecNonQuery( "DBCC CHECKIDENT ( '[People]', RESEED, 0 )" );
        }

        // We need the status types.
        Dictionary<int, Status> statusTypes = Status.Load( Transaction.Connection );

        // Interate through data.
        Dictionary<int, Person> people = new Dictionary<int, Person>();

        int rowCount;
        int itemCount = 0;
        int importedPeopleCount = 0;
        int importedContactCount = 0;

        for( lineIndex++; lineIndex < lines.Count; lineIndex++ )
        {
          // Encountered another section?
          if( lines[ lineIndex ][ 0 ] == ':' )
          {
            break;
          }

          string[] data = lines[ lineIndex ].Split( ',' );
          itemCount++;

          if( data.Length != 3 )
          {
            throw new Exception(
              string.Format(
                "Item {0} appears to be missing one or more fields, skipping.",
                itemCount ) );
          }

          string personName = data[ 0 ];
          string statusName = data[ 1 ];
          string address = data[ 2 ];

          // Person already in DB?
          int personId = -1;

          Person person = people.Values.FirstOrDefault( x => x.Name == personName );

          if( person != null )
          {
            personId = person.Id;
          }

          // Add person to People table.
          if( personId < 0 )
          {
            rowCount =
              Transaction.ExecNonQuery(
                string.Format(
                  "INSERT INTO People ( name ) " +
                  "VALUES ( '{0}' )",
                  personName ) );

            if( rowCount == 0 )
            {
              throw new Exception(
                string.Format(
                  "Failed to add item {0} to the People table.",
                  itemCount ) );
            }
            else
            {
              importedPeopleCount++;
            }

            // Reload the People.
            people = Person.Load( Transaction.Connection, statusTypes );

            person = people.Values.FirstOrDefault( x => x.Name == personName );

            if( person != null )
            {
              personId = person.Id;
            }
            else
            {
              throw new Exception(
                string.Format(
                  "Failed to find person '{0}' that was just added.",
                  personName ) );
            }
          }

          // Add person's contact.
          Status status = statusTypes.Values.FirstOrDefault( x => x.Name == statusName );

          if( status == null )
          {
            throw new Exception(
              string.Format(
                "Failed to find StatusType with name '{0}' for person '{1}'.",
                statusName,
                personName ) );
          }

          rowCount =
            Transaction.ExecNonQuery(
              string.Format(
                "INSERT INTO PeopleContact ( peopleId, statusTypeId, address )" +
                  "VALUES ( {0}, {1}, '{2}' )",
                personId,
                status.Id,
                address ) );

          if( rowCount == 0 )
          {
            throw new Exception(
              string.Format(
                "Failed to import contact '{0}' for person '{0}'.",
                statusName,
                personName ) );
          }
          else
          {
            importedContactCount++;
          }
        }

        Log( string.Format( "Imported {0} people.", importedPeopleCount ) );
        Log( string.Format( "Imported {0} contacts.", importedContactCount ) );
      }
      catch( Exception ex )
      {
        throw new Exception( "Error while importing People: " + ex.Message );
      }
    }

    //-------------------------------------------------------------------------
  }
}