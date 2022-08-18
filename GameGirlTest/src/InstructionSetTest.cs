using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameGirl;

namespace GameGirlTest
{
  [TestClass]
  public class InstructionSetTest
  {
    private Registers registers;
    private MMU mmu;

    private InstructionSet instructionSet;

    [TestMethod]
    public void TestCPL()
    {
      //GIVEN
      registers = new Registers();
      registers.A = 0b00000000;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x2F);

      //THEN
      Assert.AreEqual(0b11111111, registers.A);
    }
  }
}
