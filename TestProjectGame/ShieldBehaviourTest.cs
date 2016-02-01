using System;
using ProjectGame;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Input;

namespace TestProjectGame
{
    [TestClass]
    public class ShieldBehaviourTest
    {
        private GameObject player;
        private ShieldBehaviour shieldBehaviour;
        private StatBehaviour statBehaviour;
        private HitBehaviour hitBehaviour;

        [TestInitialize]
        public void initialize()
        {
            player = new GameObject(true, true);

            shieldBehaviour = new ShieldBehaviour(null);
            player.AddBehaviour("ShieldBehaviour",shieldBehaviour);

            statBehaviour = new StatBehaviour(100, 100, 0);
            player.AddBehaviour("StatBehaviour",statBehaviour);

            hitBehaviour = new HitBehaviour(null);
            player.AddBehaviour("HitBehaviour",hitBehaviour);
        }

        [TestMethod]
        public void TestAllowDraw()
        {
            initialize();

            Assert.IsTrue(shieldBehaviour.AllowDraw());

            (player.GetBehaviourOfType("StatBehaviour") as StatBehaviour).Testos = 0;

            Assert.IsFalse(shieldBehaviour.AllowDraw());
        }

        [TestMethod]
        public void TestMouseForDefend()
        {
            initialize();

            shieldBehaviour.defend = false;

            Assert.IsFalse(shieldBehaviour.CheckToDefend());

            shieldBehaviour.defend = true;

            Assert.IsTrue(shieldBehaviour.CheckToDefend());
        }

        [TestMethod]
        public void TestDefendNow()
        {
            Assert.AreEqual(100.0f, statBehaviour.Testos);
            Assert.IsFalse(shieldBehaviour.shield.IsDrawable);
            Assert.IsFalse(hitBehaviour.defend);

            shieldBehaviour.DefendNow();

            Assert.AreEqual(99.5f, statBehaviour.Testos);
            Assert.IsTrue(shieldBehaviour.shield.IsDrawable);
            Assert.IsTrue(hitBehaviour.defend);
        }
    }
}
