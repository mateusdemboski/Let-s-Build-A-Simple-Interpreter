using System;

namespace Interpreter
{
    public class Interpreter
    {
        /// client string input, e.g. "3+5"
        private readonly string Text;
        /// An index into Text
        private int Pos = 0;
        /// current token instance
        private Token CurrentToken;
        private char? CurrentChar;

        public Interpreter(string text)
        {
            this.Text = text;
            this.CurrentChar = this.Text[this.Pos];
        }

        private Exception Error()
        {
            return new Exception("Error parsing input");
        }

        /// Advance the 'pos' pointer and set the 'CurrentChar' variable.
        private void Advance()
        {
            this.Pos++;
            if(this.Pos > this.Text.Length -1)
                this.CurrentChar = null; // Indicates end of input
            else
                this.CurrentChar = this.Text[this.Pos];
        }

        private void SkipWhitespace()
        {
            while (this.CurrentChar.HasValue && Char.IsWhiteSpace(this.CurrentChar.Value))
                this.Advance();
        }

        /// <returns>A (multidigit) integer consumed from the input.</returns>
        private int Integer()
        {
            var result = "";
            while (this.CurrentChar.HasValue && char.IsDigit(this.CurrentChar.Value))
            {
                result += this.CurrentChar.Value;
                this.Advance();
            }
            return Int32.Parse(result);
        }

        /// Lexical analyzer (also known as scanner or tokenizer)
        /// <summary>
        /// This method is responsible for breaking a sentence
        /// apart into tokens. One token at a time.
        /// </summary>
        private Token GetNextToken()
        {
            while (this.CurrentChar.HasValue)
            {
                if (Char.IsWhiteSpace(this.CurrentChar.Value))
                {
                    this.SkipWhitespace();
                    continue;
                }

                if(Char.IsDigit(this.CurrentChar.Value))
                {
                    return new Token(Types.INTEGER, this.Integer().ToString());
                }
                else if (this.CurrentChar.Value == '+')
                {
                    this.Advance();
                    return new Token(Types.PLUS, "+");
                }
                else if (this.CurrentChar.Value == '-')
                {
                    this.Advance();
                    return new Token(Types.MINUS, "-");
                }

                throw Error();
            }

            return new Token(Types.EOF, null);
        }

        /// <summary>
        /// compare the current token type with the passed token
        /// type and if they match then "eat" the current token
        /// and assign the next token to the self.current_token,
        /// otherwise raise an exception.
        /// </summary>
        private void Eat(Types tokenType)
        {
            if(this.CurrentToken.Type == tokenType)
                this.CurrentToken = this.GetNextToken();
            else
                throw Error();
        }

        /// Parser / Interpreter
        public int Expr()
        {
            // set current token to the first token taken from the input
            this.CurrentToken = this.GetNextToken();
            
            // we expect the current token to be an integer
            var left = this.CurrentToken;
            this.Eat(Types.INTEGER);

            // we expect the current token to be a '+' or '-' token
            var op = this.CurrentToken;
            if(op.Type == Types.PLUS)
                this.Eat(Types.PLUS);
            else
                this.Eat(Types.MINUS);
            
            // we expect the current token to be an integer
            var right = this.CurrentToken;
            this.Eat(Types.INTEGER);
            // after the above call the this.CurrentToken is set to
            // EOF token

            // at this point either the INTEGER PLUS INTEGER or
            // the INTEGER MINUS INTEGER sequence of tokens
            // has been successfully found and the method can just
            // return the result of adding or subtracting two integers,
            // thus effectively interpreting client input
            int result;
            if(op.Type == Types.PLUS)
                result = Int32.Parse(left.Value) + Int32.Parse(right.Value);
            else 
                result = Int32.Parse(left.Value) - Int32.Parse(right.Value);
            return result;
        }
    }
}