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

        private TimeSpan cooldown;
        private TimeSpan timeSindsUsage;

        public AttackBehaviour(GameObject sword)
        {
            Sword = sword;
            timeSindsUsage = TimeSpan.FromMilliseconds(cooldown.Milliseconds);
            if (Sword.HasBehaviourOfType(typeof(WeaponBehaviour)))
                BehaviourSword = Sword.GetBehaviourOfType(typeof(WeaponBehaviour));

        }

        public void OnUpdate(GameTime gameTime)
        {
            SetCooldown();
            timeSindsUsage += gameTime.ElapsedGameTime;
            
            if(timeSindsUsage >= cooldown && Attack)
            {
                (BehaviourSword as WeaponBehaviour).SwingSword = true;
            }
           
            Attack = false;
        }

        private void SetCooldown()
        {
            if (GameObject.HasBehaviourOfType(typeof(MonsterAttack)))
                cooldown = TimeSpan.FromMilliseconds(1050);
            else
                cooldown = TimeSpan.FromMilliseconds(700);
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
