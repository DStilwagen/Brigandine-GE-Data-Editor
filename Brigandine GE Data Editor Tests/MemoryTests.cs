using System;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using BrigandineGEDataEditor;
using BrigandineGEDataEditor.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrigandineGEDataEditorTests
{
    [TestClass]
    public class MemoryTests
    {
        private readonly MemoryAccessor memoryAccessor = MemoryAccessor.CreateAccessor();

        [TestMethod]
        public void AdjustAddressTest()
        {
            //Base address is 0x8000F800;
            var testPtr = 0x801A1119;
            var expectedPtr = 0x191919;
            Assert.AreEqual(expectedPtr, MemoryAccessor.AdjustAddress(testPtr));
        }

        [TestMethod]
        public void DereferenceStringTest()
        {
            var namePtr = 2147572493;
            var expectedString = "Vampire Lord";
            Assert.AreEqual(expectedString, memoryAccessor.DereferenceString(namePtr));
        }

        [TestMethod]
        public void ConstructorAndPropertiesInitializeProperlyTest()
        {
            Assert.AreEqual(MemoryAddresses.Attacks.Length, memoryAccessor.Attacks.Length);
            Assert.AreEqual(MemoryAddresses.Castles.Length, memoryAccessor.Castles.Length);
            Assert.AreEqual(MemoryAddresses.DefaultKnights.Length, memoryAccessor.DefaultKnights.Length);
            Assert.AreEqual(MemoryAddresses.Classes.Length, memoryAccessor.Classes.Length);
            Assert.AreEqual(MemoryAddresses.Items.Length, memoryAccessor.Items.Length);
            Assert.AreEqual(MemoryAddresses.SpecialAttacks.Length, memoryAccessor.SpecialAttacks.Length);
            Assert.AreEqual(MemoryAddresses.Spells.Length, memoryAccessor.Spells.Length);
            Assert.AreEqual(MemoryAddresses.Skills.Length, memoryAccessor.Skills.Length);
#if DEBUG_MEMORY_STATGROWTH_BROKEN
            Assert.AreEqual(MemoryAddresses.StatGrowths.Length, memoryAccessor.StatGrowths.Length);
            Assert.AreNotEqual(0, MemoryAccessor.StatGrowths.HPGrowth);☺
#endif
            Assert.AreEqual("Full Blade", memoryAccessor.DereferenceString(memoryAccessor.Attacks[0].Name));
            Assert.AreEqual("Hervery", memoryAccessor.DereferenceString(memoryAccessor.Castles[0].Name));
            Assert.AreEqual(1, memoryAccessor.DefaultKnights[0].Level);
            Assert.AreEqual("Fighter", memoryAccessor.DereferenceString(memoryAccessor.Classes[0].Name));
            Assert.AreEqual("Solomon's Ring", memoryAccessor.DereferenceString(memoryAccessor.Items[0].Name));
            Assert.AreEqual("Howl Fire",
                memoryAccessor.DereferenceString(memoryAccessor.SpecialAttacks[0].Name));
            Assert.AreEqual("Heal", memoryAccessor.DereferenceString(memoryAccessor.Spells[0].Name));
            Assert.AreEqual("Nimble", memoryAccessor.DereferenceString(memoryAccessor.Skills[0].Name));
        }

        [TestMethod]
        public void CastFromUnsafeStructsTest()
        {
            CastleData firstCastleData = memoryAccessor.Castles[0];
            Assert.AreEqual("Hervery", memoryAccessor.DereferenceString(firstCastleData.Name));

            DefaultKnightData firstDefaultKnight = memoryAccessor.DefaultKnights[0];
            Assert.AreEqual(1, firstDefaultKnight.Level);

            ClassData classData = memoryAccessor.Classes[0];
            Assert.AreEqual("Fighter", memoryAccessor.DereferenceString(classData.Name));

            SpecialAttackData firstSpecialAttackData = memoryAccessor.SpecialAttacks[0];
            Assert.AreEqual("Howl Fire", memoryAccessor.DereferenceString(firstSpecialAttackData.Name));

            SpellData firstSpellData = memoryAccessor.Spells[0];
            Assert.AreEqual("Heal", memoryAccessor.DereferenceString(firstSpellData.Name));
        }

        [TestMethod]
        public void TextDataGetTextTest()
        {
            var castle = memoryAccessor.Castles[0];
            var text = castle.Name;
            Assert.AreEqual("Hervery", text.GetText(memoryAccessor));
        }

        [TestMethod]
        public void TextDataSetTextTest()
        {
            var castle = memoryAccessor.Castles[0];
            var text = castle.Name;
            Assert.AreEqual("Hervery", text.GetText(memoryAccessor));

            var equalLengthString = "1234567";
            if (text.SetText(memoryAccessor, ref equalLengthString))
                Assert.AreEqual("1234567", equalLengthString);
            var smallerLengthString = "12345";
            if (text.SetText(memoryAccessor, ref smallerLengthString))
                Assert.AreEqual("12345", smallerLengthString);
            var largerLengthString = "123456789";
            if (!text.SetText(memoryAccessor, ref largerLengthString))
                Assert.AreNotEqual("1234567", largerLengthString);
        }
        
    }
}