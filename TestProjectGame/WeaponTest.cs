using System;
using ProjectGame;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjectGame
{
    [TestClass]
    public class WeaponTest
    {
        [TestMethod]
        public void MonsterAttack()
        {
            var Player = new GameObject();
            var Monster = new GameObject();
            var swordMonster = new GameObject();

            swordMonster.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = Player
            });

            Monster.AddBehaviour(new MonsterAttack(Player, swordMonster));
            Monster.AddBehaviour(new MonsterMovementBehaviour());

            Monster.Position = new Vector2(100, 100);
            Player.Position = new Vector2(100, 80);

            var behaviourAttack = Monster.GetBehaviourOfType(typeof(MonsterAttack));
            var behaviourWeapon = swordMonster.GetBehaviourOfType(typeof(WeaponBehaviour));
            (behaviourAttack as MonsterAttack).OnUpdate(new GameTime());

            Assert.IsTrue((behaviourWeapon as WeaponBehaviour).BotAttack);
            Assert.IsTrue(swordMonster.IsDrawable); 
        }
    }
}
