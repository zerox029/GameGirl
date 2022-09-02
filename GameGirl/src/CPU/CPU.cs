using System;
using System.Threading;
namespace GameGirl
{
  public class CPU
  {
    Registers registers;
    InstructionSet instructionSet;

    MMU mmu;

    public CPU(MMU mmu)
    {
      this.registers = new Registers();
      this.instructionSet = new InstructionSet(registers, mmu);
      this.mmu = mmu;
    }

    public void EmulationLoop()
    {
      while (true)
      {
        ushort pc = registers.PC;
        byte currentOpcode = mmu.ReadByte(registers.PC);
        byte instructionLength = instructionSet.GetInstructionLength(currentOpcode);
        ushort argument = GetArgumentForCurrentOpcode((byte)(instructionLength - 1));

        Console.Write("0x{0:X}: ", pc);

        try
        {
          registers.PC += instructionLength;
          instructionSet.RunInstruction(currentOpcode, argument);
        }
        catch (Exception exception)
        {
          Console.Error.WriteLine(exception.Message);

          return;
        }
      }
    }

    private ushort GetArgumentForCurrentOpcode(byte argumentLength)
    {
      if (argumentLength == 1)
      {
        return mmu.ReadByte((ushort)(registers.PC + 1));
      }
      else if (argumentLength == 2)
      {
        ushort upperNibble = (ushort)(mmu.ReadByte((ushort)(registers.PC + 2)) << 8);
        ushort lowerNibble = mmu.ReadByte((ushort)(registers.PC + 1));

        ushort arg = (ushort)(upperNibble + lowerNibble);
        return arg;
      }
      else
      {
        return 0;
      }
    }
  }
}