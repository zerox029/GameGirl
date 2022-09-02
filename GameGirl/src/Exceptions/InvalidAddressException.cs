using System;
using GameGirl;

namespace Exceptions
{
  public class InvalidAddressException : Exception
  {
    public InvalidAddressException() { }

    public InvalidAddressException(ushort address) :
      base(String.Format("Invalid address {0:X}", address))
    {

    }
  }
}