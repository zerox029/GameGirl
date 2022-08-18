using System;
using Exceptions;

namespace GameGirl
{
  public class InstructionSet
  {
    private Instruction[] instructions;
    private Registers registers;
    private MMU mmu;

    public InstructionSet(Registers registers, MMU mmu)
    {
      this.registers = registers;
      this.mmu = mmu;

      GenerateInstructionSet();
    }

    private void GenerateInstructionSet()
    {
      instructions = new Instruction[256];

      instructions[0x00] = new Instruction("NOP", 0x00, 1, (value) => NOP());

      instructions[0x2F] = new Instruction("CPL", 0x2F, 1, (value) => CPL());

      instructions[0x3F] = new Instruction("CCF", 0x3F, 1, (value) => CCF());

      instructions[0x40] = new Instruction("LD B, B", 0x40, 1, (value) => registers.B = registers.B);
      instructions[0x41] = new Instruction("LD B, C", 0x41, 1, (value) => registers.B = registers.C);
      instructions[0x42] = new Instruction("LD B, D", 0x42, 1, (value) => registers.B = registers.D);
      instructions[0x43] = new Instruction("LD B, E", 0x43, 1, (value) => registers.B = registers.E);
      instructions[0x44] = new Instruction("LD B, H", 0x44, 1, (value) => registers.B = registers.H);
      instructions[0x45] = new Instruction("LD B, L", 0x45, 1, (value) => registers.B = registers.L);
      instructions[0x46] = new Instruction("LD B, [HL]", 0x46, 1, (value) => registers.B = mmu.GetByte(registers.HL));
      instructions[0x47] = new Instruction("LD B, A", 0x47, 1, (value) => registers.B = registers.A);

      instructions[0x80] = new Instruction("add A b", 0x80, 1, (value) => ADD(registers.B));
      instructions[0x81] = new Instruction("add A c", 0x81, 1, (value) => ADD(registers.C));
      instructions[0x82] = new Instruction("add A d", 0x82, 1, (value) => ADD(registers.D));
      instructions[0x83] = new Instruction("add A e", 0x83, 1, (value) => ADD(registers.E));
      instructions[0x84] = new Instruction("add A h", 0x84, 1, (value) => ADD(registers.H));
      instructions[0x85] = new Instruction("add A l", 0x85, 1, (value) => ADD(registers.L));
      instructions[0x86] = new Instruction("add A [hl]", 0x86, 1, (value) => ADD(registers.HL));
      instructions[0x87] = new Instruction("add A a", 0x87, 1, (value) => ADD(registers.A));
      instructions[0x88] = new Instruction("adc A b", 0x88, 1, (value) => ADC(registers.B));
      instructions[0x89] = new Instruction("adc A c", 0x89, 1, (value) => ADC(registers.C));
      instructions[0x8A] = new Instruction("adc A d", 0x8A, 1, (value) => ADC(registers.D));
      instructions[0x8B] = new Instruction("adc A e", 0x8B, 1, (value) => ADC(registers.E));
      instructions[0x8C] = new Instruction("adc A h", 0x8C, 1, (value) => ADC(registers.H));
      instructions[0x8D] = new Instruction("adc A l", 0x8D, 1, (value) => ADC(registers.L));
      instructions[0x8E] = new Instruction("adc A [hl]", 0x8E, 1, (value) => ADC(registers.HL));
      instructions[0x8F] = new Instruction("adc A a", 0x8F, 1, (value) => ADC(registers.A));

      instructions[0x90] = new Instruction("sub A b", 0x90, 1, (value) => SUB(registers.B));
      instructions[0x91] = new Instruction("sub A c", 0x91, 1, (value) => SUB(registers.C));
      instructions[0x92] = new Instruction("sub A d", 0x92, 1, (value) => SUB(registers.D));
      instructions[0x93] = new Instruction("sub A e", 0x93, 1, (value) => SUB(registers.E));
      instructions[0x94] = new Instruction("sub A h", 0x94, 1, (value) => SUB(registers.H));
      instructions[0x95] = new Instruction("sub A l", 0x95, 1, (value) => SUB(registers.L));
      instructions[0x96] = new Instruction("sub A [hl]", 0x96, 1, (value) => SUB(registers.HL));
      instructions[0x97] = new Instruction("sub A a", 0x97, 1, (value) => SUB(registers.A));

      instructions[0xC2] = new Instruction("JP NZ, a16", 0xC2, 3, (value) => JP(value, () => !registers.GetFlag(Flag.ZERO)));
      instructions[0xC3] = new Instruction("JP, a16", 0xC3, 3, (value) => JP(value));
      instructions[0xCA] = new Instruction("JP NZ, a16", 0xCA, 3, (value) => JP(value, () => registers.GetFlag(Flag.ZERO)));

      instructions[0xD2] = new Instruction("JP NC, a16", 0xD2, 3, (value) => JP(value, () => !registers.GetFlag(Flag.CARRY)));
      instructions[0xDA] = new Instruction("JP NC, a16", 0xDA, 3, (value) => JP(value, () => registers.GetFlag(Flag.CARRY)));

    }

    public byte GetInstructionLength(byte opcode)
    {
      return instructions[opcode].Length;
    }

    public void RunInstruction(byte opcode)
    {
      Instruction instruction = instructions[opcode];

      if (instruction == null) throw new UnknownOpcodeException(opcode);
      else if (instruction.Handler == null) throw new UnknownOpcodeException(instruction);

      instruction.Handler.Invoke(0);
    }

    public void RunInstruction(byte opcode, ushort argument)
    {
      Instruction instruction = instructions[opcode];

      if (instruction == null) throw new UnknownOpcodeException(opcode);
      else if (instruction.Handler == null) throw new UnknownOpcodeException(instruction);

      instruction.Handler.Invoke(argument);
    }

    #region 8-bit Arithmetic and Logic Instructions

    // Used by opcodes 0x80 to 0x85 and 0x87
    private void ADD(byte value)
    {
      int result = registers.A + value;
      int carry = registers.A ^ value ^ result;

      registers.ClearAllFlags();

      if (result == 0) registers.SetFlag(Flag.ZERO);

      //Carry flags
      if ((carry & 0x100) != 0)
      {
        registers.SetFlag(Flag.CARRY);
      }
      if ((carry & 0x10) != 0)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }

      registers.A = (byte)result;
    }

    // Used by opcode 0x86
    private void ADD(ushort value)
    {
      int result = registers.A + value;
      int carry = registers.A ^ value ^ result;

      registers.ClearAllFlags();

      if (result == 0) registers.SetFlag(Flag.ZERO);

      //Carry flags
      if ((carry & 0x100) != 0)
      {
        registers.SetFlag(Flag.CARRY);
      }
      if ((carry & 0x10) != 0)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }

      registers.A = (byte)result;
    }

    // Used by opcodes 0x88 - 0x8F
    private void ADC(byte value)
    {
      int carry = registers.GetFlag(Flag.CARRY) ? 1 : 0;
      int result = registers.A + value + carry;

      int carryValue = registers.A ^ value ^ result;

      registers.ClearAllFlags();

      if (result == 0) registers.SetFlag(Flag.ZERO);

      //Carry flags
      if ((carry & 0x100) != 0)
      {
        registers.SetFlag(Flag.CARRY);
      }
      if ((carry & 0x10) != 0)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }

      registers.A = (byte)result;
    }

    // Used by opcodes 0x88 to 0x8D and 0x8F 
    private void ADC(ushort value)
    {
      int carry = registers.GetFlag(Flag.CARRY) ? 1 : 0;
      int result = registers.A + value + carry;

      int carryValue = registers.A ^ value ^ result;

      registers.ClearAllFlags();

      if (result == 0) registers.SetFlag(Flag.ZERO);

      //Carry flags
      if ((carry & 0x100) != 0)
      {
        registers.SetFlag(Flag.CARRY);
      }
      if ((carry & 0x10) != 0)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }
    }

    // Used by opcodes 0x90 to 0x95 and 0x97
    private void SUB(byte value)
    {
      int result = registers.A - value;
      int carry = registers.A ^ value ^ result;

      registers.ClearAllFlags();

      registers.SetFlag(Flag.SUBSTRACTION);

      if (result == 0) registers.SetFlag(Flag.ZERO);

      //Carry flags
      if (value > registers.A)
      {
        registers.SetFlag(Flag.CARRY);
      }
      if ((carry & 0x10) != 0)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }
    }

    // Used by opcodes 0x96
    private void SUB(ushort value)
    {
      int result = registers.A - value;
      int carry = registers.A ^ value ^ result;

      registers.ClearAllFlags();

      registers.SetFlag(Flag.SUBSTRACTION);

      if (result == 0) registers.SetFlag(Flag.ZERO);

      //Carry flags
      if (value > registers.A)
      {
        registers.SetFlag(Flag.CARRY);
      }
      if ((carry & 0x10) != 0)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }
    }

    #endregion

    #region Jumps and Subroutines

    private void JP(ushort address)
    {
      registers.PC = address;
    }

    private void JP(ushort address, Func<bool> condition)
    {
      if (condition.Invoke())
      {
        JP(address);
      }
    }

    #endregion

    #region Miscellaneous Instructions

    // Used by opcode 0x00
    private void NOP() { }

    private void CPL()
    {
      var complement = (byte)~registers.A;
      registers.A = complement;
      Console.WriteLine(registers.A);
    }

    private void CCF()
    {
      if (registers.GetFlag(Flag.CARRY))
      {
        registers.ClearFlag(Flag.CARRY);
      }
      else
      {
        registers.SetFlag(Flag.CARRY);
      }
    }

    #endregion
  }
}