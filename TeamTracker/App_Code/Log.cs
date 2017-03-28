using System;
using System.Text;
using System.Web;
using System.IO;

namespace TeamTracker
{
  public class Log
  {
    //---------------------------------------------------------------------------

    public const string FILENAME = "TeamTracker.log";

    //---------------------------------------------------------------------------

    public static void LogToFile( string msg )
    {
      AppendToLogFile(
        string.Format(
          "{0}{1}",
          GetTimestampPrefix(),
          msg ) );
    }
    
    //---------------------------------------------------------------------------

    public static void LogToFile( Exception ex )
    {
      if( ex == null )
      {
        return;
      }

      StringBuilder builder = new StringBuilder();

      string timestampPrefix = GetTimestampPrefix();

      builder.Append( timestampPrefix + "*********************" + Environment.NewLine );
      builder.Append( timestampPrefix + "Type : " + ex.GetType().Name );
      builder.Append( Environment.NewLine );
      builder.Append( timestampPrefix + "Message : " + ex.Message );
      builder.Append( Environment.NewLine );
      builder.Append( timestampPrefix + "Source : " + ex.Source );
      builder.Append( Environment.NewLine );

      if( ex.StackTrace != null )
      {
        builder.Append( timestampPrefix + "Error Trace : " + ex.StackTrace );
        builder.Append( Environment.NewLine );
      }

      Exception innerEx = ex.InnerException;

      while( innerEx != null )
      {
        builder.Append( Environment.NewLine );
        builder.Append( Environment.NewLine );
        builder.Append( timestampPrefix + "Type : " + innerEx.GetType().Name );
        builder.Append( Environment.NewLine );
        builder.Append( timestampPrefix + "Message : " + innerEx.Message );
        builder.Append( Environment.NewLine );
        builder.Append( timestampPrefix + "Source : " + innerEx.Source );
        builder.Append( Environment.NewLine );

        if( ex.StackTrace != null )
        {
          builder.Append( timestampPrefix + "Error Trace : " + innerEx.StackTrace );
          builder.Append( Environment.NewLine );
        }

        innerEx = innerEx.InnerException;
      }

      AppendToLogFile( builder.ToString() );
    }

    //---------------------------------------------------------------------------

    public static void ClearLog()
    {
      File.WriteAllText( HttpContext.Current.Server.MapPath( FILENAME ), "" );
    }

    //---------------------------------------------------------------------------

    static string GetTimestampPrefix()
    {
      return DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ) + " | ";
    }

    //---------------------------------------------------------------------------

    static void AppendToLogFile( string content )
    {
      string filePath = HttpContext.Current.Server.MapPath( FILENAME );
      string buffer = "";
      bool fileExists = File.Exists( filePath );

      if( fileExists )
      {
        using( var reader = new StreamReader( filePath ) )
        {
          buffer = reader.ReadToEnd();
        }
      }

      using( var writer = new StreamWriter( filePath ) )
      {
        writer.WriteLine( content );
        writer.Write( buffer );
        writer.Flush();
      }
    }

    //---------------------------------------------------------------------------
  }
}