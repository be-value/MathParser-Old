﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lexer.cs" company="Decerno">
//   (c) 2010, Jelle de Vries
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MathParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The <see cref="Lexer"/> is responsible for splitting an input string
    /// into a series of lexemes
    /// </summary>
    internal class Lexer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lexer"/> class.
        /// </summary>
        public Lexer()
        {
            Pattern = Patterns.All;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexer"/> class.
        /// </summary>
        /// <param name="pattern">
        /// The pattern to use for lexical analysis.
        /// </param>
        internal Lexer(string pattern)
        {
            Pattern = pattern;
        }

        /// <summary>
        /// Gets or sets the splitting pattern used by this 
        /// </summary>
        private string Pattern { get; }

        /// <summary>
        /// Splitting of the string into lexemes
        /// </summary>
        /// <param name="expression">The subject of this lexical analysis</param>
        /// <returns>The list of identified lexemes</returns>
        public IEnumerable<string> Split(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentException("parameter cannot be null or empty", "expression");
            }

            return from s in Regex.Split(expression, Pattern) 
                   where !string.IsNullOrEmpty(s) 
                   select s;
        }
    }
}
