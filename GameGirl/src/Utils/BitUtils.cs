namespace GameGirl
{
  public class BitUtils
  {
    public static bool IsBitSet(int value, int position)
    {
      return (value & (1 << position)) != 0;
    }

    public static int ResetBit(int value, int position)
    {
      return value & ~(1 << position);
    }
  }
}