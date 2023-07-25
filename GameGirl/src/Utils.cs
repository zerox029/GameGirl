namespace GameGirl
{
  public static class Utils
  {
    //Memory dimensions
    public static ushort MEMORY_BANK_SIZE { get { return 0xFFFF; } }
    public static ushort ROM_BANK_SIZE { get { return 0x8000; } }
    public static ushort VIDEO_RAM_SIZE { get { return 0x2000; } }
    public static ushort EXTERNAL_RAM_SIZE { get { return 0x2000; } }
    public static ushort WORK_RAM_SIZE { get { return 0x2000; } }
    public static ushort SPRITE_ATTRIBUTION_TABLE_SIZE { get { return 0x100; } }
    public static ushort IO_SIZE { get { return 0x80; } }
    public static ushort HIGH_RAM_SIZE { get { return 0x80; } }

    //Timer values
    public static ushort DIVIDER_REGISTER_ADDRESS { get { return 0xFF04; } }
    public static ushort TIMER_ADDRESS { get { return 0xFF05; } }
    public static ushort TIMER_MODULATOR_ADDRESS { get { return 0xFF06; } }
    public static ushort TIMER_CONTROLLER_ADDRESS { get { return 0xFF07; } }
    public static int CLOCK_FREQUENCY_STATE_00 { get { return 4096; } }
    public static int CLOCK_FREQUENCY_STATE_01 { get { return 262144; } }
    public static int CLOCK_FREQUENCY_STATE_10 { get { return 65536; } }
    public static int CLOCK_FREQUENCY_STATE_11 { get { return 16384; } }
    public static int CLOCK_SPEED { get { return 4194304; } }

    //Interrupt values
    public static ushort INTERRUPT_REQUEST_REGISTER_ADDRESS { get { return 0xFF0F; } }
    public static ushort INTERRUPT_ENABLED_REGISTER_ADDRESS { get { return 0xFFFF; } }
  }
}