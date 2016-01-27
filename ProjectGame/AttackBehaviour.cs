using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class AttackBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Sword { get; set; }
        public IBehaviour BehaviourSword { get; set; }
        public bool Attack { get; set; }

        public TimeSpan Cooldown;
        private TimeSpan timeUntilUsage;

        public AttackBehaviour(GameObject sword)
        {
            Sword = sword;
            timeUntilUsage = TimeSpan.FromMilliseconds(0);
            if (Sword.HasBehaviourOfType(typeof(WeaponBehaviour)))
                BehaviourSword = Sword.GetBehaviourOfType(typeof(WeaponBehaviour));

        }

        public void OnUpdate(GameTime gameTime)
        {
            setCooldown();
            timeUntilUsage -= gameTime.ElapsedGameTime;

            if (allowHit(timeUntilUsage))
            {
                hit();
            }

            Attack = false;
        }

        public void OnMessage(IMessage message)
        {
        }

        private void setCooldown()
        {
            if (GameObject.HasBehaviourOfType(typeof(MonsterAttack)))
                setCooldownVar(1050);
            else
                setCooldownVar(700);
        }

        private bool allowHit(TimeSpan time)
        {
            return time.TotalMilliseconds <= 0 && Attack;
        }

        private void setCooldownVar(float i)
        {
            Cooldown = TimeSpan.FromMilliseconds(i);
        }

        private void hit()
        {
            (BehaviourSword as WeaponBehaviour).SwingSword = true;
            (GameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour).TestosDown(1);
            timeUntilUsage = Cooldown;
        }
    }
}
