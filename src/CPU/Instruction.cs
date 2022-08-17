using System;

namespace GameGirl
{
  public class Instruction
  {
    public string Name { get; }
    public byte Opcode { get; }
    public Action<byte> Handler { get; }

    public Instruction(string name, byte opcode, Action<byte> handler)
    {
      Name = name;
      Opcode = opcode;
      handler = Handler;
    }
  }
}