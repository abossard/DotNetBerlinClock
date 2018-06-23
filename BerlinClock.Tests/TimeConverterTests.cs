using System;
using System.Linq;
using BerlinClock.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BerlinClock.Tests
{
    [TestClass]
    public class TimeConverterTests
    {
        
        [TestMethod]
        public void VerifyTimeTuple_ValidInput_Success()
        {
            // Arrange
            var input = (24, 0 , 0);

            // Act, Assert
            TimeConverter.VerifyTimeTuple(input);
        }

        [TestMethod]
        public void VerifyTimeTuple_InvalidInput_Exception()
        {
            // Arrange
            var input = (25, 0 , 0);

            // Act, Assert
            Assert.ThrowsException<ArgumentException>(() => TimeConverter.VerifyTimeTuple(input));
        }

        [TestMethod]
        public void ParseTimeFormat_ValidInput_Success()
        {
            // Arrange
            const string input = "23:59:59";

            // Act 
            var result = TimeConverter.ParseTimeFormat(input);

            // Assert
            Assert.AreEqual(23, result.hours);
            Assert.AreEqual(59, result.minutes);
            Assert.AreEqual(59, result.seconds);
        }

        [TestMethod]
        public void ParseTimeFormat_NotEnoughColons_Throws()
        {
            // Arrange
            const string input = "23:59";

            // Act, Assert
            Assert.ThrowsException<ArgumentException>(() => TimeConverter.ParseTimeFormat(input));
        }

        [TestMethod]
        public void ParseTimeFormat_InvalidNumber_Throws()
        {
            // Arrange
            const string input = "23:59:xx";

            // Act, Assert
            Assert.ThrowsException<ArgumentException>(() => TimeConverter.ParseTimeFormat(input));
        }

        [TestMethod]
        public void ConvertTimeTupeToBerlinTuple_ValidInput_Success()
        {
            // Arrange
            var input = (hours: 23, minutes: 59, seconds: 1);

            // Act 
            var (hours5, hours1, minutes5, minutes1, secondsEven) = TimeConverter.ConvertTimeTupeToBerlinTuple(input);

            // Assert
            Assert.AreEqual(4, hours5);
            Assert.AreEqual(3, hours1);
            Assert.AreEqual(11, minutes5);
            Assert.AreEqual(4, minutes1);
            Assert.AreEqual(false, secondsEven);
        }

        [TestMethod]
        public void CreateFormatFunc_ValidInput_Success()
        {
            // Arrange
            const int positions = 5;
            const int specialPosition = 2;
            const char transpose = 'Z';
            const char defaultCharacter = 'X';
            
            var subjectUnderTest = TimeConverter.CreateFormatFunc(positions, defaultCharacter, index => index == specialPosition, transpose);

            // Act 
            var result = subjectUnderTest(4).ToCharArray();
            
            // Assert
            Assert.AreEqual(positions, result.Length);
            Assert.AreEqual(transpose, result[specialPosition]);
            Assert.AreEqual(defaultCharacter, result.First());
            Assert.AreEqual(TimeConverter.Filler, result.Last());
        }
    }
}