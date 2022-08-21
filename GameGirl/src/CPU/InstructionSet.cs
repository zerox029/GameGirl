using System;
using System.Reflection;
using System.Linq.Expressions;
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
      instructions[0x04] = new Instruction("INC B", 0x04, 1, (value) => INC(registers, reg => reg.B));

      instructions[0x2F] = new Instruction("CPL", 0x2F, 1, (value) => CPL());

      instructions[0x37] = new Instruction("SCF", 0x37, 1, (value) => SCF());
      instructions[0x3F] = new Instruction("CCF", 0x3F, 1, (value) => CCF());

      instructions[0x40] = new Instruction("LD B, B", 0x40, 1, (value) => registers.B = registers.B);
      instructions[0x41] = new Instruction("LD B, C", 0x41, 1, (value) => registers.B = registers.C);
      instructions[0x42] = new Instruction("LD B, D", 0x42, 1, (value) => registers.B = registers.D);
      instructions[0x43] = new Instruction("LD B, E", 0x43, 1, (value) => registers.B = registers.E);
      instructions[0x44] = new Instruction("LD B, H", 0x44, 1, (value) => registers.B = registers.H);
      instructions[0x45] = new Instruction("LD B, L", 0x45, 1, (value) => registers.B = registers.L);
      instructions[0x46] = new Instruction("LD B, [HL]", 0x46, 1, (value) => registers.B = mmu.GetByte(registers.HL));
      instructions[0x47] = new Instruction("LD B, A", 0x47, 1, (value) => registers.B = registers.A);
      instructions[0x48] = new Instruction("LD C, B", 0x48, 1, (value) => registers.C = registers.B);
      instructions[0x49] = new Instruction("LD C, C", 0x49, 1, (value) => registers.C = registers.C);
      instructions[0x4A] = new Instruction("LD C, D", 0x4A, 1, (value) => registers.C = registers.D);
      instructions[0x4B] = new Instruction("LD C, E", 0x4B, 1, (value) => registers.C = registers.E);
      instructions[0x4C] = new Instruction("LD C, H", 0x4C, 1, (value) => registers.C = registers.H);
      instructions[0x4D] = new Instruction("LD C, L", 0x4D, 1, (value) => registers.C = registers.L);
      instructions[0x4E] = new Instruction("LD C, [HL]", 0x4E, 1, (value) => registers.C = mmu.GetByte(registers.HL));
      instructions[0x4F] = new Instruction("LD C, A", 0x4F, 1, (value) => registers.C = registers.A);

      instructions[0x50] = new Instruction("LD D, B", 0x50, 1, (value) => registers.D = registers.B);
      instructions[0x51] = new Instruction("LD D, C", 0x51, 1, (value) => registers.D = registers.C);
      instructions[0x52] = new Instruction("LD D, D", 0x52, 1, (value) => registers.D = registers.D);
      instructions[0x53] = new Instruction("LD D, E", 0x53, 1, (value) => registers.D = registers.E);
      instructions[0x54] = new Instruction("LD D, H", 0x54, 1, (value) => registers.D = registers.H);
      instructions[0x55] = new Instruction("LD D, L", 0x55, 1, (value) => registers.D = registers.L);
      instructions[0x56] = new Instruction("LD D, [HL]", 0x56, 1, (value) => registers.D = mmu.GetByte(registers.HL));
      instructions[0x57] = new Instruction("LD D, A", 0x57, 1, (value) => registers.D = registers.A);
      instructions[0x58] = new Instruction("LD E, B", 0x58, 1, (value) => registers.E = registers.B);
      instructions[0x59] = new Instruction("LD E, C", 0x59, 1, (value) => registers.E = registers.C);
      instructions[0x5A] = new Instruction("LD E, D", 0x5A, 1, (value) => registers.E = registers.D);
      instructions[0x5B] = new Instruction("LD E, E", 0x5B, 1, (value) => registers.E = registers.E);
      instructions[0x5C] = new Instruction("LD E, H", 0x5C, 1, (value) => registers.E = registers.H);
      instructions[0x5D] = new Instruction("LD E, L", 0x5D, 1, (value) => registers.E = registers.L);
      instructions[0x5E] = new Instruction("LD E, [HL]", 0x5E, 1, (value) => registers.E = mmu.GetByte(registers.HL));
      instructions[0x5F] = new Instruction("LD E, A", 0x5F, 1, (value) => registers.E = registers.A);

      instructions[0x60] = new Instruction("LD H, B", 0x60, 1, (value) => registers.H = registers.B);
      instructions[0x61] = new Instruction("LD H, C", 0x61, 1, (value) => registers.H = registers.C);
      instructions[0x62] = new Instruction("LD H, D", 0x62, 1, (value) => registers.H = registers.D);
      instructions[0x63] = new Instruction("LD H, E", 0x63, 1, (value) => registers.H = registers.E);
      instructions[0x64] = new Instruction("LD H, H", 0x64, 1, (value) => registers.H = registers.H);
      instructions[0x65] = new Instruction("LD H, L", 0x65, 1, (value) => registers.H = registers.L);
      instructions[0x66] = new Instruction("LD H, [HL]", 0x66, 1, (value) => registers.H = mmu.GetByte(registers.HL));
      instructions[0x67] = new Instruction("LD H, A", 0x67, 1, (value) => registers.H = registers.A);
      instructions[0x68] = new Instruction("LD L, B", 0x68, 1, (value) => registers.L = registers.B);
      instructions[0x69] = new Instruction("LD L, C", 0x69, 1, (value) => registers.L = registers.C);
      instructions[0x6A] = new Instruction("LD L, D", 0x6A, 1, (value) => registers.L = registers.D);
      instructions[0x6B] = new Instruction("LD L, E", 0x6B, 1, (value) => registers.L = registers.E);
      instructions[0x6C] = new Instruction("LD L, H", 0x6C, 1, (value) => registers.L = registers.H);
      instructions[0x6D] = new Instruction("LD L, L", 0x6D, 1, (value) => registers.L = registers.L);
      instructions[0x6E] = new Instruction("LD L, [HL]", 0x6E, 1, (value) => registers.L = mmu.GetByte(registers.HL));
      instructions[0x6F] = new Instruction("LD L, A", 0x6F, 1, (value) => registers.L = registers.A);

      instructions[0x60] = new Instruction("LD H, B", 0x60, 1, (value) => mmu.WriteByte(registers.HL, registers.B));
      instructions[0x61] = new Instruction("LD H, C", 0x61, 1, (value) => mmu.WriteByte(registers.HL, registers.C));
      instructions[0x62] = new Instruction("LD H, D", 0x62, 1, (value) => mmu.WriteByte(registers.HL, registers.D));
      instructions[0x63] = new Instruction("LD H, E", 0x63, 1, (value) => mmu.WriteByte(registers.HL, registers.E));
      instructions[0x64] = new Instruction("LD H, H", 0x64, 1, (value) => mmu.WriteByte(registers.HL, registers.H));
      instructions[0x65] = new Instruction("LD H, L", 0x65, 1, (value) => mmu.WriteByte(registers.HL, registers.L));
      instructions[0x66] = new Instruction("LD H, [HL]", 0x66, 1, null);
      instructions[0x67] = new Instruction("LD H, A", 0x67, 1, (value) => registers.H = registers.A);
      instructions[0x68] = new Instruction("LD A, B", 0x68, 1, (value) => registers.A = registers.B);
      instructions[0x69] = new Instruction("LD A, C", 0x69, 1, (value) => registers.A = registers.C);
      instructions[0x6A] = new Instruction("LD A, D", 0x6A, 1, (value) => registers.A = registers.D);
      instructions[0x6B] = new Instruction("LD A, E", 0x6B, 1, (value) => registers.A = registers.E);
      instructions[0x6C] = new Instruction("LD A, H", 0x6C, 1, (value) => registers.A = registers.H);
      instructions[0x6D] = new Instruction("LD A, L", 0x6D, 1, (value) => registers.A = registers.L);
      instructions[0x6E] = new Instruction("LD A, [HL]", 0x6E, 1, (value) => registers.A = mmu.GetByte(registers.HL));
      instructions[0x6F] = new Instruction("LD A, A", 0x6F, 1, (value) => registers.A = registers.A);

      instructions[0x80] = new Instruction("ADD A b", 0x80, 1, (value) => ADD(registers.B));
      instructions[0x81] = new Instruction("ADD A c", 0x81, 1, (value) => ADD(registers.C));
      instructions[0x82] = new Instruction("ADD A d", 0x82, 1, (value) => ADD(registers.D));
      instructions[0x83] = new Instruction("ADD A e", 0x83, 1, (value) => ADD(registers.E));
      instructions[0x84] = new Instruction("ADD A h", 0x84, 1, (value) => ADD(registers.H));
      instructions[0x85] = new Instruction("ADD A l", 0x85, 1, (value) => ADD(registers.L));
      instructions[0x86] = new Instruction("ADD A [hl]", 0x86, 1, (value) => ADD(registers.HL));
      instructions[0x87] = new Instruction("ADD A a", 0x87, 1, (value) => ADD(registers.A));
      instructions[0x88] = new Instruction("ADC A b", 0x88, 1, (value) => ADC(registers.B));
      instructions[0x89] = new Instruction("ADC A c", 0x89, 1, (value) => ADC(registers.C));
      instructions[0x8A] = new Instruction("ADC A d", 0x8A, 1, (value) => ADC(registers.D));
      instructions[0x8B] = new Instruction("ADC A e", 0x8B, 1, (value) => ADC(registers.E));
      instructions[0x8C] = new Instruction("ADC A h", 0x8C, 1, (value) => ADC(registers.H));
      instructions[0x8D] = new Instruction("ADC A l", 0x8D, 1, (value) => ADC(registers.L));
      instructions[0x8E] = new Instruction("ADC A [hl]", 0x8E, 1, (value) => ADC(registers.HL));
      instructions[0x8F] = new Instruction("ADC A a", 0x8F, 1, (value) => ADC(registers.A));

      instructions[0x90] = new Instruction("SUB A b", 0x90, 1, (value) => SUB(registers.B));
      instructions[0x91] = new Instruction("SUB A c", 0x91, 1, (value) => SUB(registers.C));
      instructions[0x92] = new Instruction("SUB A d", 0x92, 1, (value) => SUB(registers.D));
      instructions[0x93] = new Instruction("SUB A e", 0x93, 1, (value) => SUB(registers.E));
      instructions[0x94] = new Instruction("SUB A h", 0x94, 1, (value) => SUB(registers.H));
      instructions[0x95] = new Instruction("SUB A l", 0x95, 1, (value) => SUB(registers.L));
      instructions[0x96] = new Instruction("SUB A [hl]", 0x96, 1, (value) => SUB(registers.HL));
      instructions[0x97] = new Instruction("SUB A a", 0x97, 1, (value) => SUB(registers.A));

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
      byte oldValue = registers.A;
      int result = registers.A + value;

      registers.A = (byte)result;

      SetFlags(oldValue, value, false);
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
      byte oldValue = registers.A;
      int carry = registers.GetFlag(Flag.CARRY) ? 1 : 0;
      int result = registers.A + value + carry;

      registers.A = (byte)result;

      SetFlags(oldValue, (byte)(value + carry), false);
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

    private void INC<T>(T target, Expression<Func<T, byte>> outExpr)
    {
      var expr = (MemberExpression)outExpr.Body;
      var prop = (PropertyInfo)expr.Member;

      byte oldValue = (byte)prop.GetValue(target);
      byte newValue = (byte)(oldValue + 1);

      prop.SetValue(target, newValue, null);

      SetFlags(oldValue, 1, false);
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

      registers.SetFlag(Flag.SUBSTRACTION);
      registers.SetFlag(Flag.HALF_CARRY);
    }

    private void CCF()
    {
      registers.FlipFlag(Flag.CARRY);
      registers.ClearFlag(Flag.SUBSTRACTION);
      registers.ClearFlag(Flag.HALF_CARRY);
    }

    private void SCF()
    {
      registers.ClearFlag(Flag.SUBSTRACTION);
      registers.ClearFlag(Flag.HALF_CARRY);
      registers.SetFlag(Flag.CARRY);
    }

    #endregion

    private void SetFlags(byte a, byte b, bool operationWasSubstraction)
    {
      if (operationWasSubstraction)
      {
        registers.SetFlag(Flag.SUBSTRACTION);

        if (a - b == 0)
        {
          registers.SetFlag(Flag.ZERO);
        }
        else
        {
          registers.ClearFlag(Flag.ZERO);
        }
      }
      else
      {
        registers.ClearFlag(Flag.SUBSTRACTION);

        CheckAndSetCarryFlagForAddition(a, b);
        CheckAndSetHalfCarryFlagForAddition(a, b);

        byte val = (byte)(a + b);
        if ((byte)(a + b) == 0)
        {
          registers.SetFlag(Flag.ZERO);
        }
        else
        {
          registers.ClearFlag(Flag.ZERO);
        }
      }
    }

    private void CheckAndSetCarryFlagForAddition(byte a, byte b)
    {
      if (a + b > 0xFF)
      {
        registers.SetFlag(Flag.CARRY);
      }
      else
      {
        registers.ClearFlag(Flag.CARRY);
      }
    }

    private void CheckAndSetHalfCarryFlagForAddition(byte a, byte b)
    {
      if ((((a & 0xF) + (b & 0xF)) & 0x10) == 0x10)
      {
        registers.SetFlag(Flag.HALF_CARRY);
      }
      else
      {
        registers.ClearFlag(Flag.HALF_CARRY);
      }
    }
  }
}