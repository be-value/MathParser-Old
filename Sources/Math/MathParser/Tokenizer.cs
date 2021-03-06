﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tokenizer.cs" company="Decerno">
//   (c) 2011, Jelle de Vries
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MathParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="Tokenizer"/> used to transform an expression into a
    /// list of tokens, understood by the parser
    /// </summary>
    internal class Tokenizer
    {
        /// <summary>
        /// Splits lexemes into a list of tokens
        /// </summary>
        /// <param name="lexemes">The lexemes representing the mathematical expression</param>
        /// <returns>The resulting list of tokens</returns>
        public IEnumerable<IToken> Tokenize(IEnumerable<string> lexemes)
        {
            var position = 0;
            if (null == lexemes)
            {
                throw new ArgumentException("parameter cannot be null", "lexemes");
            }

            IToken previous = null;
            foreach (var lexeme in lexemes)
            {
                if (!string.IsNullOrEmpty(lexeme.Trim()))
                {
                    var current = Tokenize(lexeme, position, previous);
                    previous = current;
                    yield return current;
                }

                position += lexeme.Length;
            }
        }

        /// <summary>
        /// Tokenizes a single lexeme
        /// </summary>
        /// <param name="lexeme">The lexeme to tokenize.</param>
        /// <param name="position">The position of the lexeme (token) in the expression.</param>
        /// <param name="previousToken">The previously read token</param>
        /// <returns>The resulting token</returns>
        private static IToken Tokenize(string lexeme, int position, IToken previousToken)
        {
            switch (lexeme)
            {
                case "+":
                    return UnaryCondition(previousToken) ? new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.UnaryPlus }
                        : new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.Addition };
                case "-":
                    return UnaryCondition(previousToken) ? new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.UnaryMinus } 
                        : new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.Subtraction };
                case "*":
                    return new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.Multiplication };
                case "/":
                    return new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.Division };
                case "(":
                    return new MetaToken { Lexeme = lexeme, Position = position, Type = Meta.LeftParenthesis };
                case ")":
                    return new MetaToken { Lexeme = lexeme, Position = position, Type = Meta.RightParenthesis };
                case ",":
                    return new MetaToken { Lexeme = lexeme, Position = position, Type = Meta.Comma };
            }

            if (IsNumber(lexeme))
            {
                return new OperandToken { Lexeme = lexeme, Position = position, Type = Operand.Numeric };
            }

            if (IsFunction(lexeme))
            {
                return new OperatorToken { Lexeme = lexeme, Position = position, Type = Operator.Function };
            }

            if (IsIdentifier(lexeme))
            {
                return new OperandToken { Lexeme = lexeme, Position = position, Type = Operand.Variable };
            }

            return new UnknownToken { Lexeme = lexeme, Position = position };
        }

        /// <summary>
        /// Test if the lexeme represents a numerical value
        /// </summary>
        /// <param name="lexeme">The lexeme to test.</param>
        /// <returns>true if the lexeme represents a numerical value, false otherwise.</returns>
        private static bool IsNumber(string lexeme)
        {
            return Regex.IsMatch(lexeme, Patterns.Number);
        }

        /// <summary>
        /// Test if the lexeme represents a function
        /// </summary>
        /// <param name="lexeme">The lexeme to test</param>
        /// <returns>true if the lexeme represents a function name, false otherwise.</returns>
        private static bool IsFunction(string lexeme)
        {
            return null != Functions.Repository && Functions.Repository.Contains(lexeme);
        }

        /// <summary>
        /// Test if the lexeme represents an identifier
        /// </summary>
        /// <param name="lexeme">The lexeme to test.</param>
        /// <returns>true if the lexeme represents an identifier, false otherwise.</returns>
        private static bool IsIdentifier(string lexeme)
        {
            return Regex.IsMatch(lexeme, Patterns.Identifier);
        }

        /// <summary>
        /// Test if the current operator should be a unary operator based on the previous token
        /// </summary>
        /// <param name="previousToken">The previously read token</param>
        /// <returns>true if the conditions for a unary operator are met, false otherwise.</returns>
        private static bool UnaryCondition(IToken previousToken)
        {
            if (null == previousToken)
            {
                return true;
            }

            if (previousToken is IMetaToken)
            {
                var metaToken = previousToken as IMetaToken;
                var metaType = metaToken.Type;
                return metaType == Meta.LeftParenthesis || metaType == Meta.Comma;
            }

            return previousToken is IOperatorToken;
        }
    }
}
