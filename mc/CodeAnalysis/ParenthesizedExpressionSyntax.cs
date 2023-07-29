namespace Minsk.CodeAnalysis {
    sealed class ParenthesizedExpressionSyntax : ExpressionSyntax {
        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken) {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
        public SyntaxToken OpenParenthesisToken { get; init; }
        public ExpressionSyntax Expression { get; init; }
        public SyntaxToken CloseParenthesisToken { get; init; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }
}