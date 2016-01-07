using System;
using ProjectGame;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjectGame
{
    [TestClass]
    public class FOVBehaviourTest
    {
        [TestInitialize]
        

        [TestMethod]
        public void TestHasFOVBehaviour()
        {
            GameObject testObject = new GameObject();
            Assert.IsFalse(testObject.HasBehaviourOfType(typeof(FOVBehavior)));
            testObject.AddBehaviour(new FOVBehavior());
            Assert.IsTrue(testObject.HasBehaviourOfType(typeof(FOVBehavior)));
        }

        [TestMethod]
        public void TestFind1()
        {
            GameObject testObject = new GameObject();
            testObject.AddBehaviour(new FOVBehavior());
            testObject.Rotation = 0;
            testObject.Position = new Vector2(100, 100);
            GameObject testPlayerObject = new GameObject();
            testPlayerObject.AddBehaviour(new InputMovementBehaviour(5, new FollowCamera()));
            testPlayerObject.Position = new Vector2(100, 200);
            //testObject.OnUpdate();
            
            
        }
    }
}
