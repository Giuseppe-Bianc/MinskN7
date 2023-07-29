namespace Minsk.CodeAnalysis {
     class NumberExpressionSyntax : ExpressionSyntax {
        public NumberExpressionSyntax(SyntaxToken numberToken) {
            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxToken NumberToken { get; init; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return NumberToken;
        }
    }
}