using System;

namespace MyLanguagePackage
{
    public class Token
    {
        private MyTokenType sequenceTerminator;
        private MyTokenType tokenType;

        public Token(MyTokenType sequenceTerminator)
        {
            this.sequenceTerminator = sequenceTerminator;
        }

        public Token(MyTokenType tokenType, string value)
        {
            this.tokenType = tokenType;
            Value = value;
        }

        public Token(string type, string value, TokenPosition position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public TokenPosition Position { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("Token: {{ Type: \"{0}\", Value: \"{1}\", Position: {{ Index: \"{2}\", Line: \"{3}\", Column: \"{4}\" }} }}", Type, Value, Position.Index, Position.Line, Position.Column);
        }
    }
    public enum MyTokenType
    {
        NotDefined,
        And,
        Application,
        Between,
        CloseParenthesis,
        Comma,
        DateTimeValue,
        Equals,
        ExceptionType,
        Fingerprint,
        In,
        Invalid,
        Like,
        Limit,
        Match,
        Message,
        NotEquals,
        NotIn,
        NotLike,
        NumberValue,
        Or,
        OpenParenthesis,
        StackFrame,
        StringValue,
        SequenceTerminator,
        Operator,
        Literal,
        Identifier
    }
}
