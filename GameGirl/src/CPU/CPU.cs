using System;

namespace GameGirl
{
  public class CPU
  {
    Registers registers;
    InstructionSet instructionSet;

    MMU mmu;

    Timer timer;

    private bool isInDebugMode = true;

    public CPU(MMU mmu)
    {
      this.registers = new Registers();
      this.instructionSet = new InstructionSet(registers, mmu);
      this.mmu = mmu;
      this.timer = new Timer(mmu);
    }

    //Setting default start up register values programmatically,
    //this would otherwise have to be done through the bootROM
    public void BootUpSequence()
    {
      registers.AF = 0x01B0;
      registers.BC = 0x0013;
      registers.DE = 0x00D8;
      registers.HL = 0x014D;
      registers.PC = 0x0100;
      registers.SP = 0xFFFE;

      mmu.WriteByte(0xFF10, 0x80);
      mmu.WriteByte(0xFF11, 0xBF);
      mmu.WriteByte(0xFF12, 0xF3);
      mmu.WriteByte(0xFF14, 0xBF);
      mmu.WriteByte(0xFF16, 0x3F);
      mmu.WriteByte(0xFF19, 0xBF);
      mmu.WriteByte(0xFF1A, 0x7F);
      mmu.WriteByte(0xFF1B, 0xFF);
      mmu.WriteByte(0xFF1C, 0x9F);
      mmu.WriteByte(0xFF1E, 0xBF);
      mmu.WriteByte(0xFF20, 0xFF);
      mmu.WriteByte(0xFF23, 0xBF);
      mmu.WriteByte(0xFF24, 0x77);
      mmu.WriteByte(0xFF25, 0xF3);
      mmu.WriteByte(0xFF26, 0xF1);
      mmu.WriteByte(0xFF40, 0x91);
      mmu.WriteByte(0xFF47, 0xFC);
      mmu.WriteByte(0xFF48, 0xFF);
      mmu.WriteByte(0xFF49, 0xFF);
    }

    public void EmulationLoop()
    {
      int totalCycles = 0;
      while (true)
      {
        byte currentOpcode = mmu.ReadByte(registers.PC);
        byte instructionLength = instructionSet.GetInstructionLength(currentOpcode);
        ushort argument = GetArgumentForCurrentOpcode((byte)(instructionLength - 1));

        try
        {
          registers.PC += instructionLength;

          byte executedCycles = instructionSet.RunInstruction(currentOpcode, argument, isInDebugMode);
          totalCycles += executedCycles;

          timer.Update(executedCycles);
        }
        catch (Exception exception)
        {
          Logger.LogWithError($"Error when executing opcode 0x{currentOpcode:X2} at address 0x{registers.PC - instructionLength:X4}: {exception.Message}");

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

        return (ushort)(upperNibble + lowerNibble);
      }
      else
      {
        return 0;
      }
    }
  }
}