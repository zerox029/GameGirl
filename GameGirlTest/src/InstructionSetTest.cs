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
    public void TestInstruction_INC_NoOverflow()
    {
      //Given; register B set to 0x00
      registers = new Registers();
      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction INC B is run
      instructionSet.RunInstruction(0x04);

      //Then; register B set to 1; flags Z, N and H set to 0
      Assert.AreEqual(1, registers.B);
      Assert.IsFalse(registers.GetFlag(Flag.ZERO));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
    }

    [TestMethod]
    public void TestInstruction_INC_WithOverflow()
    {
      //Given; register B set to 0xFF
      registers = new Registers();
      registers.B = 0xFF;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction INC B is run
      instructionSet.RunInstruction(0x04);

      //Then; register B set to 0; flags Z and H set to 1 and N set to 0
      Assert.AreEqual(0, registers.B);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsTrue(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_0x2F()
    {
      //GIVEN
      registers = new Registers();
      registers.A = 0b00001111;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x2F);

      //THEN
      Assert.AreEqual(0b11110000, registers.A);
      Assert.IsTrue(registers.GetFlag(Flag.SUBSTRACTION));
      Assert.IsTrue(registers.GetFlag(Flag.HALF_CARRY));
    }

    [TestMethod]
    public void TestInstruction_0x37()
    {
      //GIVEN
      registers = new Registers();
      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x37);

      //THEN
      Assert.IsTrue(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
    }

    [TestMethod]
    public void TestInstruction_0x3F()
    {
      //GIVEN
      registers = new Registers();
      registers.SetFlag(Flag.CARRY);

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x3F);

      //THEN
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
    }

    [TestMethod]
    public void TestInstruction_0x40()
    {
      //GIVEN
      registers = new Registers();
      registers.B = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x40);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x41()
    {
      //GIVEN
      registers = new Registers();
      registers.C = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x41);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x42()
    {
      //GIVEN
      registers = new Registers();
      registers.D = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x42);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x43()
    {
      //GIVEN
      registers = new Registers();
      registers.E = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x43);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x44()
    {
      //GIVEN
      registers = new Registers();
      registers.H = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x44);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x45()
    {
      //GIVEN
      registers = new Registers();
      registers.L = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x45);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x46()
    {
      //GIVEN
      registers = new Registers();
      registers.HL = 0x2F78;

      mmu = new MMU();
      mmu.WriteByte(0x2F78, 20);

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x46);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }

    [TestMethod]
    public void TestInstruction_0x47()
    {
      //GIVEN
      registers = new Registers();
      registers.A = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //WHEN
      instructionSet.RunInstruction(0x47);

      //THEN
      Assert.AreEqual(registers.B, 20);
    }
  }
}
