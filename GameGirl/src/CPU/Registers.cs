namespace GameGirl
{
  public class Registers
  {
    public byte A { get; set; }
    public byte B { get; set; }
    public byte C { get; set; }
    public byte D { get; set; }
    public byte E { get; set; }
    public byte F { get; set; }
    public byte H { get; set; }
    public byte L { get; set; }

    public ushort SP; //Stack Pointer
    public ushort PC; // Program Counter

    public ushort AF
    {
      get
      {
        ushort a = (ushort)(A << 8);
        ushort f = (ushort)(F);

        return (ushort)(a | f);
      }

      set
      {
        A = (byte)(value >> 8);
        B = (byte)(value);
      }
    }

    public ushort BC
    {
      get
      {
        ushort b = (ushort)(B << 8);
        ushort c = (ushort)(C);

        return (ushort)(b | c);
      }

      set
      {
        B = (byte)(value >> 8);
        C = (byte)(value);
      }
    }

    public ushort DE
    {
      get
      {
        ushort d = (ushort)(D << 8);
        ushort e = (ushort)(E);

        return (ushort)(d | e);
      }

      set
      {
        D = (byte)(value >> 8);
        E = (byte)(value);
      }
    }

    public ushort HL
    {
      get
      {
        ushort h = (ushort)(H << 8);
        ushort l = (ushort)(L);

        return (ushort)(h | l);
      }

      set
      {
        H = (byte)(value >> 8);
        L = (byte)(value);
      }
    }

    public bool GetFlag(Flag flag)
    {
      return (F & (byte)flag) != 0;
    }

    public void SetFlag(Flag flag)
    {
      F |= (byte)flag;
    }

    public void ClearFlag(Flag flag)
    {
      F &= (byte)~flag;
    }

    public void FlipFlag(Flag flag)
    {
      F ^= (byte)flag;
    }

    public void ClearAllFlags()
    {
      F = 0;
    }
  }
}