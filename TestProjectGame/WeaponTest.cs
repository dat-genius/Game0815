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
            GameTime gameTime = new GameTime();
            MonsterSword.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = Monster
            });
            Monster.AddBehaviour(new MonsterAttack(Player));
            Monster.AddBehaviour(new AttackBehaviour(MonsterSword));
            Monster.AddBehaviour(new StatBehaviour(100, 100, 0.1f));
            Monster.Position = new Vector2(100, 100);
            Monster.Rotation = MathHelper.ToRadians(180);
            Player.Position = new Vector2(100, 130);

            Monster.OnUpdate(gameTime);
           

            var attackBehaviour = Monster.GetBehaviourOfType(typeof(AttackBehaviour));
            var monsterAttackBehaviour = Monster.GetBehaviourOfType(typeof(MonsterAttack));
            var weaponBehaviour = MonsterSword.GetBehaviourOfType(typeof(WeaponBehaviour));

            Assert.IsTrue((monsterAttackBehaviour as MonsterAttack).CheckRange());
            Assert.AreEqual(TimeSpan.FromMilliseconds(1050), (attackBehaviour as AttackBehaviour).Cooldown);
            Assert.IsTrue((weaponBehaviour as WeaponBehaviour).SwingSword);

            Player.Position = new Vector2(300, 300);

            Monster.OnUpdate(gameTime);
            MonsterSword.OnUpdate(gameTime);

            Assert.IsFalse((monsterAttackBehaviour as MonsterAttack).CheckRange());
            Assert.IsFalse((weaponBehaviour as WeaponBehaviour).SwingSword);
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
            Player.AddBehaviour(new StatBehaviour(100, 100, 0.1f));
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

