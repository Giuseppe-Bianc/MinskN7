namespace Minsk.CodeAnalysis {
    public sealed class Evaluator {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root) {
            this._root = root;
        }

        public int Evaluate() {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node) {
            switch (node) {
                case LiteralExpressionSyntax n:
                    return (int)n.LiteralToken.Value;
                case UnaryExpressionSyntax u:
                    var operand = EvaluateExpression(u.Operand);

                    return u.OperatorToken.Kind switch {
                        SyntaxKind.PlusToken => operand,
                        SyntaxKind.MinusToken => -operand,
                        _ => throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}"),
                    };
                case BinaryExpressionSyntax b: {
                        var left = EvaluateExpression(b.Left);
                        var right = EvaluateExpression(b.Right);

                        return b.OperatorToken.Kind switch {
                            SyntaxKind.PlusToken => left + right,
                            SyntaxKind.MinusToken => left - right,
                            SyntaxKind.StarToken => left * right,
                            SyntaxKind.SlashToken => left / right,
                            _ => throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}")
                        };
                    }

                case ParenthesizedExpressionSyntax p:
                    return EvaluateExpression(p.Expression);
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }
    }
}