using System;

namespace GameGirl
{
  public class Instruction
  {
    public string Name { get; }
    public byte Opcode { get; }
    public byte Length { get; }
    public Action<ushort> Handler { get; }

    public Instruction(string name, byte opcode, byte length, Action<ushort> handler)
    {
      Name = name;
      Opcode = opcode;
      Length = length;
      Handler = handler;
    }
  }
}