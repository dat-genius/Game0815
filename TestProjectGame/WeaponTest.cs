using System;
using ProjectGame;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Input;

namespace TestProjectGame
{
    [TestClass]
    public class WeaponTest
    {
        [TestMethod]
        public void TestMonsterAttack()
        {
            GameObject Player = new GameObject();
            GameObject Monster = new GameObject();
            GameObject MonsterSword = new GameObject(false, false);
            MonsterSword.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = Monster
            });
            Monster.AddBehaviour(new MonsterAttack(Player));
            Monster.AddBehaviour(new AttackBehaviour(MonsterSword));
            Monster.Position = new Vector2(100, 100);
            Player.Position = new Vector2(100, 140);

            Monster.OnUpdate(new GameTime());

            var attackBehaviour = Monster.GetBehaviourOfType(typeof(AttackBehaviour));
            var monsterAttackBehaviour = Monster.GetBehaviourOfType(typeof(MonsterAttack));
            var weaponBehaviour = MonsterSword.GetBehaviourOfType(typeof(WeaponBehaviour));

            Assert.IsTrue((monsterAttackBehaviour as MonsterAttack).CheckRange());
            Assert.AreEqual(TimeSpan.FromMilliseconds(1050), (attackBehaviour as AttackBehaviour).cooldown);
            Assert.IsTrue((weaponBehaviour as WeaponBehaviour).SwingSword);
        }

        [TestMethod]
        public void TestPlayerSwing()
        {
            GameObject Player = new GameObject();
            GameObject PlayerSword = new GameObject(false, false);
            PlayerSword.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = Player
            });
            bool Clicked = true;
            Player.AddBehaviour(new InputMovementBehaviour(5f, new FollowCamera()));
            Player.AddBehaviour(new AttackBehaviour(PlayerSword));
            var inputBehaviour = Player.GetBehaviourOfType(typeof(InputMovementBehaviour));
            var weaponBehaviour = PlayerSword.GetBehaviourOfType(typeof(WeaponBehaviour));
            var attackBehaviour = Player.GetBehaviourOfType(typeof(AttackBehaviour));

            if (Clicked)
            {
                (inputBehaviour as InputMovementBehaviour).SwingSword();
            }
            Assert.IsTrue((attackBehaviour as AttackBehaviour).Attack);

            Player.OnUpdate(new GameTime());


            Assert.AreEqual(TimeSpan.FromMilliseconds(700), (attackBehaviour as AttackBehaviour).cooldown);
            Assert.IsTrue((weaponBehaviour as WeaponBehaviour).SwingSword);
        }
    }

}

