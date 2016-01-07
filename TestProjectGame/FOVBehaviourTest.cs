using System;
using ProjectGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjectGame
{
    [TestClass]
    public class FOVBehaviourTest
    {
        [TestMethod]
        public void SetViewDistanceTest()
        {
            GameObject testObject = new GameObject();
            testObject.AddBehaviour(new FOVBehavior());
            Assert.IsTrue(testObject.HasBehaviourOfType(typeof(FOVBehavior)));
        }
    }
}
