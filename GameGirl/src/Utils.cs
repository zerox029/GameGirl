namespace GameGirl
{
  public static class Utils
  {
    public static ushort MEMORY_BANK_SIZE { get { return 0xFFFF; } }
    public static ushort ROM_BANK_SIZE { get { return 0x8000; } }
    public static ushort VIDEO_RAM_SIZE { get { return 0x2000; } }
    public static ushort EXTERNAL_RAM_SIZE { get { return 0x2000; } }
    public static ushort WORK_RAM_SIZE { get { return 0x2000; } }
    public static ushort SPRITE_ATTRIBUTION_TABLE_SIZE { get { return 0x100; } }
    public static ushort IO_SIZE { get { return 0x80; } }
    public static ushort HIGH_RAM_SIZE { get { return 0x80; } }

    public static double CLOCK_SPEED { get { return 2.194304f; } }
    public static ushort START_ADDRESS { get { return 0x0100; } }
  }
}