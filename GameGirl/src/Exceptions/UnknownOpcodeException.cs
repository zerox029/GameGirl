using System;
using GameGirl;

namespace Exceptions
{
  public class UnknownOpcodeException : Exception
  {
    public UnknownOpcodeException() { }

    public UnknownOpcodeException(byte opcode) :
      base(String.Format("Unknown opcode {0:X}", opcode))
    {

    }

    public UnknownOpcodeException(Instruction instruction) :
      base(String.Format("Unknown opcode {0:X} with label {1}", instruction.Opcode, instruction.Name))
    {

    }
  }
}