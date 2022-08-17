using System;

namespace GameGirl
{
  public class Instruction
  {
    public string Name { get; }
    public byte Opcode { get; }
    public Action Handler { get; }

    public Instruction(string name, byte opcode, Action handler)
    {
      Name = name;
      Opcode = opcode;
      handler = Handler;
    }
  }
}