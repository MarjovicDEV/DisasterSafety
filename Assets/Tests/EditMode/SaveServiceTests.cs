using System;
using System.IO;
using System.Linq;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class SaveServiceTests
    {
        string dir;
        DateTime now;
        SaveService service;

        [SetUp]
        public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "ds-save-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            now = new DateTime(2026, 11, 16, 9, 30, 15);
            service = new SaveService(dir, () => now);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }

        string SavePath => Path.Combine(dir, SaveService.FileName);

        [Test]
        public void Load_MissingFile_ReportsMissing()
        {
            var result = service.Load();
            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Missing));
            Assert.That(result.Progress, Is.Null);
        }

        [Test]
        public void SaveThenLoad_RoundTripsProgress()
        {
            var p = ProgressState.NewGame();
            p.SetDisplayName("Niño");
            p.RecordCompletion(new LevelId(Episode.Earthquake, Difficulty.Easy, 2), 3);
            service.Save(p);

            var result = service.Load();
            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Loaded));
            Assert.That(result.Progress.DisplayName, Is.EqualTo("Niño"));
            Assert.That(result.Progress.HasSavedProgress, Is.True);
            Assert.That(result.Progress.StarsFor(new LevelId(Episode.Earthquake, Difficulty.Easy, 2)), Is.EqualTo(3));
            Assert.That(result.Progress.IsOpen(Episode.Earthquake, Difficulty.Easy), Is.True);
            Assert.That(result.Progress.IsOpen(Episode.Fire), Is.False);
        }

        [Test]
        public void SaveThenLoad_RoundTripsTheStudentPick()
        {
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");
            p.SetStudent(StudentKind.Girl);
            service.Save(p);

            Assert.That(service.Load().Progress.Student, Is.EqualTo(StudentKind.Girl));
        }

        [Test]
        public void Load_OldSaveWithoutStudent_ReadsNoneAndKeepsTheRest()
        {
            File.WriteAllText(SavePath,
                "{\"saveVersion\":1,\"displayName\":\"Ana\",\"openEpisodes\":[\"Earthquake\"],"
                + "\"openDifficulties\":[\"Earthquake/Easy\"],"
                + "\"completedLevels\":[{\"level\":\"Earthquake/Easy/2\",\"stars\":2}],"
                + "\"lastEpisode\":\"Earthquake\",\"lastDifficulty\":\"Easy\"}");

            var result = service.Load();
            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Loaded));
            Assert.That(result.Progress.Student, Is.EqualTo(StudentKind.None));
            Assert.That(result.Progress.DisplayName, Is.EqualTo("Ana"));
            Assert.That(result.Progress.StarsFor(new LevelId(Episode.Earthquake, Difficulty.Easy, 2)), Is.EqualTo(2));
        }

        [TestCase("{ this is not json")]
        [TestCase("")]
        [TestCase("{}")]
        [TestCase("{\"saveVersion\":999}")]
        [TestCase("{\"saveVersion\":1,\"completedLevels\":[{\"level\":\"Volcano/Easy/1\",\"stars\":1}]}")]
        public void Load_UnreadableFile_IsPreservedNotDeleted(string contents)
        {
            File.WriteAllText(SavePath, contents);

            var result = service.Load();

            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Preserved));
            Assert.That(result.Progress, Is.Null);
            Assert.That(File.Exists(SavePath), Is.False, "the unreadable file must be moved aside");
            Assert.That(result.PreservedPath, Does.EndWith("save.corrupt-20261116-093015.json"));
            Assert.That(File.ReadAllText(result.PreservedPath), Is.EqualTo(contents));
        }

        [Test]
        public void Load_TwoUnreadableFilesInOneSecond_KeepsBoth()
        {
            File.WriteAllText(SavePath, "first");
            var a = service.Load();
            File.WriteAllText(SavePath, "second");
            var b = service.Load();

            Assert.That(a.PreservedPath, Is.Not.EqualTo(b.PreservedPath));
            Assert.That(File.ReadAllText(a.PreservedPath), Is.EqualTo("first"));
            Assert.That(File.ReadAllText(b.PreservedPath), Is.EqualTo("second"));
        }

        [Test]
        public void Save_ReplacesExistingFile_AndLeavesNoTempFile()
        {
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");
            service.Save(p);
            p.SetDisplayName("Bea");
            service.Save(p);

            Assert.That(service.Load().Progress.DisplayName, Is.EqualTo("Bea"));
            Assert.That(Directory.GetFiles(dir).Select(Path.GetFileName), Is.EquivalentTo(new[] { SaveService.FileName }));
        }

        [Test]
        public void Save_IgnoresAStaleTempFileFromACrash()
        {
            File.WriteAllText(SavePath + ".tmp", "half written");
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");
            service.Save(p);

            Assert.That(service.Load().Progress.DisplayName, Is.EqualTo("Ana"));
            Assert.That(File.Exists(SavePath + ".tmp"), Is.False);
        }

        [Test]
        public void Load_InvalidDisplayName_IsClearedButProgressKept()
        {
            File.WriteAllText(SavePath,
                "{\"saveVersion\":1,\"displayName\":\"<script>\",\"openEpisodes\":[\"Earthquake\"],\"openDifficulties\":[\"Earthquake/Easy\"],\"completedLevels\":[{\"level\":\"Earthquake/Easy/1\",\"stars\":2}]}");

            var result = service.Load();

            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Loaded));
            Assert.That(result.Progress.HasDisplayName, Is.False);
            Assert.That(result.Progress.HasSavedProgress, Is.True);
        }
    }
}
