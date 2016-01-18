using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectGame;
using Microsoft.Xna.Framework;

namespace TestProjectGame
{
    /// <summary>
    /// Summary description for StatBehaviourTest
    /// </summary>
    [TestClass]
    public class StatBehaviourTest
    {

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestStatDecl()
        {
            var player = new GameObject();
            player.AddBehaviour(new StatBehaviour(100,100,1));

            var Stats = player.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(100, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatHealthDown()
        {
            var player = new GameObject();
            player.AddBehaviour(new StatBehaviour(100, 100, 1));

            var Stats = player.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            Stats.HealthDown(20);

            Assert.AreEqual(80, Stats.Health);
            Assert.AreEqual(100, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatTestosDown()
        {
            var player = new GameObject();
            player.AddBehaviour(new StatBehaviour(100, 100, 1));

            var Stats = player.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            Stats.TestosDown(1);

            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(80, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatTestosRegen()
        {
            var player = new GameObject();
            player.AddBehaviour(new StatBehaviour(100, 100, 1));

            var Stats = player.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            Stats.TestosDown(1);

            Stats.Regen = true;

            Stats.OnUpdate(new GameTime());

            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(81, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }
    }
}
