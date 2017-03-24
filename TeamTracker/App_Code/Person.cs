using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TeamTracker
{
  public class Person : IComparable<Person>
  {
    //-------------------------------------------------------------------------

    public static Dictionary<int, Person> Load(
      SqlConnection connection,
      Dictionary<int, Status> statusTypes )
    {
      Dictionary<int, Person> people = new Dictionary<int, Person>();

      // Load people.
      SqlDataReader reader =
        new SqlCommand(
          "SELECT id, name " +
          "FROM People",
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          people.Add(
            reader.GetInt32( 0 ),
            new Person(
              reader.GetInt32( 0 ),
              reader.GetString( 1 ) ) );
        }
      }

      // Load each person's contacts.
      reader =
        new SqlCommand(
          "SELECT peopleId, statusTypeId, address " +
          "FROM PeopleContact",
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          int personId = reader.GetInt32( 0 );
          int statusTypeId = reader.GetInt32( 1 );
          string address = reader.GetString( 2 );

          people[ personId ].Contacts.Add(
            statusTypes[ statusTypeId ],
            new Contact(
              statusTypes[ statusTypeId ],
              address ) );
        }
      }

      // Load each person's statuses.
      reader =
        new SqlCommand(
          "SELECT peopleId, statusTypeId " +
          "FROM PeopleStatus",
          connection ).ExecuteReader();

      using( reader )
      {
        while( reader.Read() )
        {
          int personId = reader.GetInt32( 0 );
          int statusTypeId = reader.GetInt32( 1 );

          people[ personId ].Statuses.Add(
            statusTypes[ statusTypeId ] );
        }
      }

      return people;
    }

    //=========================================================================

    public int Id { get; private set; }
    public string Name { get; private set; }
    public List<Status> Statuses { get; private set; }
    public Dictionary<Status, Contact> Contacts { get; private set; }

    //-------------------------------------------------------------------------

    public Person( int id,
                   string name )
    {
      Id = id;
      Name = name;
      Statuses = new List<Status>();
      Contacts = new Dictionary<Status, Contact>();
    }

    //-------------------------------------------------------------------------

    public int CompareTo( Person other )
    {
      return Name.CompareTo( other.Name );
    }

    //-------------------------------------------------------------------------
  }
}