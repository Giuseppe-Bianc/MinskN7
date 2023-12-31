using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Minsk {
    internal static class Program {
        private static void Main() {
            var showTree = false;

            while (true) {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree") {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                    continue;
                } else if (line == "#cls") {
                    Console.Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var binder = new Binder();
                var boundExpression = binder.BindExpression(syntaxTree.Root);

                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();


                if (showTree) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!diagnostics.Any()) {
                    var e = new Evaluator(boundExpression);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    ref var searcSpace = ref MemoryMarshal.GetArrayDataReference(diagnostics);
                    for (int i = 0; i < diagnostics.Length; i++) {
                        var item = Unsafe.Add(ref searcSpace, i);
                        Console.WriteLine(item);
                    }

                    Console.ResetColor();
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true) {
            var marker = isLast ? "└──" : "├──";

            Console.Write($"{indent}{marker}{node.Kind}");

            if (node is SyntaxToken { Value: not null } t) {
                Console.Write($" {t.Value}");
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();
            var childrens = CollectionsMarshal.AsSpan(node.GetChildren().ToList());
            ref var searcSpace = ref MemoryMarshal.GetReference(childrens);
            for (int i = 0; i < childrens.Length; i++) {
                var item = Unsafe.Add(ref searcSpace, i);
                PrettyPrint(item, indent, item == lastChild);
            }
        }
    }
}
