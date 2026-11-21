using System.Collections;
using DisasterSafety.Game.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DisasterSafety.Tests
{
    /// <summary>
    /// Grown-ups on a small test Setting built in the test: a Grown-up walks, poses, turns and goes back
    /// to its start place and pose, and the GrownUp Act-out Steps describe those actions.
    /// </summary>
    public sealed class GrownUpPlayTests
    {
        GameObject root;
        GrownUpActor mom;
        LevelSetting setting;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("Test Setting", typeof(RectTransform), typeof(Canvas));
            setting = root.AddComponent<LevelSetting>();
            var sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);
            var frames = new[] { sprite, sprite };

            var go = new GameObject("Mom", typeof(RectTransform));
            go.transform.SetParent(root.transform, false);
            mom = go.AddComponent<GrownUpActor>();
            mom.kind = GrownUpKind.Mom;
            mom.frames = new GrownUpFrames { idle = frames, wave = frames, walk = frames, happy = frames, sit = frames, hurtSit = frames, kneel = frames };
            mom.unitsPerPixel = 4f;
            mom.feetY = 160f;
            var body = new GameObject("Body", typeof(RectTransform), typeof(Image));
            body.transform.SetParent(go.transform, false);
            mom.body = body.GetComponent<Image>();
            mom.startX = 300f;
            mom.startY = 0f;
            mom.startPose = GrownUpPose.HurtSit;
            mom.startFacingLeft = true;
            setting.grownUps = new[] { mom };
            mom.PlaceAtStart();
        }

        [TearDown]
        public void TearDown() => Object.Destroy(root);

        [Test]
        public void GrownUp_StartsAtItsStartPlaceAndPose()
        {
            Assert.That(mom.X, Is.EqualTo(300f));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.HurtSit));
            Assert.That(mom.FacingLeft, Is.True);
            Assert.That(mom.AtStart, Is.True);
            Assert.That(((RectTransform)mom.transform).anchoredPosition, Is.EqualTo(new Vector2(1200f, 160f)));
        }

        [UnityTest]
        public IEnumerator GrownUp_Walks_AndStandsIdleFacingTheFront()
        {
            var walk = mom.WalkTo(260f, 0f);
            var routine = StartCoroutineOn(walk);
            yield return null;
            yield return null;
            Assert.That(mom.IsWalking, Is.True, "still on the way");
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Walk));
            Assert.That(mom.FacingLeft, Is.True, "walking to the left faces left");
            yield return routine;
            Assert.That(mom.X, Is.EqualTo(260f));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Idle));
            Assert.That(mom.FacingLeft, Is.False);
            Assert.That(mom.AtStart, Is.False);
        }

        Coroutine StartCoroutineOn(IEnumerator routine) => mom.StartCoroutine(routine);

        [Test]
        public void GrownUp_PlaysAPoseAndTurns()
        {
            mom.Play(GrownUpPose.Happy);
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Happy));
            mom.Face(false);
            Assert.That(mom.FacingLeft, Is.False);
            Assert.That(mom.body.rectTransform.localScale.x, Is.EqualTo(1f));
            mom.Face(true);
            Assert.That(mom.body.rectTransform.localScale.x, Is.EqualTo(-1f));
        }

        [Test]
        public void GrownUp_WithoutIdleFrames_StandsOnAWalkFrame()
        {
            var sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);
            var dad = new GrownUpFrames { walk = new[] { sprite, sprite, sprite, sprite }, kneel = new[] { sprite, sprite } };
            Assert.That(dad.For(GrownUpPose.Idle), Is.Not.Null.And.Length.EqualTo(1));
            Assert.That(dad.For(GrownUpPose.Kneel).Length, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator GrownUp_AfterWalkingAndPosing_ReturnsToItsStartPlaceAndPose()
        {
            yield return mom.WalkTo(200f, 0f);
            mom.Play(GrownUpPose.Wave);
            Assert.That(mom.AtStart, Is.False);

            yield return mom.ReturnToStart();
            Assert.That(mom.X, Is.EqualTo(300f));
            Assert.That(mom.Y, Is.EqualTo(0f));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.HurtSit));
            Assert.That(mom.FacingLeft, Is.True);
            Assert.That(mom.AtStart, Is.True);
        }

        [Test]
        public void ActOutSteps_DescribeWhatTheGrownUpDoes()
        {
            var walk = ActOutStep.GrownUpWalk(mom, 220f);
            var pose = ActOutStep.GrownUpPoseStep(mom, GrownUpPose.Kneel, 0.5f);
            var turn = ActOutStep.GrownUpTurn(mom, left: true);
            foreach (var step in new[] { walk, pose, turn })
            {
                Assert.That(step.kind, Is.EqualTo(ActOutStepKind.GrownUp));
                Assert.That(step.grownUp, Is.SameAs(mom));
                Assert.That(step.Moves, Is.False, "a Grown-up step never moves the Student");
            }
            Assert.That(walk.grownUpAction, Is.EqualTo(GrownUpAction.Walk));
            Assert.That(walk.x, Is.EqualTo(220f));
            Assert.That(pose.grownUpPose, Is.EqualTo(GrownUpPose.Kneel));
            Assert.That(pose.seconds, Is.EqualTo(0.5f));
            Assert.That(turn.faceLeft, Is.True);

            var actOut = new ActOut { key = "call", steps = new[] { ActOutStep.Walk(120f), walk, pose } };
            Assert.That(actOut.EndX(100f), Is.EqualTo(120f), "the Student's end place ignores the Grown-up's walk");
        }
    }
}
