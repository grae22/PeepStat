using System.Collections.Generic;

public class Person
{
  //---------------------------------------------------------------------------

  public int Id { get; set; }
  public string Name { get; set; }
  public string Extension { get; set; }
  public List<Status> Status { get; set; }

  //---------------------------------------------------------------------------

  public Person()
  {
    Status = new List<Status>();
  }

  //---------------------------------------------------------------------------
}