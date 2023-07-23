using System;
using System.IO;
using Exceptions;

namespace GameGirl
{
  class GameGirl
  {
    CPU cpu;
    MMU mmu;

    public void Boot()
    {
      Init("roms/tetris.gb");

      cpu.EmulationLoop();
    }

    private void Init(String romFilePath)
    {
      mmu = new MMU(LoadRom(romFilePath));
      cpu = new CPU(mmu);

      cpu.BootUpSequence();
    }

    private byte[] LoadRom(String filePath)
    {
      byte[] rom = File.ReadAllBytes(filePath);

      if (rom.Length != 0x8000)
      {
        throw new InvalidRomException(rom.Length);
      }

      return rom;
    }
  }
}