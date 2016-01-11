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

        [TestMethod]
        public void Monsterkilled()
        {
            GameObject Player = new GameObject();
            GameObject Monster = new GameObject();
            GameObject Sword = new GameObject(false, false);
            GameTime gameTime = new GameTime();
            TimeSpan cooldown = TimeSpan.FromMilliseconds(800);
            TimeSpan timeSindsUsage = TimeSpan.FromMilliseconds(800);

            Monster.AddBehaviour(new MonsterMovementBehaviour());
            Player.AddBehaviour(new InputMovementBehaviour(5, new FollowCamera()));
            Sword.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = Player
            });

            Monster.Position = new Vector2(100, 100);
            Player.Position = new Vector2(100, 80);
           

            var weaponBehaviour = Sword.GetBehaviourOfType(typeof(WeaponBehaviour));
            var monsterMovementBehaviour = Monster.GetBehaviourOfType(typeof(MonsterMovementBehaviour));
            (monsterMovementBehaviour as MonsterMovementBehaviour).Collision = true;
            int Attack = 0;

            while(Attack < 5)
            {
                if (timeSindsUsage == cooldown)
                {
                    timeSindsUsage = TimeSpan.FromMilliseconds(0);
                    (weaponBehaviour as WeaponBehaviour).Weapontest = true;
                    Attack++;
                }
                (weaponBehaviour as WeaponBehaviour).OnUpdate(gameTime);
                (monsterMovementBehaviour as MonsterMovementBehaviour).OnUpdate(gameTime);

                timeSindsUsage += gameTime.ElapsedGameTime;
                (weaponBehaviour as WeaponBehaviour).Weapontest = false;
            }

            int Lives = (monsterMovementBehaviour as MonsterMovementBehaviour).Lives;
            Assert.AreEqual(0, Lives);
            Assert.IsFalse(Monster.IsDrawable);
        }
    }
}
