using System;
using UnityEngine;

namespace DisasterSafety.Core
{
    /// <summary>
    /// The situation one Level presents. This class is also the JSON shape of
    /// Resources/Scenarios/&lt;Episode&gt;/&lt;Difficulty&gt;/&lt;n&gt;.json, so field names are the file format.
    /// </summary>
    [Serializable]
    public sealed class Scenario
    {
        /// <summary>LevelId key, e.g. "Earthquake/Easy/1".</summary>
        public string level;
        /// <summary>"before", "during" or "after".</summary>
        public string phase;
        /// <summary>Art key for the scene picture, e.g. "classroom".</summary>
        public string setting;
        public TextLine goal;
        /// <summary>Bantay's opening line. A story line, so it may have no source.</summary>
        public TextLine intro;
        public TextLine hint;
        /// <summary>The short explanation shown on Level Completed.</summary>
        public TextLine lesson;
        public ScenarioChoice[] choices;

        /// <summary>Returns null when the text is not a readable Scenario.</summary>
        public static Scenario Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                return JsonUtility.FromJson<Scenario>(json);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public bool TryGetLevel(out LevelId id) => LevelId.TryParse(level, out id);

        public int SafeChoiceIndex
        {
            get
            {
                if (choices == null) return -1;
                for (var i = 0; i < choices.Length; i++)
                    if (choices[i] != null && choices[i].safe) return i;
                return -1;
            }
        }

        public ScenarioChoice SafeChoice => SafeChoiceIndex >= 0 ? choices[SafeChoiceIndex] : null;
    }

    /// <summary>One sentence or two, with the research findings it comes from (e.g. "EQ-10").</summary>
    [Serializable]
    public sealed class TextLine
    {
        public string text;
        public string[] sources;
    }

    /// <summary>One action the Learner can pick.</summary>
    [Serializable]
    public sealed class ScenarioChoice
    {
        public string id;
        public string label;
        /// <summary>Art key for the card picture and the Feedback picture.</summary>
        public string picture;
        public bool safe;
        /// <summary>Optional Feedback heading, e.g. "Not safe yet". Empty uses "Safe!" or "Not safe".</summary>
        public string heading;
        public string feedback;
        public string[] sources;
    }
}
