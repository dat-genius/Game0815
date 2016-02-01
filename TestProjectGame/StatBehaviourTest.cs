using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using ProjectGame;


namespace TestProjectGame
{
    /// <summary>
    /// Summary description for StatBehaviourTest
    /// </summary>
    [TestClass]
    public class StatBehaviourTest
    {
        private GameObject player;
        private StatBehaviour Stats;

        [TestInitialize]
        public void initialize()
        {
            player = new GameObject();
            player.AddBehaviour("StatBehaviour",new StatBehaviour(100, 100, 1));

            Stats = player.GetBehaviourOfType("StatBehaviour") as StatBehaviour;
        }

        [TestMethod]
        public void TestStatDecl()
        {
            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(100, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatHealthDownLowDamage()
        {
            Stats.HealthDown(20);

            Assert.AreEqual(80, Stats.Health);
            Assert.AreEqual(100, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatHealthDownHighDamage()
        {
            Stats.HealthDown(50);

            Assert.AreEqual(50, Stats.Health);
            Assert.AreEqual(100, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }


        [TestMethod]
        public void TestStatTestosDown()
        {
            Stats.TestosDown(1);

            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(80, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatTestosRegen()
        {
            Stats.Regen = true;

            Stats.TestosDown(1);

            Stats.OnUpdate(new GameTime());

            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(81, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }

        [TestMethod]
        public void TestStatTestosRegenCap()
        {

            Stats.Regen = true;

            Stats.TestosDown(1);

            for (int i = 0; i < 30; i++)
                Stats.OnUpdate(new GameTime());

            Assert.AreEqual(100, Stats.Health);
            Assert.AreEqual(100, Stats.Testos);
            Assert.AreEqual(1, Stats.RegenSpeed);
        }
    }
}
