using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace TeamTracker
{
  public class Status : IComparable
  {
    //-------------------------------------------------------------------------

    public static Dictionary<int, Status> Load( SqlConnection connection )
    {
      Dictionary<int, Status> statuses = new Dictionary<int, Status>();

      SqlDataReader reader =
        new SqlCommand(
          "SELECT id, name, sortOrder, hyperlinkPrefix " +
          "FROM StatusTypes",
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          statuses.Add(
            reader.GetInt32( 0 ),
            new Status(
              reader.GetInt32( 0 ),
              reader.GetString( 1 ),
              reader.GetInt32( 2 ),
              reader.GetString( 3 ) ) );
        }
      }          

      return statuses;
    }

    //-------------------------------------------------------------------------

    public static Status GetByName( string name,
                                    IEnumerable<Status> types )
    {
      return types.FirstOrDefault(
        x => x.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
    }

    //=========================================================================

    public int Id { get; private set; }
    public string Name { get; private set; }
    public int SortOrder { get; private set; }
    public string HyperlinkPrefix { get; private set; }

    //-------------------------------------------------------------------------

    public Status( int id,
                   string name,
                   int sortOrder,
                   string hyperlinkPrefix )
    {
      Id = id;
      Name = name;
      SortOrder = sortOrder;
      HyperlinkPrefix = hyperlinkPrefix;
    }

    //-------------------------------------------------------------------------

    public int CompareTo( object ob )
    {
      if( ob is Status )
      {
        return SortOrder.CompareTo( ((Status)ob).SortOrder );
      }

      return 0;
    }

    //-------------------------------------------------------------------------
  }
}