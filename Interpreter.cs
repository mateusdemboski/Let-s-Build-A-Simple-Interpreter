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

        public Interpreter(string text)
        {
            this.Text = text;
        }

        private Exception Error()
        {
            return new Exception("Error parsing input");
        }
        
        /// Lexical analyzer (also known as scanner or tokenizer)
        /// <summary>
        /// This method is responsible for breaking a sentence
        /// apart into tokens. One token at a time.
        /// </summary>
        private Token GetNextToken()
        {
            // is `this.Pos` index past the end of the `this.Text` ?
            // if so, then return EOF token because there is no more
            // input left to convert into tokens
            if(this.Pos > Text.Length -1)
                return new Token(Types.EOF, null);

            // get a character at the position this.Pos and decide
            // what token to create based on the single character
            var currentChar = this.Text[this.Pos];

            // if the character is a digit then convert it to
            // integer, create an INTEGER token, increment this.Pos
            // index to point to the next character after the digit,
            // and return the INTEGER token
            if(Char.IsNumber(currentChar))
            {
                this.Pos++;
                return new Token(Types.INTEGER, currentChar.ToString());
            }
            else if (currentChar == '+')
            {
                this.Pos++;
                return new Token(Types.PLUS, currentChar.ToString());
            }

            throw Error();
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

        /// Expr -> INTEGER PLUS INTEGER
        public int Expr()
        {
            // set current token to the first token taken from the input
            this.CurrentToken = this.GetNextToken();
            
            // we expect the current token to be a single-digit integer
            var left = this.CurrentToken;
            this.Eat(Types.INTEGER);

            // we expect the current token to be a '+' token
            var op = this.CurrentToken;
            this.Eat(Types.PLUS);
            
            // we expect the current token to be a single-digit integer
            var right = this.CurrentToken;
            this.Eat(Types.INTEGER);
            // after the above call the this.CurrentToken is set to
            // EOF token

            // at this point INTEGER PLUS INTEGER sequence of tokens
            // has been successfully found and the method can just
            // return the result of adding two integers, thus
            // effectively interpreting client input
            var result = Int32.Parse(left.Value) + Int32.Parse(right.Value);
            return result;
        }
    }
}