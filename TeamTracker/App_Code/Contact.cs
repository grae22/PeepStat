namespace TeamTracker
{
  public class Contact
  {
    //---------------------------------------------------------------------------

    public Status Type { get; private set; }
    public string Address { get; private set; }

    //---------------------------------------------------------------------------

    public Contact( Status type,
                    string address )
    {
      Type = type;
      Address = address;
    }

    //---------------------------------------------------------------------------
  }
}