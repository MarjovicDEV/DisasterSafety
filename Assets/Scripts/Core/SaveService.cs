using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace DisasterSafety.Core
{
    public enum SaveLoadStatus
    {
        /// <summary>No save file exists yet (fresh install).</summary>
        Missing,
        /// <summary>The save file was read and is valid.</summary>
        Loaded,
        /// <summary>The save file could not be read and was kept as a Preserved Save.</summary>
        Preserved
    }

    public readonly struct SaveLoadResult
    {
        public readonly SaveLoadStatus Status;
        public readonly ProgressState Progress;
        public readonly string PreservedPath;

        public SaveLoadResult(SaveLoadStatus status, ProgressState progress, string preservedPath)
        {
            Status = status;
            Progress = progress;
            PreservedPath = preservedPath;
        }
    }

    /// <summary>
    /// Reads and writes save.json in one directory (Application.persistentDataPath in the game).
    /// Writes go to a temp file first and then replace the real file, so a crash mid-write
    /// never leaves a half-written save. Unreadable files are renamed, never deleted.
    /// </summary>
    public sealed class SaveService
    {
        public const string FileName = "save.json";

        readonly string directory;
        readonly Func<DateTime> clock;

        public SaveService(string directory, Func<DateTime> clock = null)
        {
            this.directory = directory ?? throw new ArgumentNullException(nameof(directory));
            this.clock = clock ?? (() => DateTime.Now);
        }

        public string SavePath => Path.Combine(directory, FileName);

        public SaveLoadResult Load()
        {
            var path = SavePath;
            if (!File.Exists(path)) return new SaveLoadResult(SaveLoadStatus.Missing, null, null);

            string text;
            try
            {
                text = File.ReadAllText(path, Encoding.UTF8);
            }
            catch (IOException)
            {
                return new SaveLoadResult(SaveLoadStatus.Preserved, null, Preserve(path));
            }
            catch (UnauthorizedAccessException)
            {
                return new SaveLoadResult(SaveLoadStatus.Preserved, null, Preserve(path));
            }

            var data = Parse(text);
            if (data == null || !IsValid(data))
                return new SaveLoadResult(SaveLoadStatus.Preserved, null, Preserve(path));

            if (DisplayNameRule.Check(data.displayName, out var name) == DisplayNameError.None)
                data.displayName = name;
            else
                data.displayName = string.Empty;

            return new SaveLoadResult(SaveLoadStatus.Loaded, new ProgressState(data), null);
        }

        public void Save(ProgressState progress)
        {
            if (progress == null) throw new ArgumentNullException(nameof(progress));
            Directory.CreateDirectory(directory);
            progress.Data.saveVersion = SaveData.CurrentVersion;

            var path = SavePath;
            var temp = path + ".tmp";
            var json = JsonUtility.ToJson(progress.Data, true);
            using (var stream = new FileStream(temp, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
            {
                writer.Write(json);
                writer.Flush();
                stream.Flush(true);
            }

            if (!File.Exists(path))
            {
                File.Move(temp, path);
                return;
            }

            try
            {
                File.Replace(temp, path, null);
            }
            catch (PlatformNotSupportedException)
            {
                File.Copy(temp, path, true);
                File.Delete(temp);
            }
        }

        static SaveData Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            try
            {
                return JsonUtility.FromJson<SaveData>(text);
            }
            catch (Exception)
            {
                // JsonUtility reports malformed JSON with ArgumentException today; any parse
                // failure means the file is unreadable and must be preserved, not crash startup.
                return null;
            }
        }

        static bool IsValid(SaveData data)
        {
            if (data.saveVersion < 1 || data.saveVersion > SaveData.CurrentVersion) return false;

            data.displayName = data.displayName ?? string.Empty;
            data.openEpisodes = data.openEpisodes ?? new System.Collections.Generic.List<string>();
            data.openDifficulties = data.openDifficulties ?? new System.Collections.Generic.List<string>();
            data.completedLevels = data.completedLevels ?? new System.Collections.Generic.List<LevelRecord>();
            data.lastEpisode = data.lastEpisode ?? string.Empty;
            data.lastDifficulty = data.lastDifficulty ?? string.Empty;

            foreach (var e in data.openEpisodes)
                if (!Curriculum.TryParseEpisode(e, out _)) return false;
            foreach (var d in data.openDifficulties)
                if (!Curriculum.TryParseKey(d, out _, out _)) return false;
            foreach (var r in data.completedLevels)
            {
                if (r == null || !LevelId.TryParse(r.level, out _)) return false;
                if (r.stars < 0 || r.stars > ProgressState.MaxStars) return false;
            }
            if (data.lastEpisode.Length > 0 && !Curriculum.TryParseEpisode(data.lastEpisode, out _)) return false;
            if (data.lastDifficulty.Length > 0 && !Curriculum.TryParseDifficulty(data.lastDifficulty, out _)) return false;
            return true;
        }

        /// <summary>Moves an unreadable file to save.corrupt-yyyyMMdd-HHmmss[-n].json and returns the new path.</summary>
        string Preserve(string path)
        {
            var stamp = clock().ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            var target = Path.Combine(directory, "save.corrupt-" + stamp + ".json");
            for (var n = 2; File.Exists(target); n++)
                target = Path.Combine(directory, "save.corrupt-" + stamp + "-" + n + ".json");
            File.Move(path, target);
            return target;
        }
    }
}
