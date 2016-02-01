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
            Assert.IsFalse(testObject.HasBehaviourOfType("FOVBehavior"));
            testObject.AddBehaviour("FOVBehaviour",new FOVBehavior(gameObjects));
            Assert.IsTrue(testObject.HasBehaviourOfType("FOVBehavior"));
        }

        [TestMethod]
        public void TestFindNoPlayer()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testObject.AddBehaviour("FOVBehaviour", testFOV);
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
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testObject.AddBehaviour("FOVBehaviour", testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour("InputMovementBehaviour", new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(100, 200);

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
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testFOV.ViewDistance = 300;
            testObject.AddBehaviour("FOVBehaviour", testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour("InputMovementBehaviour",new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(100, 0);

            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            testObject.CollidingGameObjects = gameObjects;
            testObject.OnUpdate(time);
            Assert.IsTrue(testFOV.DetectPlayer());
        }

        [TestMethod]
        public void TestEdgeFindTrue()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testObject.AddBehaviour("FOVBehaviour", testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            testFOV.ViewDistance = 100;
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour("InputMovementBehaviour",new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(51, 50);
            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            Assert.IsTrue(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(100, 1);
            Assert.IsTrue(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(100, 99);
            Assert.IsTrue(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(149, 50);
            Assert.IsTrue(testFOV.DetectPlayer());
        }

        [TestMethod]
        public void TestEdgeFindFalse()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testObject.AddBehaviour("FOVBehaviour", testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            testFOV.ViewDistance = 100;
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour("InputMovementBehaviour",new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(50, 50);
            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            Assert.IsFalse(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(100, 100);
            Assert.IsFalse(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(150, 50);
            Assert.IsFalse(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(100, 0);
        }

        [TestMethod]
        public void TestCornerFindTrue()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testObject.AddBehaviour("FOVBehaviour", testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            testFOV.ViewDistance = 100;
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour("InputMovementBehaviour", new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(51, 1);
            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            Assert.IsTrue(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(51, 99);
            Assert.IsTrue(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(149, 99);
            Assert.IsTrue(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(149, 1);
            Assert.IsTrue(testFOV.DetectPlayer());
        }

        [TestMethod]
        public void TestCornerFindFalse()
        {
            GameObject testObject = new GameObject();
            FOVBehavior testFOV = new FOVBehavior(gameObjects);
            testObject.AddBehaviour("FOVBehaviour", testFOV);
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            testFOV.ViewDistance = 100;
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour("InputMovementBehaviour", new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(50, 0);
            gameObjects.Add(testObject);
            gameObjects.Add(testPlayerObject);
            Assert.IsFalse(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(50, 100);
            Assert.IsFalse(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(150, 100);
            Assert.IsFalse(testFOV.DetectPlayer());
            testPlayerObject.Position = new Vector2(150, 0);
        }
    }
}