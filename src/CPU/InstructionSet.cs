using Exceptions;

namespace GameGirl
{
  public class InstructionSet
  {
    private Instruction[] instructions;
    private Registers registers;

    public InstructionSet(Registers registers)
    {
      this.registers = registers;

      GenerateInstructionSet();
    }

    private void GenerateInstructionSet()
    {
      instructions = new Instruction[256];

      instructions[0x80] = new Instruction("add A b", 0x80, () => Add(registers.B));
      instructions[0x81] = new Instruction("add A c", 0x81, () => Add(registers.C));
      instructions[0x82] = new Instruction("add A d", 0x82, () => Add(registers.D));
      instructions[0x83] = new Instruction("add A e", 0x83, () => Add(registers.E));
      instructions[0x84] = new Instruction("add A h", 0x84, () => Add(registers.H));
      instructions[0x85] = new Instruction("add A l", 0x85, () => Add(registers.L));
      instructions[0x86] = new Instruction("add A [hl]", 0x86, () => Add(registers.HL));
      instructions[0x87] = new Instruction("add A a", 0x87, () => Add(registers.A));
      instructions[0x88] = new Instruction("adc A b", 0x88, () => Adc(registers.B));
      instructions[0x89] = new Instruction("adc A c", 0x89, () => Adc(registers.C));
      instructions[0x8A] = new Instruction("adc A d", 0x8A, () => Adc(registers.D));
      instructions[0x8B] = new Instruction("adc A e", 0x8B, () => Adc(registers.E));
      instructions[0x8C] = new Instruction("adc A h", 0x8C, () => Adc(registers.H));
      instructions[0x8D] = new Instruction("adc A l", 0x8D, () => Adc(registers.L));
      instructions[0x8E] = new Instruction("adc A [hl]", 0x8E, () => Adc(registers.HL));
      instructions[0x8F] = new Instruction("adc A a", 0x8F, () => Adc(registers.A));

      instructions[0x90] = new Instruction("sub A b", 0x90, () => Sub(registers.B));
      instructions[0x91] = new Instruction("sub A c", 0x91, () => Sub(registers.C));
      instructions[0x92] = new Instruction("sub A d", 0x92, () => Sub(registers.D));
      instructions[0x93] = new Instruction("sub A e", 0x93, () => Sub(registers.E));
      instructions[0x94] = new Instruction("sub A h", 0x94, () => Sub(registers.H));
      instructions[0x95] = new Instruction("sub A l", 0x95, () => Sub(registers.L));
      instructions[0x96] = new Instruction("sub A [hl]", 0x96, () => Sub(registers.HL));
      instructions[0x97] = new Instruction("sub A a", 0x97, () => Sub(registers.A));
    }

    public void RunInstruction(byte opcode)
    {
      Instruction instruction = instructions[opcode];

      if (instruction == null) throw new UnknownOpcodeException(opcode);
      else if (instruction.Handler == null) throw new UnknownOpcodeException(instruction);

      instruction.Handler.Invoke();
    }

    // Used by opcodes 0x80 to 0x85 and 0x87
    private void Add(byte value)
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
    private void Add(ushort value)
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
    private void Adc(byte value)
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
    private void Adc(ushort value)
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
    private void Sub(byte value)
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
    private void Sub(ushort value)
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
  }
}