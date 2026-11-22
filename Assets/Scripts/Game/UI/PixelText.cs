using System.Collections.Generic;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// Measures text in a pixel font (PixelFontBuilder) from its glyph advances, so a layout can be fitted
    /// before the Canvas has laid it out: the Caption's width and the Action Bar's label size.
    /// </summary>
    public static class PixelText
    {
        /// <summary>The width of the text on one line, in UI units.</summary>
        public static float Width(Font font, string text)
        {
            if (font == null || string.IsNullOrEmpty(text)) return 0f;
            var advances = Advances(font);
            var width = 0f;
            foreach (var ch in text)
                if (advances.TryGetValue(ch, out var a)) width += a;
            return width;
        }

        /// <summary>How many lines the text wraps to at the width, breaking between words as Text does.</summary>
        public static int Lines(Font font, string text, float maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            var space = Width(font, " ");
            var lines = 1;
            var line = 0f;
            foreach (var word in text.Split(' '))
            {
                var w = Width(font, word);
                if (line > 0f && line + space + w > maxWidth) { lines++; line = w; }
                else line += (line > 0f ? space : 0f) + w;
            }
            return lines;
        }

        static readonly Dictionary<Font, Dictionary<char, float>> Cache = new Dictionary<Font, Dictionary<char, float>>();

        static Dictionary<char, float> Advances(Font font)
        {
            if (Cache.TryGetValue(font, out var advances) && advances.Count > 0) return advances;
            advances = new Dictionary<char, float>();
            foreach (var info in font.characterInfo ?? new CharacterInfo[0])
                advances[(char)info.index] = info.advance;
            Cache[font] = advances;
            return advances;
        }
    }
}
