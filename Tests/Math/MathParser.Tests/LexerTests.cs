﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LexerTests.cs" company="Decerno">
//   (c) 2011, Jelle de Vries
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MathParser.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// Test cases for the Lexer class
    /// </summary>
    public class LexerTests
    {
        /// <summary>
        /// Test if providing a null argument causes the Split method to throw
        /// an argument exception
        /// </summary>
        [Fact]
        public void SplitCalledWithNullThrowsArgumentException()
        {
            var lexer = new Lexer(string.Empty);
            Assert.Throws<ArgumentException>(() => lexer.Split(null).ToList());
        }

        /// <summary>
        /// Test if providing an empty string argument causes the Split method
        /// to throw an argument exception
        /// </summary>
        [Fact]
        public void SplitCalledWithEmptyStringThrowsArgumentException()
        {
            var lexer = new Lexer(string.Empty);
            Assert.Throws<ArgumentException>(() => lexer.Split(string.Empty).ToList());
        }

        /// <summary>
        /// Test if providing a single whitespace sign causes the split method to
        /// return only this whitespace lexeme
        /// </summary>
        [Fact]
        public void SplitCalledWithWhitespaceReturnsWhitespaceLexeme()
        {
            // Arrange
            var lexer = new Lexer(string.Empty);
            const string input = " ";

            // Act
            var output = new List<string>(lexer.Split(input));

            // Assert
            Assert.Equal(1, output.Count);
            Assert.Equal(" ", output[0]);
        }

        /// <summary>
        /// Test if providing a double whitespaces causes the split method to
        /// return a single lexeme containing those whitespaces
        /// </summary>
        [Fact]
        public void SplitCalledWithTwoWhitespacesReturnsOneLexeme()
        {
            // Arrange
            var lexer = new Lexer(@"(\s+)");
            const string input = "  ";

            // Act
            var output = new List<string>(lexer.Split(input));

            // Assert
            Assert.Equal(1, output.Count);
            Assert.Equal("  ", output[0]);
        }

        /// <summary>
        /// Test if providing a single digits splits the 10 digits
        /// into three correct lexemes
        /// </summary>
        [Fact]
        public void SplitOn5CalledWithAllNumbersReturnsThreeLexemes()
        {
            // Arrange
            var lexer = new Lexer(@"(5)");
            const string input = "0123456789";

            // Act
            var output = new List<string>(lexer.Split(input));

            // Assert
            Assert.Equal(3, output.Count);
            Assert.Equal("01234", output[0]);
            Assert.Equal("5", output[1]);
            Assert.Equal("6789", output[2]);
        }

        /// <summary>
        /// Test if providing to digits 3 and 5 splits the 10 digits
        /// into five correct lexemes
        /// </summary>
        [Fact]
        public void SplitOn3And7CalledWithAllNumbersReturnsFiveLexemes()
        {
            // Arrange
            var lexer = new Lexer(@"(3)|(7)");
            const string input = "01234567890";

            // Act
            var output = new List<string>(lexer.Split(input));

            // Assert
            Assert.Equal(5, output.Count);
            Assert.Equal("012", output[0]);
            Assert.Equal("3", output[1]);
            Assert.Equal("456", output[2]);
            Assert.Equal("7", output[3]);
            Assert.Equal("890", output[4]);
        }

        /// <summary>
        /// Test if providing to digits 3 and 5 splits the 10 digits
        /// into five correct lexemes
        /// </summary>
        [Fact]
        public void SplitOn4And5CalledWithAllNumbersReturnsFourLexemes()
        {
            // Arrange
            var lexer = new Lexer(@"(4)|(5)");
            const string input = "0123456789";

            // Act
            var output = new List<string>(lexer.Split(input));

            // Assert
            Assert.Equal(4, output.Count);
            Assert.Equal("0123", output[0]);
            Assert.Equal("4", output[1]);
            Assert.Equal("5", output[2]);
            Assert.Equal("6789", output[3]);
        }

        /// <summary>
        /// Test if splitting a decimal fixed point number returns this as a single lexeme
        /// </summary>
        [Fact]
        public void SplitOnNumericalWithDecimalpoint()
        {
            // Arrange
            var lexer = new Lexer(@"(\d+\.?\d*)");
            const string input = "12.825765865";

            // Act
            var output = new List<string>(lexer.Split(input));
            Console.WriteLine("Input:\t'" + input + "'");
            Console.WriteLine("--------------------------");
            foreach (var lexeme in output)
            {
                Console.WriteLine("'" + lexeme + "'");
            }

            // Assert
            Assert.Equal(1, output.Count);
        }
     }
}
