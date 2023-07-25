using System;

namespace GameGirl
{
  public enum Interrupt
  {
    VBLANK = 0x00000001,
    LCD = 0x00000010,
    TIMER = 0x00000100,
    SERIAL = 0x00001000,
    JOYPAD = 0x00010000
  }

  public class InterruptHandler
  {
    private Registers registers;
    private MMU mmu;

    public bool InterrupMasterEnable { get; set; }

    public InterruptHandler(Registers registers, MMU mmu)
    {
      this.registers = registers;
      this.mmu = mmu;
    }

    public void RequestInterrupt(Interrupt interruptType)
    {
      byte interruptRequestRegister = mmu.ReadByte(ValueUtils.INTERRUPT_REQUEST_REGISTER_ADDRESS);
      mmu.WriteByte(ValueUtils.INTERRUPT_REQUEST_REGISTER_ADDRESS, (byte)(interruptRequestRegister | (byte)interruptType));
    }

    public void CheckInterrupts()
    {
      if (InterrupMasterEnable)
      {
        byte interruptRequestRegister = mmu.ReadByte(ValueUtils.INTERRUPT_REQUEST_REGISTER_ADDRESS);
        byte interruptEnabledRegister = mmu.ReadByte(ValueUtils.INTERRUPT_ENABLED_REGISTER_ADDRESS);

        if (interruptEnabledRegister > 0)
        {
          foreach (Interrupt interrupt in Enum.GetValues(typeof(Interrupt)))
          {
            if (BitUtils.IsBitSet(interruptRequestRegister, (byte)interrupt) && BitUtils.IsBitSet(interruptEnabledRegister, (byte)interrupt))
            {
              DoInterrupt(interrupt);
            }
          }
        }
      }
    }

    private void DoInterrupt(Interrupt interrupt)
    {
      InterrupMasterEnable = false;
      byte interruptRequestRegister = mmu.ReadByte(ValueUtils.INTERRUPT_REQUEST_REGISTER_ADDRESS);
      interruptRequestRegister = (byte)BitUtils.ResetBit(interruptRequestRegister, (int)interrupt);
      mmu.WriteByte(ValueUtils.INTERRUPT_REQUEST_REGISTER_ADDRESS, interruptRequestRegister);

      mmu.PushToStack(registers.PC, registers);

      switch (interrupt)
      {
        case Interrupt.VBLANK:
          registers.PC = 0x40;
          break;
        case Interrupt.LCD:
          registers.PC = 0x48;
          break;
        case Interrupt.TIMER:
          registers.PC = 0x50;
          break;
        case Interrupt.SERIAL:
          throw new NotImplementedException("The serial interrupt is currently not supported");
        case Interrupt.JOYPAD:
          registers.PC = 0x60;
          break;
      }
    }
  }
}