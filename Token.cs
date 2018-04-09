namespace Interpreter
{
    public class Token
    {
        /// token type: INTEGER, PLUS, or EOF
        public Types Type { get; set; }
        /// token value: 0, 1, 2. 3, 4, 5, 6, 7, 8, 9, '+', or None
        public string Value { get; set; }

        public Token(){}

        public Token(Types type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// String representation of the class instance.
        /// </summary>
        public override string ToString()
        {
            return $"Token({this.Type.ToString()}, {this.Value})";
        }
    }
}