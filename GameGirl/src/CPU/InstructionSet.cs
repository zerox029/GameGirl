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
      instructions[0x01] = new Instruction("LD BC, d16", 0x01, 3, (value) => registers.BC = value);
      instructions[0x02] = new Instruction("LD [BC], A", 0x02, 1, (value) => mmu.WriteByte(registers.BC, registers.A));
      instructions[0x03] = new Instruction("INC BC", 0x03, 1, (value) => INC(registers, reg => registers.BC));
      instructions[0x04] = new Instruction("INC B", 0x04, 1, (value) => INC(registers, reg => reg.B));
      instructions[0x05] = new Instruction("DEC B", 0x05, 1, (value) => DEC(registers, reg => reg.B));
      instructions[0x06] = new Instruction("LD B, d8", 0x06, 2, (value) => registers.B = (byte)value);
      instructions[0x0A] = new Instruction("LD A, [BC]", 0x0A, 1, (value) => registers.A = mmu.ReadByte(registers.BC));
      instructions[0x0B] = new Instruction("DEC BC", 0x0B, 1, (value) => DEC(registers, reg => reg.BC));
      instructions[0x0C] = new Instruction("INC C", 0x0C, 1, (value) => INC(registers, reg => reg.C));
      instructions[0x0D] = new Instruction("DEC C", 0x0D, 1, (value) => DEC(registers, reg => reg.C));
      instructions[0x0E] = new Instruction("LD C, d8", 0x0E, 2, (value) => registers.C = (byte)value);

      instructions[0x11] = new Instruction("LD DE, d16", 0x11, 3, (value) => registers.DE = value);
      instructions[0x12] = new Instruction("LD [DE], A", 0x12, 1, (value) => mmu.WriteByte(registers.DE, registers.A));
      instructions[0x13] = new Instruction("INC DE", 0x13, 1, (value) => INC(registers, reg => registers.DE));
      instructions[0x14] = new Instruction("INC D", 0x14, 1, (value) => INC(registers, reg => reg.D));
      instructions[0x15] = new Instruction("DEC D", 0x15, 1, (value) => DEC(registers, reg => reg.D));
      instructions[0x16] = new Instruction("LD D, d8", 0x16, 2, (value) => registers.D = (byte)value);
      instructions[0x1A] = new Instruction("LD A, [DE]", 0x1A, 1, (value) => registers.A = mmu.ReadByte(registers.DE));
      instructions[0x1B] = new Instruction("DEC DE", 0x1B, 1, (value) => DEC(registers, reg => reg.DE));
      instructions[0x1C] = new Instruction("INC E", 0x1C, 1, (value) => INC(registers, reg => reg.E));
      instructions[0x1D] = new Instruction("DEC E", 0x1D, 1, (value) => DEC(registers, reg => reg.E));
      instructions[0x1E] = new Instruction("LD E, d8", 0x1E, 2, (value) => registers.E = (byte)value);

      instructions[0x20] = new Instruction("JR NZ, s8", 0x20, 2, (value) => JR((sbyte)value, () => registers.GetFlag(Flag.ZERO) == false));
      instructions[0x21] = new Instruction("LD HL, d16", 0x21, 3, (value) => registers.HL = value);
      instructions[0x22] = new Instruction("LD [HL+], A", 0x22, 1, (value) => { mmu.WriteByte(registers.HL, registers.A); registers.HL++; });
      instructions[0x23] = new Instruction("INC HL", 0x23, 1, (value) => INC(registers, reg => registers.HL));
      instructions[0x24] = new Instruction("INC H", 0x24, 1, (value) => INC(registers, reg => reg.H));
      instructions[0x25] = new Instruction("DEC H", 0x25, 1, (value) => DEC(registers, reg => reg.H));
      instructions[0x26] = new Instruction("LD H, d8", 0x26, 2, (value) => registers.H = (byte)value);
      instructions[0x2A] = new Instruction("LD A, [HL+]", 0x2A, 1, (value) => { registers.A = mmu.ReadByte(registers.HL); registers.HL++; });
      instructions[0x2B] = new Instruction("DEC HL", 0x2B, 1, (value) => DEC(registers, reg => reg.HL));
      instructions[0x2C] = new Instruction("INC L", 0x2C, 1, (value) => INC(registers, reg => reg.L));
      instructions[0x2D] = new Instruction("DEC L", 0x2D, 1, (value) => DEC(registers, reg => reg.L));
      instructions[0x2E] = new Instruction("LD L, d8", 0x2E, 2, (value) => registers.L = (byte)value);
      instructions[0x2F] = new Instruction("CPL", 0x2F, 1, (value) => CPL());

      instructions[0x31] = new Instruction("LD SP, d16", 0x31, 3, (value) => registers.SP = value);
      instructions[0x32] = new Instruction("LD [HL-], A", 0x32, 1, (value) => { mmu.WriteByte(registers.HL, registers.A); registers.HL--; });
      instructions[0x33] = new Instruction("INC SP", 0x33, 1, (value) => INC(registers, reg => registers.SP));
      instructions[0x34] = new Instruction("INC [HL]", 0x34, 1, (value) => INC(registers, reg => mmu.ReadByte(registers.HL)));
      instructions[0x35] = new Instruction("DEC [HL]", 0x35, 1, (value) => DEC(registers, reg => mmu.ReadByte(registers.HL)));
      instructions[0x36] = new Instruction("LD [HL], d8", 0x36, 2, (value) => mmu.WriteByte(registers.HL, (byte)value));
      instructions[0x37] = new Instruction("SCF", 0x37, 1, (value) => SCF());
      instructions[0x3A] = new Instruction("LD A, [HL-]", 0x3A, 1, (value) => { registers.A = mmu.ReadByte(registers.HL); registers.HL--; });
      instructions[0x3B] = new Instruction("DEC SP", 0x3B, 1, (value) => DEC(registers, reg => reg.SP));
      instructions[0x3C] = new Instruction("INC A", 0x3C, 1, (value) => INC(registers, reg => reg.A));
      instructions[0x3D] = new Instruction("DEC A", 0x3D, 1, (value) => DEC(registers, reg => reg.A));
      instructions[0x3E] = new Instruction("LD A, d8", 0x3E, 2, (value) => registers.A = (byte)value);
      instructions[0x3F] = new Instruction("CCF", 0x3F, 1, (value) => CCF());

      instructions[0x40] = new Instruction("LD B, B", 0x40, 1, (value) => registers.B = registers.B);
      instructions[0x41] = new Instruction("LD B, C", 0x41, 1, (value) => registers.B = registers.C);
      instructions[0x42] = new Instruction("LD B, D", 0x42, 1, (value) => registers.B = registers.D);
      instructions[0x43] = new Instruction("LD B, E", 0x43, 1, (value) => registers.B = registers.E);
      instructions[0x44] = new Instruction("LD B, H", 0x44, 1, (value) => registers.B = registers.H);
      instructions[0x45] = new Instruction("LD B, L", 0x45, 1, (value) => registers.B = registers.L);
      instructions[0x46] = new Instruction("LD B, [HL]", 0x46, 1, (value) => registers.B = mmu.ReadByte(registers.HL));
      instructions[0x47] = new Instruction("LD B, A", 0x47, 1, (value) => registers.B = registers.A);
      instructions[0x48] = new Instruction("LD C, B", 0x48, 1, (value) => registers.C = registers.B);
      instructions[0x49] = new Instruction("LD C, C", 0x49, 1, (value) => registers.C = registers.C);
      instructions[0x4A] = new Instruction("LD C, D", 0x4A, 1, (value) => registers.C = registers.D);
      instructions[0x4B] = new Instruction("LD C, E", 0x4B, 1, (value) => registers.C = registers.E);
      instructions[0x4C] = new Instruction("LD C, H", 0x4C, 1, (value) => registers.C = registers.H);
      instructions[0x4D] = new Instruction("LD C, L", 0x4D, 1, (value) => registers.C = registers.L);
      instructions[0x4E] = new Instruction("LD C, [HL]", 0x4E, 1, (value) => registers.C = mmu.ReadByte(registers.HL));
      instructions[0x4F] = new Instruction("LD C, A", 0x4F, 1, (value) => registers.C = registers.A);

      instructions[0x50] = new Instruction("LD D, B", 0x50, 1, (value) => registers.D = registers.B);
      instructions[0x51] = new Instruction("LD D, C", 0x51, 1, (value) => registers.D = registers.C);
      instructions[0x52] = new Instruction("LD D, D", 0x52, 1, (value) => registers.D = registers.D);
      instructions[0x53] = new Instruction("LD D, E", 0x53, 1, (value) => registers.D = registers.E);
      instructions[0x54] = new Instruction("LD D, H", 0x54, 1, (value) => registers.D = registers.H);
      instructions[0x55] = new Instruction("LD D, L", 0x55, 1, (value) => registers.D = registers.L);
      instructions[0x56] = new Instruction("LD D, [HL]", 0x56, 1, (value) => registers.D = mmu.ReadByte(registers.HL));
      instructions[0x57] = new Instruction("LD D, A", 0x57, 1, (value) => registers.D = registers.A);
      instructions[0x58] = new Instruction("LD E, B", 0x58, 1, (value) => registers.E = registers.B);
      instructions[0x59] = new Instruction("LD E, C", 0x59, 1, (value) => registers.E = registers.C);
      instructions[0x5A] = new Instruction("LD E, D", 0x5A, 1, (value) => registers.E = registers.D);
      instructions[0x5B] = new Instruction("LD E, E", 0x5B, 1, (value) => registers.E = registers.E);
      instructions[0x5C] = new Instruction("LD E, H", 0x5C, 1, (value) => registers.E = registers.H);
      instructions[0x5D] = new Instruction("LD E, L", 0x5D, 1, (value) => registers.E = registers.L);
      instructions[0x5E] = new Instruction("LD E, [HL]", 0x5E, 1, (value) => registers.E = mmu.ReadByte(registers.HL));
      instructions[0x5F] = new Instruction("LD E, A", 0x5F, 1, (value) => registers.E = registers.A);

      instructions[0x60] = new Instruction("LD H, B", 0x60, 1, (value) => registers.H = registers.B);
      instructions[0x61] = new Instruction("LD H, C", 0x61, 1, (value) => registers.H = registers.C);
      instructions[0x62] = new Instruction("LD H, D", 0x62, 1, (value) => registers.H = registers.D);
      instructions[0x63] = new Instruction("LD H, E", 0x63, 1, (value) => registers.H = registers.E);
      instructions[0x64] = new Instruction("LD H, H", 0x64, 1, (value) => registers.H = registers.H);
      instructions[0x65] = new Instruction("LD H, L", 0x65, 1, (value) => registers.H = registers.L);
      instructions[0x66] = new Instruction("LD H, [HL]", 0x66, 1, (value) => registers.H = mmu.ReadByte(registers.HL));
      instructions[0x67] = new Instruction("LD H, A", 0x67, 1, (value) => registers.H = registers.A);
      instructions[0x68] = new Instruction("LD L, B", 0x68, 1, (value) => registers.L = registers.B);
      instructions[0x69] = new Instruction("LD L, C", 0x69, 1, (value) => registers.L = registers.C);
      instructions[0x6A] = new Instruction("LD L, D", 0x6A, 1, (value) => registers.L = registers.D);
      instructions[0x6B] = new Instruction("LD L, E", 0x6B, 1, (value) => registers.L = registers.E);
      instructions[0x6C] = new Instruction("LD L, H", 0x6C, 1, (value) => registers.L = registers.H);
      instructions[0x6D] = new Instruction("LD L, L", 0x6D, 1, (value) => registers.L = registers.L);
      instructions[0x6E] = new Instruction("LD L, [HL]", 0x6E, 1, (value) => registers.L = mmu.ReadByte(registers.HL));
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
      instructions[0x6E] = new Instruction("LD A, [HL]", 0x6E, 1, (value) => registers.A = mmu.ReadByte(registers.HL));
      instructions[0x6F] = new Instruction("LD A, A", 0x6F, 1, (value) => registers.A = registers.A);

      instructions[0x80] = new Instruction("ADD A b", 0x80, 1, (value) => ADD(registers.B));
      instructions[0x81] = new Instruction("ADD A c", 0x81, 1, (value) => ADD(registers.C));
      instructions[0x82] = new Instruction("ADD A d", 0x82, 1, (value) => ADD(registers.D));
      instructions[0x83] = new Instruction("ADD A e", 0x83, 1, (value) => ADD(registers.E));
      instructions[0x84] = new Instruction("ADD A h", 0x84, 1, (value) => ADD(registers.H));
      instructions[0x85] = new Instruction("ADD A l", 0x85, 1, (value) => ADD(registers.L));
      instructions[0x86] = new Instruction("ADD A [hl]", 0x86, 1, (value) => ADD(mmu.ReadByte(registers.HL)));
      instructions[0x87] = new Instruction("ADD A a", 0x87, 1, (value) => ADD(registers.A));
      instructions[0x88] = new Instruction("ADC A b", 0x88, 1, (value) => ADC(registers.B));
      instructions[0x89] = new Instruction("ADC A c", 0x89, 1, (value) => ADC(registers.C));
      instructions[0x8A] = new Instruction("ADC A d", 0x8A, 1, (value) => ADC(registers.D));
      instructions[0x8B] = new Instruction("ADC A e", 0x8B, 1, (value) => ADC(registers.E));
      instructions[0x8C] = new Instruction("ADC A h", 0x8C, 1, (value) => ADC(registers.H));
      instructions[0x8D] = new Instruction("ADC A l", 0x8D, 1, (value) => ADC(registers.L));
      instructions[0x8E] = new Instruction("ADC A [hl]", 0x8E, 1, (value) => ADC(mmu.ReadByte(registers.HL)));
      instructions[0x8F] = new Instruction("ADC A a", 0x8F, 1, (value) => ADC(registers.A));

      instructions[0x90] = new Instruction("SUB A b", 0x90, 1, (value) => SUB(registers.B));
      instructions[0x91] = new Instruction("SUB A c", 0x91, 1, (value) => SUB(registers.C));
      instructions[0x92] = new Instruction("SUB A d", 0x92, 1, (value) => SUB(registers.D));
      instructions[0x93] = new Instruction("SUB A e", 0x93, 1, (value) => SUB(registers.E));
      instructions[0x94] = new Instruction("SUB A h", 0x94, 1, (value) => SUB(registers.H));
      instructions[0x95] = new Instruction("SUB A l", 0x95, 1, (value) => SUB(registers.L));
      instructions[0x96] = new Instruction("SUB A [hl]", 0x96, 1, (value) => SUB(mmu.ReadByte(registers.HL)));
      instructions[0x97] = new Instruction("SUB A a", 0x97, 1, (value) => SUB(registers.A));
      instructions[0x98] = new Instruction("SBC A b", 0x98, 1, (value) => SBC(registers.B));
      instructions[0x99] = new Instruction("SBC A c", 0x99, 1, (value) => SBC(registers.C));
      instructions[0x9A] = new Instruction("SBC A d", 0x9A, 1, (value) => SBC(registers.D));
      instructions[0x9B] = new Instruction("SBC A e", 0x9B, 1, (value) => SBC(registers.E));
      instructions[0x9C] = new Instruction("SBC A h", 0x9C, 1, (value) => SBC(registers.H));
      instructions[0x9D] = new Instruction("SBC A l", 0x9D, 1, (value) => SBC(registers.L));
      instructions[0x9E] = new Instruction("SBC A [hl]", 0x9E, 1, (value) => SBC(mmu.ReadByte(registers.HL)));
      instructions[0x9F] = new Instruction("SBC A a", 0x9F, 1, (value) => SBC(registers.A));

      instructions[0xA8] = new Instruction("XOR B", 0xA8, 1, (value) => XOR(registers.B));
      instructions[0xA9] = new Instruction("XOR C", 0xA9, 1, (value) => XOR(registers.C));
      instructions[0xAA] = new Instruction("XOR D", 0xAA, 1, (value) => XOR(registers.D));
      instructions[0xAB] = new Instruction("XOR E", 0xAB, 1, (value) => XOR(registers.E));
      instructions[0xAC] = new Instruction("XOR H", 0xAC, 1, (value) => XOR(registers.H));
      instructions[0xAD] = new Instruction("XOR L", 0xAD, 1, (value) => XOR(registers.L));
      instructions[0xAE] = new Instruction("XOR [hl]", 0xAE, 1, (value) => XOR(mmu.ReadByte(registers.HL)));
      instructions[0xAF] = new Instruction("XOR A", 0xAF, 1, (value) => XOR(registers.A));

      instructions[0xB8] = new Instruction("CP B", 0xB8, 1, (value) => CP(registers.B));
      instructions[0xB9] = new Instruction("CP C", 0xB9, 1, (value) => CP(registers.C));
      instructions[0xBA] = new Instruction("CP D", 0xBA, 1, (value) => CP(registers.D));
      instructions[0xBB] = new Instruction("CP E", 0xBB, 1, (value) => CP(registers.E));
      instructions[0xBC] = new Instruction("CP H", 0xBC, 1, (value) => CP(registers.H));
      instructions[0xBD] = new Instruction("CP L", 0xBD, 1, (value) => CP(registers.L));
      instructions[0xBE] = new Instruction("CP [HL]", 0xBE, 1, (value) => CP(mmu.ReadByte(registers.HL)));
      instructions[0xBF] = new Instruction("CP A", 0xBF, 1, (value) => CP(registers.A));

      instructions[0xC2] = new Instruction("JP NZ, a16", 0xC2, 3, (value) => JP(value, () => !registers.GetFlag(Flag.ZERO)));
      instructions[0xC3] = new Instruction("JP, a16", 0xC3, 3, (value) => JP(value));
      instructions[0xCA] = new Instruction("JP NZ, a16", 0xCA, 3, (value) => JP(value, () => registers.GetFlag(Flag.ZERO)));

      instructions[0xD2] = new Instruction("JP NC, a16", 0xD2, 3, (value) => JP(value, () => !registers.GetFlag(Flag.CARRY)));
      instructions[0xDA] = new Instruction("JP NC, a16", 0xDA, 3, (value) => JP(value, () => registers.GetFlag(Flag.CARRY)));

      instructions[0xE0] = new Instruction("LD [a8], A", 0xE0, 2, null);

      instructions[0xF3] = new Instruction("DI", 0xF3, 1, (value) => DI());
      instructions[0xFB] = new Instruction("EI", 0xFB, 1, (value) => EI());
      instructions[0xFF] = new Instruction("RST 7", 0xFF, 1, null);
    }

    public byte GetInstructionLength(byte opcode)
    {
      Instruction instruction = instructions[opcode];

      if (instruction != null)
      {
        return instructions[opcode].Length;
      }
      else
      {
        return 0;
      }
    }

    public void RunInstruction(byte opcode)
    {
      Instruction instruction = instructions[opcode];

      if (instruction == null) throw new UnknownOpcodeException(opcode);
      else if (instruction.Handler == null) throw new UnknownOpcodeException(instruction);

      Console.WriteLine("Executing opcode {0:X} ({1})", opcode, instruction.Name);

      instruction.Handler.Invoke(0);
    }

    public void RunInstruction(byte opcode, ushort argument)
    {
      Instruction instruction = instructions[opcode];

      if (instruction == null) throw new UnknownOpcodeException(opcode);
      else if (instruction.Handler == null) throw new UnknownOpcodeException(instruction);

      Console.WriteLine("Executing opcode {0:X} ({1})", opcode, instruction.Name);

      instruction.Handler.Invoke(argument);
    }

    #region 8-bit Arithmetic and Logic Instructions

    /// Add a specified byte value to register A
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNHC
    private void ADD(byte value)
    {
      byte oldValue = registers.A;
      int result = registers.A + value;

      registers.A = (byte)result;

      SetFlags(oldValue, value, false);
    }

    /// Add a specified byte value to register A plus the carry flag
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNHC
    private void ADC(byte value)
    {
      byte oldValue = registers.A;
      int carry = registers.GetFlag(Flag.CARRY) ? 1 : 0;
      int result = registers.A + value + carry;

      registers.A = (byte)result;

      SetFlags(oldValue, (byte)(value + carry), false);
    }

    /// Increase the value in a specified register by 1
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNH
    private void INC<T>(T target, Expression<Func<T, byte>> outExpr)
    {
      var expr = (MemberExpression)outExpr.Body;
      var prop = (PropertyInfo)expr.Member;

      byte oldValue = (byte)prop.GetValue(target);
      byte newValue = (byte)(oldValue + 1);

      prop.SetValue(target, newValue, null);

      SetFlags(oldValue, 1, false);
    }

    /// Increase the value in a specified register pair by 1
    /// Cycles: 1, Bytes: 2
    /// Affected flags: none
    private void INC<T>(T target, Expression<Func<T, ushort>> outExpr)
    {
      var expr = (MemberExpression)outExpr.Body;
      var prop = (PropertyInfo)expr.Member;

      ushort oldValue = (ushort)prop.GetValue(target);
      ushort newValue = (ushort)(oldValue + 1);

      prop.SetValue(target, newValue, null);

      SetFlags(oldValue, 1, false);
    }

    /// Decrease the value in a specified register by 1
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNH
    private void DEC<T>(T target, Expression<Func<T, byte>> outExpr)
    {
      var expr = (MemberExpression)outExpr.Body;
      var prop = (PropertyInfo)expr.Member;

      byte oldValue = (byte)prop.GetValue(target);
      byte newValue = (byte)(oldValue - 1);

      prop.SetValue(target, newValue, null);

      SetFlags(oldValue, 1, true);
    }

    /// Decrease the value in a specified register by 1
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNH
    private void DEC<T>(T target, Expression<Func<T, ushort>> outExpr)
    {
      var expr = (MemberExpression)outExpr.Body;
      var prop = (PropertyInfo)expr.Member;

      ushort oldValue = (ushort)prop.GetValue(target);
      ushort newValue = (ushort)(oldValue - 1);

      prop.SetValue(target, newValue, null);

      SetFlags(oldValue, 1, true);
    }

    /// Substract a specified byte value from register A
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNHC
    private void SUB(byte value)
    {
      byte oldValue = registers.A;
      int result = registers.A - value;

      registers.A = (byte)result;

      SetFlags(oldValue, value, true);
    }

    /// Subtract a specified value and the carry flag from A
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZHNC
    private void SBC(byte value)
    {
      byte oldValue = registers.A;
      int carry = registers.GetFlag(Flag.CARRY) ? 1 : 0;
      int result = registers.A - value - carry;

      registers.A = (byte)result;

      SetFlags(oldValue, (byte)(value + carry), true);
    }


    /// Bitwise XOR between the value in r8 and A
    /// Cycles: 1, Bytes: 1
    /// Affected flags: Z
    private void XOR(byte value)
    {
      byte result = (byte)(registers.A | value);
      registers.A = result;

      if (result == 0)
      {
        registers.SetFlag(Flag.SUBSTRACTION);
      }
    }

    #endregion

    #region Jumps and Subroutines

    /// Jump to a specified memory address; effectively, store a specified address into PC.
    /// Cycles: 4, Bytes: 3
    /// Affected flags: none
    private void JP(ushort address)
    {
      registers.PC = address;
    }

    /// Jump to a specified memory address; effectively, store a specified address into PC given that a specified condition is met
    /// Cycles: 4 taken / 3 untaken, Bytes: 3
    /// Affected flags: none
    private void JP(ushort address, Func<bool> condition)
    {
      if (condition.Invoke())
      {
        JP(address);
      }
    }

    /// Relative Jump by adding e8 to the address of the instruction following the JR. To clarify, an operand of 0 is equivalent to no jumping.
    /// Cycles: 3, Bytes: 2
    /// Affected flags: none
    private void JR(sbyte value)
    {
      registers.PC += (ushort)(value);
    }

    /// Relative Jump by adding e8 to the current address if condition cc is met
    /// Cycles 3 taken / 2 untaken
    /// Affected flags: none
    private void JR(sbyte value, Func<bool> condition)
    {
      if (condition.Invoke())
      {
        JR(value);
      }
    }

    #endregion

    #region Miscellaneous Instructions

    /// Disable Interrupts by clearing the IME flag.
    /// Cycles: 1, Bytes: 1
    /// Affected flags: none
    private void DI()
    {
      registers.IME = false;
    }

    /// Enable Interrupts by clearing the IME flag.
    /// Cycles: 1, Bytes: 1
    /// Affected flags: none
    private void EI()
    {
      registers.IME = true;
    }

    /// No OPeration
    /// Cycles: 1, Bytes: 1
    /// Affected flags: none
    private void NOP() { }

    /// Subtract a specified value from A and set flags accordingly, but don't store the result. This is useful for ComParing values
    /// Cycles: 1, Bytes: 1
    /// Affected flags: ZNHC
    private void CP(byte value)
    {
      byte oldValue = registers.A;
      int result = registers.A - value;

      SetFlags(oldValue, value, true);
    }

    /// ComPLement accumulator (A = ~A)
    /// Cycles: 1, Bytes: 1
    /// Affected flags: NH
    private void CPL()
    {
      var complement = (byte)~registers.A;
      registers.A = complement;

      registers.SetFlag(Flag.SUBSTRACTION);
      registers.SetFlag(Flag.HALF_CARRY);
    }

    /// Complement Carry Flag
    /// Cycles: 1, Bytes: 1
    /// Affected flags: NHC
    private void CCF()
    {
      registers.FlipFlag(Flag.CARRY);
      registers.ClearFlag(Flag.SUBSTRACTION);
      registers.ClearFlag(Flag.HALF_CARRY);
    }

    /// Set Carry Flag
    /// Cycles: 1, Bytes: 1
    /// Affected flags: NHC
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

        CheckAndSetCarryFlagForSubstraction(a, b);
        CheckAndSetHalfCarryFlagForSubstraction(a, b);

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

    private void SetFlags(ushort a, ushort b, bool operationWasSubstraction)
    {
      if (operationWasSubstraction)
      {
        registers.SetFlag(Flag.SUBSTRACTION);

        CheckAndSetCarryFlagForSubstraction(a, b);
        CheckAndSetHalfCarryFlagForSubstraction(a, b);

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

        ushort val = (ushort)(a + b);
        if ((ushort)(a + b) == 0)
        {
          registers.SetFlag(Flag.ZERO);
        }
        else
        {
          registers.ClearFlag(Flag.ZERO);
        }
      }
    }

    private void CheckAndSetCarryFlagForAddition(ushort a, ushort b)
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

    private void CheckAndSetCarryFlagForSubstraction(ushort a, ushort b)
    {
      if (a < b)
      {
        registers.SetFlag(Flag.CARRY);
      }
      else
      {
        registers.ClearFlag(Flag.CARRY);
      }
    }

    private void CheckAndSetHalfCarryFlagForAddition(ushort a, ushort b)
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

    private void CheckAndSetHalfCarryFlagForSubstraction(ushort a, ushort b)
    {
      if ((((a & 0xF) - (b & 0xF)) & 0x10) == 0x10)
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