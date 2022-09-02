using System;
using GameGirl;

namespace Exceptions
{
  public class InvalidRomException : Exception
  {
    public InvalidRomException() { }

    public InvalidRomException(int length) :
      base(String.Format("The provided ROM was invalid, it should be 0x8000 in length. Was actually {0:X}", length))
    {

    }
  }
}