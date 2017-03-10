using System;

public class Status : IComparable
{
  //---------------------------------------------------------------------------

  public int Id { get; set; }
  public string Name { get; set; }
  public int SortOrder { get; set; }

  //---------------------------------------------------------------------------

  public int CompareTo( object ob )
  {
    if( ob is Status )
    {
      return SortOrder.CompareTo( ((Status)ob).SortOrder );
    }

    return 0;
  }

  //---------------------------------------------------------------------------
}