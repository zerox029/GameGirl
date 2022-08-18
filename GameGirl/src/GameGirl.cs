using System;
using System.IO;

namespace GameGirl
{
  class GameGirl
  {
    CPU cpu;
    MMU mmu;

    public void Boot()
    {
      Init();
      LoadRom("roms/tetris.gb");

      cpu.RunThroughRomOpcodes();
    }

    private void Init()
    {
      mmu = new MMU();
      cpu = new CPU(mmu);
    }

    private void LoadRom(string filePath)
    {
      byte[] byteArray = File.ReadAllBytes(filePath);
      ushort currentPosition = 0;

      foreach (byte b in byteArray)
      {
        mmu.WriteByte(currentPosition, b);
        currentPosition++;
      }
    }
  }
}