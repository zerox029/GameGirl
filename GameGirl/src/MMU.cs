using System;

namespace GameGirl
{
  //Temporary simple replacement for the actual mmu
  public class MMU
  {
    private byte[] memoryBus;

    public MMU()
    {
      memoryBus = new byte[Utils.MEMORY_SIZE];
    }

    public void WriteByte(ushort address, byte value)
    {
      if (address < 0 || address > 0xFFFF) return;

      memoryBus[address] = value;
    }

    public byte GetByte(ushort address)
    {
      return memoryBus[address];
    }

    public byte[] GetRom()
    {
      byte[] rom = new byte[0x8000];
      Array.Copy(memoryBus, 0, rom, 0, 0x8000);

      return rom;
    }
  }
}