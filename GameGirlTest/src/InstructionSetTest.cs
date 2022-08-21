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
    public void TestInstruction_INC_NoHalfCarry()
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
    public void TestInstruction_INC_WithHalfCarry()
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
    public void TestInstruction_DEC_NoHalfCarry()
    {
      //Given; register B set to 0x01
      registers = new Registers();
      registers.B = 0x01;
      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction INC B is run
      instructionSet.RunInstruction(0x05);

      //Then; register B set to 0; ZN flags set
      Assert.AreEqual(0, registers.B);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsTrue(registers.GetFlag(Flag.SUBSTRACTION));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
    }

    [TestMethod]
    public void TestInstruction_ADD_8bit_NoCarry()
    {
      //Given; register A set to 0x00 and register B is set to 10
      registers = new Registers();
      registers.B = 10;
      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction ADD A, B is run
      instructionSet.RunInstruction(0x80);

      //Then; register A set to 10; all flags set to 0
      Assert.AreEqual(10, registers.A);
      Assert.IsFalse(registers.GetFlag(Flag.ZERO));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_ADD_8bit_WithHalfCarry()
    {
      //Given; register A set to 15 and register B is set to 35
      registers = new Registers();
      registers.A = 15;
      registers.B = 20;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction ADD A, B is run
      instructionSet.RunInstruction(0x80);

      //Then; register A set to 35; all flags set to 0 except half carry
      Assert.AreEqual(35, registers.A);
      Assert.IsFalse(registers.GetFlag(Flag.ZERO));
      Assert.IsTrue(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_ADD_8bit_WithCarry()
    {
      //Given; register A set to 255 and register B is set to 1
      registers = new Registers();
      registers.A = 255;
      registers.B = 1;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction ADD A, B is run
      instructionSet.RunInstruction(0x80);

      //Then; register A set to 0; Zero and half carry flags are set
      Assert.AreEqual(0, registers.A);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsTrue(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsTrue(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_ADC_8bit_NoCarry()
    {
      //Given; register A set to 0x00 and register B is set to 10, carry flag set
      registers = new Registers();
      registers.B = 10;
      registers.SetFlag(Flag.CARRY);

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction ADC A, B is run
      instructionSet.RunInstruction(0x88);

      //Then; register A set to 11; all flags set to 0
      Assert.AreEqual(11, registers.A);
      Assert.IsFalse(registers.GetFlag(Flag.ZERO));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_ADC_8bit_WithHalfCarry()
    {
      //Given; register A set to 15 and register B is set to 20, carry flag is set
      registers = new Registers();
      registers.A = 15;
      registers.B = 20;
      registers.SetFlag(Flag.CARRY);

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction ADC A, B is run
      instructionSet.RunInstruction(0x88);

      //Then; register A set to 35; all flags set to 0 except half carry
      Assert.AreEqual(36, registers.A);
      Assert.IsFalse(registers.GetFlag(Flag.ZERO));
      Assert.IsTrue(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_ADC_8bit_WithCarry()
    {
      //Given; register A set to 255 and register B is set to 0, carry flag is set
      registers = new Registers();
      registers.A = 255;
      registers.SetFlag(Flag.CARRY);

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction ADC A, B is run
      instructionSet.RunInstruction(0x88);

      //Then; register A set to 0; zero, carry and halfcarry flags are set
      Assert.AreEqual(0, registers.A);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsTrue(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsTrue(registers.GetFlag(Flag.CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_SUB_8bit_NoCarry()
    {
      //Given; register A set to 0x10 and register B is set to 0x10
      registers = new Registers();
      registers.A = 0x10;
      registers.B = 0x10;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction SUB A, B is run
      instructionSet.RunInstruction(0x90);

      //Then; register A set to 0; zero and substraction flags are set
      Assert.AreEqual(0, registers.A);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsTrue(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_SBC_8bit_NoCarry()
    {
      //Given; register A set to 10 and register B is set to 9, carry flag is set
      registers = new Registers();
      registers.A = 10;
      registers.B = 9;
      registers.SetFlag(Flag.CARRY);

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction SBC A, B is run
      instructionSet.RunInstruction(0x98);

      //Then; register A set to 0; zero and substraction flags are set
      Assert.AreEqual(0, registers.A);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsTrue(registers.GetFlag(Flag.SUBSTRACTION));
    }

    [TestMethod]
    public void TestInstruction_CP()
    {
      //Given; register A set to 0x10 and register B is set to 0x10
      registers = new Registers();
      registers.A = 0x10;
      registers.B = 0x10;

      mmu = new MMU();

      instructionSet = new InstructionSet(registers, mmu);

      //When; instruction SUB A, B is run
      instructionSet.RunInstruction(0xB8);

      //Then; register A set to 0; zero and substraction flags are set
      Assert.AreEqual(0x10, registers.A);
      Assert.IsTrue(registers.GetFlag(Flag.ZERO));
      Assert.IsFalse(registers.GetFlag(Flag.HALF_CARRY));
      Assert.IsFalse(registers.GetFlag(Flag.CARRY));
      Assert.IsTrue(registers.GetFlag(Flag.SUBSTRACTION));
    }
  }
}
