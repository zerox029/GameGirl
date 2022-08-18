using System;

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

      RunThroughRomOpcodes();
    }

    //For testing purposes, run through each opcode one by one
    public void RunThroughRomOpcodes()
    {
      foreach (byte b in mmu.GetRom())
      {
        Console.WriteLine("Executing opcode {0:X}", b);

        try
        {
          instructionSet.RunInstruction(b, 0);
        }
        catch (Exception exception)
        {
          Console.Error.WriteLine(exception.Message);

          return;
        }
      }
    }
  }
}