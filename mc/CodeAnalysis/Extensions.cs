namespace Minsk.CodeAnalysis {
    public static class Extensions {
        public static string ASCIIUnicode(this char c) {
            int asciiValue = c;
            return $"'{c}' ASCII: {asciiValue} Unicode: U-{asciiValue:X4}";
        }

        public static bool EqualsIgnoreCase(this string str1, string str2) {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this char c1, char c2) {
            return char.ToUpperInvariant(c1).Equals(char.ToUpperInvariant(c2));
        }
    }
}
