using System.Collections;
using DisasterSafety.Game.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DisasterSafety.Tests
{
    /// <summary>
    /// Act-out Steps on a small test Setting built in the test: a Student that walks up a slope and props
    /// that return to their start place on Try Again.
    /// </summary>
    public sealed class ActOutPlayTests
    {
        GameObject root;
        StudentActor student;
        RectTransform shelf;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("Test Setting", typeof(RectTransform), typeof(Canvas));
            var sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);
            var frames = new[] { sprite, sprite };

            var s = new GameObject("Student", typeof(RectTransform));
            s.transform.SetParent(root.transform, false);
            student = s.AddComponent<StudentActor>();
            var set = new StudentFrames { idle = frames, surprised = frames, walk = frames, duck = frames, wait = frames, happy = frames, crouch = frames, hold = frames };
            student.boy = set;
            student.girl = set;
            student.unitsPerPixel = 4f;
            student.feetY = 160f;
            var body = new GameObject("Body", typeof(RectTransform), typeof(Image));
            body.transform.SetParent(s.transform, false);
            student.body = body.GetComponent<Image>();
            student.PlaceAt(100f, 0f);

            shelf = (RectTransform)new GameObject("Shelf", typeof(RectTransform)).transform;
            shelf.SetParent(root.transform, false);
        }

        [TearDown]
        public void TearDown() => Object.Destroy(root);

        LevelProp Prop(string key, Vector2 position, bool visible)
        {
            var go = new GameObject(key, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(shelf, false);
            ((RectTransform)go.transform).anchoredPosition = position;
            var prop = go.AddComponent<LevelProp>();
            prop.key = key;
            prop.image = go.GetComponent<Image>();
            go.SetActive(visible);
            prop.CaptureStart();
            return prop;
        }

        [UnityTest]
        public IEnumerator Student_WalksUpASlope_AndStandsAtTheTopFacingFront()
        {
            yield return student.WalkTo(140f, 20f, PoseActor.FastWalkSpeed);
            Assert.That(student.X, Is.EqualTo(140f));
            Assert.That(student.Y, Is.EqualTo(20f));
            Assert.That(((RectTransform)student.transform).anchoredPosition, Is.EqualTo(new Vector2(560f, 240f)),
                "x 140 and 20 art pixels above the floor line are 560 and 160 + 80 UI units");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(student.FacingLeft, Is.False);
        }

        [UnityTest]
        public IEnumerator Student_WalkOnTheFlat_KeepsItsHeight()
        {
            student.PlaceAt(100f, 12f);
            yield return student.WalkTo(60f);
            Assert.That(student.X, Is.EqualTo(60f));
            Assert.That(student.Y, Is.EqualTo(12f));
        }

        [UnityTest]
        public IEnumerator Prop_PickedUpAndPutDown_GoesBackToItsStartPlace()
        {
            var box = Prop("box", new Vector2(40f, 0f), visible: true);
            box.PickUp(student.transform);
            Assert.That(box.transform.parent, Is.SameAs(student.transform), "the box moves with the Student");
            Assert.That(box.AtStart, Is.False);

            box.Place(new Vector2(300f, 120f));
            Assert.That(box.transform.parent, Is.SameAs(shelf));
            Assert.That(((RectTransform)box.transform).anchoredPosition, Is.EqualTo(new Vector2(300f, 120f)));

            yield return box.ResetToStart(0.2f);
            Assert.That(box.AtStart, Is.True);
            Assert.That(((RectTransform)box.transform).anchoredPosition, Is.EqualTo(new Vector2(40f, 0f)));
            Assert.That(box.image.color.a, Is.EqualTo(1f));
        }

        [UnityTest]
        public IEnumerator Prop_Slid_SlidesBack()
        {
            var bed = Prop("bed", new Vector2(200f, 0f), visible: true);
            yield return bed.Slide(24f, 0.2f);
            Assert.That(((RectTransform)bed.transform).anchoredPosition.x, Is.EqualTo(224f));
            yield return bed.ResetToStart(0.2f);
            Assert.That(bed.AtStart, Is.True);
        }

        [UnityTest]
        public IEnumerator Prop_HiddenAtStartAndShown_IsHiddenAgain()
        {
            var note = Prop("note", Vector2.zero, visible: false);
            note.Show();
            Assert.That(note.gameObject.activeSelf, Is.True);
            yield return note.ResetToStart(0.2f);
            Assert.That(note.gameObject.activeSelf, Is.False);
            Assert.That(note.AtStart, Is.True);
        }

        [Test]
        public void ActOut_StopsAfterItsStopStep_AndEndsAtItsLastMove()
        {
            var actOut = new ActOut
            {
                key = "shelf",
                stopAfter = 1,
                steps = new[]
                {
                    ActOutStep.Walk(150f),
                    ActOutStep.Pose(StudentPose.Crouch, 0.3f),
                    ActOutStep.Walk(220f),
                    ActOutStep.Pose(StudentPose.Hold),
                },
            };
            Assert.That(actOut.LastStep(stops: true), Is.EqualTo(1));
            Assert.That(actOut.LastStep(stops: false), Is.EqualTo(3));
            Assert.That(actOut.StopX(100f), Is.EqualTo(150f));
            Assert.That(actOut.EndX(100f), Is.EqualTo(220f));
            Assert.That(actOut.EndPose, Is.EqualTo(StudentPose.Hold));
            Assert.That(new ActOut { steps = new[] { ActOutStep.Wait(1f) } }.StopX(100f), Is.EqualTo(100f),
                "an Act-out that never moves stops at the start");
        }
    }
}
