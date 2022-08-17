using System;

namespace GameGirl
{
  class CPU
  {
    Registers registers;
    InstructionSet instructionSet;

    public CPU()
    {
      this.registers = new Registers();
      this.instructionSet = new InstructionSet(registers);
    }

    public void decodeAndRunOpdcode()
    {
      switch (getCurrentOpcode())
      {
        case 0x80:
          instructionSet.Add_A_b();
          break;
        case 0x81:
          instructionSet.Add_A_c();
          break;
        case 0x82:
          instructionSet.Add_A_d();
          break;
        case 0x83:
          instructionSet.Add_A_e();
          break;
        case 0x84:
          instructionSet.Add_A_h();
          break;
        case 0x85:
          instructionSet.Add_A_l();
          break;

        default:
          break;
      }
    }

    private byte getCurrentOpcode()
    {
      return 0x80;
    }
  }
}