namespace Minsk.CodeAnalysis {
    internal sealed class Lexer {
        private readonly string _text;
        private readonly int _textLenght;
        private int _position;
        private readonly List<string> _diagnostics = new List<string>();

        public Lexer(string text) {
            _text = text;
            _textLenght = _text.Length;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current {
            get => _position >= _textLenght ? '\0' : _text[_position];
        }

        private void Next() {
            _position++;
        }

        public SyntaxToken NextToken() {
            // <numbers>
            // + - * / ( )
            // <whitespace>

            if (_position >= _textLenght)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            if (char.IsDigit(Current)) {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"The number {_text} isn't valid Int32.");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current)) {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            switch (Current) {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            }

            _diagnostics.Add($"ERROR: bad character input: '{Current.ASCIIUnicode()}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}