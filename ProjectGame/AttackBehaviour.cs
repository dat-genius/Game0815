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

        public TimeSpan cooldown;
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
            SetCooldown();
            timeUntilUsage -= gameTime.ElapsedGameTime;

            if (AllowHit(timeUntilUsage))
            {
                Hit();
            }

            Attack = false;
        }

        public void OnMessage(IMessage message)
        {
        }

        private bool AllowHit(TimeSpan time)
        {
            return time.TotalMilliseconds <= 0 && Attack;
        }

        private void Hit()
        {
            (BehaviourSword as WeaponBehaviour).SwingSword = true;
            (GameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour).TestosDown(1);
            timeUntilUsage = cooldown;
        }

        private void SetCooldown()
        {
            if (GameObject.HasBehaviourOfType(typeof(MonsterAttack)))
                SetCooldownVar(1050);
            else
                SetCooldownVar(700);
        }

        private void SetCooldownVar(float i)
        {
            cooldown = TimeSpan.FromMilliseconds(i);
        }
    }
}
