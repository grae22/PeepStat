using System;
using System.Text;
using System.Web;
using System.IO;

namespace TeamTracker
{
  public class Log
  {
    //---------------------------------------------------------------------------

    public static void LogToFile( Exception ex )
    {
      if( ex == null )
      {
        return;
      }

      StringBuilder builder = new StringBuilder();

      builder.Append( "*********************" + Environment.NewLine );
      builder.Append( ' ' + DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) );
      builder.Append( Environment.NewLine + "*********************" + Environment.NewLine );
      builder.Append( "Type : " + ex.GetType().Name );
      builder.Append( Environment.NewLine );
      builder.Append( "Message : " + ex.Message );
      builder.Append( Environment.NewLine );
      builder.Append( "Source : " + ex.Source );
      builder.Append( Environment.NewLine );

      if( ex.StackTrace != null )
      {
        builder.Append( "Error Trace : " + ex.StackTrace );
        builder.Append( Environment.NewLine );
      }

      Exception innerEx = ex.InnerException;

      while( innerEx != null )
      {
        builder.Append( Environment.NewLine );
        builder.Append( Environment.NewLine );
        builder.Append( "Type : " + innerEx.GetType().Name );
        builder.Append( Environment.NewLine );
        builder.Append( "Message : " + innerEx.Message );
        builder.Append( Environment.NewLine );
        builder.Append( "Source : " + innerEx.Source );
        builder.Append( Environment.NewLine );

        if( ex.StackTrace != null )
        {
          builder.Append( "Error Trace : " + innerEx.StackTrace );
          builder.Append( Environment.NewLine );
        }

        innerEx = innerEx.InnerException;
      }

      string filePath = HttpContext.Current.Server.MapPath( "TeamTracker.log" );
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
        writer.WriteLine( builder.ToString() );
        writer.Write( buffer );
        writer.Flush();
      }
    }

    //---------------------------------------------------------------------------
  }
}