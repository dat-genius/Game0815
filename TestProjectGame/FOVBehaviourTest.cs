using System;
using System.Collections.Generic;
using ProjectGame;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjectGame
{
    [TestClass]
    public class FOVBehaviourTest
    {
        private List<GameObject> gameObjects;
        private GameObject testObject;
        private GameTime time;

        [TestInitialize]
        public void initialize()
        {
            time = new GameTime();
            gameObjects = new List<GameObject>();
            testObject = new GameObject();
        }

        [TestMethod]
        public void TestHasFOVBehaviour()
        {
            Assert.IsFalse(testObject.HasBehaviourOfType(typeof(FOVBehavior)));
            testObject.AddBehaviour(new FOVBehavior());
            Assert.IsTrue(testObject.HasBehaviourOfType(typeof(FOVBehavior)));
        }

        [TestMethod]
        public void TestFindNoPlayer()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior();
            testObject.AddBehaviour(testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            gameObjects.Add(testObject);
            testObject.CollidingGameObjects = gameObjects;
            testObject.OnUpdate(time);
            Assert.IsFalse(testFOV.DetectPlayer());

        }
        [TestMethod]
        public void TestFindOutsideOfBox()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior();
            testObject.AddBehaviour(testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour(new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(100, -200);

            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            testObject.CollidingGameObjects = gameObjects;
            testObject.OnUpdate(time);
            Assert.IsFalse(testFOV.DetectPlayer());

        }

        [TestMethod]
        public void TestFindTrue()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior();
            testObject.AddBehaviour(testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour(new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(100, 200);

            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            testObject.CollidingGameObjects = gameObjects;
            testObject.OnUpdate(time);
            FOVBehavior test = (FOVBehavior)testObject.GetBehaviourOfType(typeof(FOVBehavior));
            Assert.IsTrue(test.DetectPlayer());

        }
    }
}