using Minsk.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Minsk {
    class Program {
        static void Main(string[] args) {
            bool showTree = false;

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

                if (showTree) {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ForegroundColor = color;
                }

                if (!syntaxTree.Diagnostics.Any()) {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                } else {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Span<string> diagnostics = CollectionsMarshal.AsSpan(syntaxTree.Diagnostics.ToList());
                    ref var searcSpace = ref MemoryMarshal.GetReference(diagnostics);
                    for (int i = 0; i < diagnostics.Length; i++) {
                        var item = Unsafe.Add(ref searcSpace, i);
                        Console.WriteLine(item);
                    }

                    Console.ForegroundColor = color;
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true) {
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

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
