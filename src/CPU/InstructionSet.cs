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

      //8-bit arithmetic/logic instructions
      instructions[0x80] = new Instruction("add A a", 0x80, (value) => Add(value));
    }

    #region 8-bit Arithmetic and Logic instruction

    public void Add_A_a()
    {
      Add(registers.A);
    }

    public void Add_A_b()
    {
      Add(registers.B);
    }

    public void Add_A_c()
    {
      Add(registers.C);
    }

    public void Add_A_d()
    {
      Add(registers.D);
    }

    public void Add_A_e()
    {
      Add(registers.E);
    }

    public void Add_A_f()
    {
      Add(registers.F);
    }

    public void Add_A_l()
    {
      Add(registers.L);
    }

    public void Add_A_h()
    {
      Add(registers.H);
    }

    public void Add_A_hl()
    {
      Add(registers.H);
    }

    #endregion

    // Used by opcodes 0x80 - 0x87
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
    }

    private void Adc(byte value)
    {
      int carry = registers.GetFlag(Flag.CARRY) ? 1 : 0;
      int result = registers.A + value + carry;


    }
  }
}