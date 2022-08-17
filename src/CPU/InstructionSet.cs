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

    #region 0x8X Opcodes

    //0x80
    public void Add_A_b()
    {
      Add(registers.B);
    }

    //0x81
    public void Add_A_c()
    {
      Add(registers.C);
    }

    //0x82
    public void Add_A_d()
    {
      Add(registers.D);
    }

    //0x83
    public void Add_A_e()
    {
      Add(registers.E);
    }

    //0x84
    public void Add_A_h()
    {
      Add(registers.H);
    }

    //0x85
    public void Add_A_l()
    {
      Add(registers.L);
    }

    //0x86
    public void Add_A_hl()
    {
      Add(registers.HL);
    }

    //0x87
    public void Add_A_a()
    {
      Add(registers.A);
    }

    //0x88
    public void Adc_A_b()
    {
      Adc(registers.B);
    }

    //0x89
    public void Adc_A_c()
    {
      Adc(registers.C);
    }

    //0x8A
    public void Adc_A_d()
    {
      Adc(registers.D);
    }

    //0x8B
    public void Adc_A_e()
    {
      Adc(registers.E);
    }

    //0x8C
    public void Adc_A_h()
    {
      Adc(registers.H);
    }

    //0x8D
    public void Adc_A_l()
    {
      Adc(registers.L);
    }

    //0x8E
    public void Adc_A_hl()
    {
      Adc(registers.HL);
    }

    //0x8F
    public void Adc_A_a()
    {
      Adc(registers.A);
    }

    #endregion

    #region 0x9X opcodes

    //0x90
    public void Sub_A_b()
    {
      Sub(registers.B);
    }

    //0x91
    public void Sub_A_c()
    {
      Sub(registers.C);
    }

    //0x92
    public void Sub_A_d()
    {
      Sub(registers.D);
    }

    //0x93
    public void Sub_A_e()
    {
      Sub(registers.E);
    }

    //0x94
    public void Sub_A_h()
    {
      Sub(registers.H);
    }

    //0x95
    public void Sub_A_l()
    {
      Sub(registers.L);
    }

    //0x96
    public void Sub_A_hl()
    {
      Sub(registers.HL);
    }

    //0x97
    public void Sub_A_a()
    {
      Sub(registers.A);
    }

    #endregion

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