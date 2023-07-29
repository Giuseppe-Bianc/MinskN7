namespace Minsk.CodeAnalysis {
    public class SyntaxToken : SyntaxNode {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value) {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; init; }
        public string Text { get; init; }
        public object Value { get; init; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}