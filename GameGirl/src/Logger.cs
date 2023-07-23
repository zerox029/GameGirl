using System;
using System.IO;

namespace GameGirl
{
  public class Logger
  {
    public static void Log(string line)
    {
      using (StreamWriter writer = new StreamWriter("Log.txt", true))
      {
        writer.WriteLine(line);
      }
    }

    public static void LogWithPrint(string line)
    {
      Console.WriteLine(line);
      Log(line);
    }

    public static void LogWithError(string line)
    {
      Console.Error.WriteLine(line);
      Log(line);
    }
  }
}