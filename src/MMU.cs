namespace GameGirl
{
  //Temporary simple replacement for the actual mmu
  class MMU
  {
    private byte[] memoryBus;

    public MMU()
    {
      memoryBus = new byte[Utils.MemorySize];
    }

    public void WriteByte(ushort address, byte value)
    {
      if (address < 0 || address > 0xFFFF) return;

      memoryBus[address] = value;
    }
  }
}