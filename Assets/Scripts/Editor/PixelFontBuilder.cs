using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DisasterSafety.EditorTools
{
    /// <summary>
    /// Builds legacy custom Font assets from the Aseprite font atlas so uGUI Text and
    /// InputField draw the game's pixel font. One Font per integer scale, because custom
    /// fonts ignore Text.fontSize.
    /// </summary>
    public static class PixelFontBuilder
    {
        public const string AtlasPath = "Assets/Art/Font/pixel_font.png";
        public const string MetricsPath = "Assets/Art/Font/pixel_font.txt";
        public const string OutputDir = "Assets/Art/Font/Generated";
        public static readonly int[] Scales = { 3, 4, 6, 8 };

        const int CellHeight = 11;   // rows per glyph cell in the atlas
        const int RowsAboveBaseline = 9;
        const int RowsBelowBaseline = 2;

        public static string FontPath(int scale) => OutputDir + "/PixelFont_x" + scale + ".fontsettings";

        public static Font Load(int scale) => AssetDatabase.LoadAssetAtPath<Font>(FontPath(scale));

        public static void Build()
        {
            var atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(AtlasPath);
            if (atlas == null) throw new FileNotFoundException("Font atlas missing. Run the Aseprite art build.", AtlasPath);
            var glyphs = ReadMetrics(MetricsPath);

            Directory.CreateDirectory(OutputDir);
            var materialPath = OutputDir + "/PixelFont.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                material = new Material(Shader.Find("UI/Default"));
                AssetDatabase.CreateAsset(material, materialPath);
            }
            material.mainTexture = atlas;
            EditorUtility.SetDirty(material);

            foreach (var scale in Scales)
            {
                var path = FontPath(scale);
                var font = AssetDatabase.LoadAssetAtPath<Font>(path);
                var isNew = font == null;
                if (isNew) font = new Font("PixelFont_x" + scale);

                font.material = material;
                font.characterInfo = MakeCharacters(glyphs, atlas.width, atlas.height, scale);
                if (isNew) AssetDatabase.CreateAsset(font, path);

                var so = new SerializedObject(font);
                SetFloat(so, "m_LineSpacing", (CellHeight + 2) * scale);
                SetFloat(so, "m_Ascent", RowsAboveBaseline * scale);
                SetFloat(so, "m_Descent", -RowsBelowBaseline * scale);
                SetInt(so, "m_FontSize", CellHeight * scale);
                so.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(font);
            }
            AssetDatabase.SaveAssets();
        }

        struct Glyph
        {
            public int Code, X, Y, Width;
        }

        static List<Glyph> ReadMetrics(string path)
        {
            var list = new List<Glyph>();
            foreach (var raw in File.ReadAllLines(path))
            {
                var line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#")) continue;
                var p = line.Split(' ');
                list.Add(new Glyph
                {
                    Code = int.Parse(p[0], CultureInfo.InvariantCulture),
                    X = int.Parse(p[1], CultureInfo.InvariantCulture),
                    Y = int.Parse(p[2], CultureInfo.InvariantCulture),
                    Width = int.Parse(p[3], CultureInfo.InvariantCulture),
                });
            }
            return list;
        }

        static CharacterInfo[] MakeCharacters(List<Glyph> glyphs, int atlasWidth, int atlasHeight, int scale)
        {
            var result = new CharacterInfo[glyphs.Count];
            for (var i = 0; i < glyphs.Count; i++)
            {
                var g = glyphs[i];
                float u0 = g.X / (float)atlasWidth;
                float u1 = (g.X + g.Width) / (float)atlasWidth;
                float vTop = 1f - g.Y / (float)atlasHeight;
                float vBottom = 1f - (g.Y + CellHeight) / (float)atlasHeight;
                var ci = new CharacterInfo
                {
                    index = g.Code,
                    uvBottomLeft = new Vector2(u0, vBottom),
                    uvBottomRight = new Vector2(u1, vBottom),
                    uvTopLeft = new Vector2(u0, vTop),
                    uvTopRight = new Vector2(u1, vTop),
                    minX = 0,
                    maxX = g.Width * scale,
                    minY = -RowsBelowBaseline * scale,
                    maxY = RowsAboveBaseline * scale,
                    advance = (g.Width + 1) * scale,
                };
                result[i] = ci;
            }
            return result;
        }

        static void SetFloat(SerializedObject so, string name, float value)
        {
            var p = so.FindProperty(name);
            if (p != null) p.floatValue = value;
            else Debug.LogWarning("Font property not found: " + name);
        }

        static void SetInt(SerializedObject so, string name, int value)
        {
            var p = so.FindProperty(name);
            if (p == null)
            {
                Debug.LogWarning("Font property not found: " + name);
                return;
            }
            if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
            else p.intValue = value;
        }
    }
}
