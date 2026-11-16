using System.Text;

namespace DisasterSafety.Core
{
    public enum DisplayNameError
    {
        None,
        Empty,
        TooLong,
        InvalidCharacter
    }

    /// <summary>
    /// Rules for the Display Name: 1 to 12 characters after trimming, inner spaces collapsed,
    /// only English letters, ñ/Ñ, digits and single spaces (the characters the game font can draw).
    /// </summary>
    public static class DisplayNameRule
    {
        public const int MaxLength = 12;

        public static bool IsAllowed(char c) =>
            (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == 'ñ' || c == 'Ñ' || c == ' ';

        public static DisplayNameError Check(string raw, out string normalized)
        {
            normalized = string.Empty;
            if (raw == null) return DisplayNameError.Empty;

            var sb = new StringBuilder(raw.Length);
            var pendingSpace = false;
            foreach (var c in raw)
            {
                if (char.IsWhiteSpace(c))
                {
                    pendingSpace = sb.Length > 0;
                    continue;
                }
                if (!IsAllowed(c)) return DisplayNameError.InvalidCharacter;
                if (pendingSpace) sb.Append(' ');
                pendingSpace = false;
                sb.Append(c);
            }

            if (sb.Length == 0) return DisplayNameError.Empty;
            if (sb.Length > MaxLength) return DisplayNameError.TooLong;
            normalized = sb.ToString();
            return DisplayNameError.None;
        }
    }
}
