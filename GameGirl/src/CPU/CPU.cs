using System;

namespace GameGirl
{
  public class CPU
  {
    Registers registers;
    InstructionSet instructionSet;

    MMU mmu;

    Timer timer;

    InterruptHandler interruptHandler;

    private bool isInDebugMode = false;

    public CPU(MMU mmu)
    {
      this.mmu = mmu;
      this.registers = new Registers();
      this.instructionSet = new InstructionSet(registers, mmu);
      this.timer = new Timer(mmu);
      this.interruptHandler = new InterruptHandler(registers, mmu);
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

      (ushort address, byte value)[] writeOperations = new (ushort address, byte value)[]
      {
        (0xFF10, 0x80), (0xFF11, 0xBF), (0xFF12, 0xF3), (0xFF14, 0xBF),
        (0xFF16, 0x3F), (0xFF19, 0xBF), (0xFF1A, 0x7F), (0xFF1B, 0xFF),
        (0xFF1C, 0x9F), (0xFF1E, 0xBF), (0xFF20, 0xFF), (0xFF23, 0xBF),
        (0xFF24, 0x77), (0xFF25, 0xF3), (0xFF26, 0xF1), (0xFF40, 0x91),
        (0xFF47, 0xFC), (0xFF48, 0xFF), (0xFF49, 0xFF)
      };

      mmu.BatchWriteBytes(writeOperations);
    }

    public void EmulationLoop()
    {
      int totalCycles = 0;
      while (true)
      {
        int executedCycles = RunNextOpcode();
        totalCycles += executedCycles;
        timer.Update(executedCycles);

        interruptHandler.CheckInterrupts();
      }
    }

    private int RunNextOpcode()
    {
      byte currentOpcode = mmu.ReadByte(registers.PC);
      byte instructionLength = instructionSet.GetInstructionLength(currentOpcode);
      ushort argument = GetArgumentForCurrentOpcode((byte)(instructionLength - 1));

      try
      {
        registers.PC += instructionLength;

        return instructionSet.RunInstruction(currentOpcode, argument, isInDebugMode);
      }
      catch (Exception exception)
      {
        Logger.LogWithError($"Error when executing opcode 0x{currentOpcode:X2} at address 0x{registers.PC - instructionLength:X4}: {exception.Message}");

        return 0;
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