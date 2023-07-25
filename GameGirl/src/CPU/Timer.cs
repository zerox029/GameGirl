namespace GameGirl
{
  public class Timer
  {
    private int timerCounter;
    private int dividerCounter;
    private MMU mmu;

    public Timer(MMU mmu)
    {
      timerCounter = ValueUtils.CLOCK_SPEED / ValueUtils.CLOCK_FREQUENCY_STATE_00;
      this.mmu = mmu;
    }

    public void Update(int cycles)
    {
      DoDivideRegister(cycles);

      if (IsClockEnabled())
      {
        timerCounter -= cycles;

        if (timerCounter <= 0)
        {
          SetClockFrequency();

          if (mmu.ReadByte(ValueUtils.TIMER_ADDRESS) == 255)
          {
            mmu.WriteByte(ValueUtils.TIMER_ADDRESS, mmu.ReadByte(ValueUtils.TIMER_MODULATOR_ADDRESS));
            //Request Timer Interupt
          }
          else
          {
            mmu.WriteByte(ValueUtils.TIMER_ADDRESS, (byte)(mmu.ReadByte(ValueUtils.TIMER_ADDRESS) + 1));
          }
        }
      }
    }

    private void DoDivideRegister(int cycles)
    {
      dividerCounter += cycles;
      if (dividerCounter >= 255)
      {
        dividerCounter = 0;
        mmu.IncrementByte(ValueUtils.DIVIDER_REGISTER_ADDRESS, 1);
      }
    }

    private bool IsClockEnabled()
    {
      return (mmu.ReadByte(ValueUtils.TIMER_CONTROLLER_ADDRESS) & 0b001) == 1;
    }

    private byte getClockFrequency()
    {
      return (byte)(mmu.ReadByte(ValueUtils.TIMER_CONTROLLER_ADDRESS) & 0x3);
    }

    private void SetClockFrequency()
    {
      switch (getClockFrequency())
      {
        case 0x00:
          timerCounter = ValueUtils.CLOCK_SPEED / ValueUtils.CLOCK_FREQUENCY_STATE_00;
          break;
        case 0x01:
          timerCounter = ValueUtils.CLOCK_SPEED / ValueUtils.CLOCK_FREQUENCY_STATE_01;
          break;
        case 0x10:
          timerCounter = ValueUtils.CLOCK_SPEED / ValueUtils.CLOCK_FREQUENCY_STATE_10;
          break;
        case 0x11:
          timerCounter = ValueUtils.CLOCK_SPEED / ValueUtils.CLOCK_FREQUENCY_STATE_11;
          break;
      }
    }
  }
}