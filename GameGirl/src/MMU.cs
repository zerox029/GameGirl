using Exceptions;
using static GameGirl.Utils;
namespace GameGirl
{
  /*
    Memory map
    0000	3FFF  =>	16 KiB ROM bank 00	From cartridge, usually a fixed bank
    4000	7FFF  =>	16 KiB ROM Bank 01~NN	From cartridge, switchable bank via mapper (if any)
    8000	9FFF  =>	8 KiB Video RAM (VRAM)	In CGB mode, switchable bank 0/1
    A000	BFFF  =>	8 KiB External RAM	From cartridge, switchable bank if any
    C000	CFFF  =>	4 KiB Work RAM (WRAM)	
    D000	DFFF  =>	4 KiB Work RAM (WRAM)	In CGB mode, switchable bank 1~7
    E000	FDFF  =>	Mirror of C000~DDFF (ECHO RAM)	Nintendo says use of this area is prohibited.
    FE00	FE9F  =>	Sprite attribute table (OAM)	
    FEA0	FEFF  =>	Not Usable	Nintendo says use of this area is prohibited
    FF00	FF7F  =>	I/O Registers	
    FF80	FFFE  =>	High RAM (HRAM)	
    FFFF	FFFF  =>	Interrupt Enable register (IE)
  */
  public class MMU
  {
    private byte[] romBank { get; set; }

    public byte[] videoRAM { get; set; }
    public byte[] externalRAM { get; set; }
    public byte[] workRAM { get; set; }
    public byte[] echoRAM { get; set; }

    public byte[] spriteAttributeTable { get; set; }
    public byte[] IO { get; set; }
    public byte[] highRAM { get; set; }
    public byte interruptEnableRegister { get; set; }


    public MMU()
    {
      romBank = new byte[ROM_BANK_SIZE];
      videoRAM = new byte[VIDEO_RAM_SIZE];
      externalRAM = new byte[EXTERNAL_RAM_SIZE];

      workRAM = new byte[WORK_RAM_SIZE];
      echoRAM = new byte[WORK_RAM_SIZE];

      spriteAttributeTable = new byte[SPRITE_ATTRIBUTION_TABLE_SIZE];

      IO = new byte[IO_SIZE];

      highRAM = new byte[HIGH_RAM_SIZE];
    }

    public MMU(byte[] rom)
    {
      romBank = rom;
      videoRAM = new byte[VIDEO_RAM_SIZE];
      externalRAM = new byte[EXTERNAL_RAM_SIZE];

      workRAM = new byte[WORK_RAM_SIZE];
      echoRAM = new byte[WORK_RAM_SIZE];

      spriteAttributeTable = new byte[SPRITE_ATTRIBUTION_TABLE_SIZE];

      IO = new byte[IO_SIZE];

      highRAM = new byte[HIGH_RAM_SIZE];
    }

    public void WriteByte(ushort address, byte value)
    {
      if (address >= 0x0 && address <= 0x7FFF)
      {
        romBank[address] = value;
      }
      else if (address >= 0x8000 && address <= 0x9FFF)
      {
        videoRAM[address - 0x8000] = value;
      }
      else if (address >= 0xA000 && address <= 0xBFFF)
      {
        externalRAM[address - 0xA000] = value;
      }
      else if (address >= 0xC000 && address <= 0xDFFF)
      {
        workRAM[address - 0xC000] = value;
        echoRAM[address + WORK_RAM_SIZE - 0xE000] = value;
      }
      else if (address >= 0xFE00 && address <= 0xFE9F)
      {
        spriteAttributeTable[address - 0xFE00] = value;
      }
      else if (address >= 0xFEA0 && address <= 0xFEFF)
      {
        //throw new InvalidAddressException(address);
      }
      else if (address >= 0xFF00 && address <= 0xFF7F)
      {
        IO[address - 0xFF00] = value;
      }
      else if (address >= 0xFF80 && address <= 0xFFFE)
      {
        highRAM[address - 0xFF80] = value;
      }
      else if (address >= 0xFFFF)
      {
        interruptEnableRegister = value;
      }
      else
      {
        throw new InvalidAddressException(address);
      }
    }

    public byte ReadByte(ushort address)
    {
      if (address >= 0x0 && address <= 0x7FFF)
      {
        return romBank[address];
      }
      else if (address >= 0x8000 && address <= 0x9FFF)
      {
        return videoRAM[address - 0x8000];
      }
      else if (address >= 0xA000 && address <= 0xBFFF)
      {
        return externalRAM[address - 0xA000];
      }
      else if (address >= 0xC000 && address <= 0xDFFF)
      {
        return workRAM[address - 0xC000];
      }
      else if (address >= 0xE000 && address <= 0xFDFF)
      {
        return echoRAM[address - 0xE000];
      }
      else if (address >= 0xFE00 && address <= 0xFE9F)
      {
        return spriteAttributeTable[address - 0xFE00];
      }
      else if (address >= 0xFEA0 && address <= 0xFEFF)
      {
        throw new InvalidAddressException(address);
      }
      else if (address >= 0xFF00 && address <= 0xFF7F)
      {
        return IO[address - 0xFF00];
      }
      else if (address >= 0xFF80 && address <= 0xFFFE)
      {
        return highRAM[address - 0xFF80];
      }
      else if (address >= 0xFFFF)
      {
        return interruptEnableRegister;
      }
      else
      {
        throw new InvalidAddressException(address);
      }
    }

    public void IncrementByte(ushort address, byte increment)
    {
      byte currentValue = ReadByte(address);
      WriteByte(address, (byte)(currentValue + increment));
    }

    public void DecrementByte(ushort address, byte decrement)
    {
      byte currentValue = ReadByte(address);
      WriteByte(address, (byte)(currentValue - decrement));
    }

    public void CopyByte(byte to, byte from)
    {
      byte copyValue = ReadByte(from);
      WriteByte(to, copyValue);
    }

    public void PushToStack(ushort value, Registers registers)
    {
      byte msb = (byte)((value & 0xFF00) >> 4);
      byte lsb = (byte)(value & 0x00FF);
      WriteByte((ushort)(registers.SP - 1), lsb);
      WriteByte((ushort)(registers.SP - 2), msb);

      registers.SP -= 2;
    }
  }
}