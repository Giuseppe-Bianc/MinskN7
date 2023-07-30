namespace Minsk.CodeAnalysis.Binding {
    internal sealed class BoundBinaryExpression : BoundExpression {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right) {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Left.Type;
        public BoundExpression Left { get; init; }
        public BoundBinaryOperatorKind OperatorKind { get; init; }
        public BoundExpression Right { get; init; }
    }
}
