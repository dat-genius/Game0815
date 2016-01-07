using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class MonsterAttack : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Target { get; set; }
        public GameObject Sword { get; set; }

        private TimeSpan timeUntilUsage;
        private Vector2 beginPosition;
        private readonly TimeSpan cooldown;
        private readonly float radius;

        public MonsterAttack(GameObject target, GameObject sword)
        {
            Target = target;
            Sword = sword;
            cooldown = TimeSpan.FromMilliseconds(600);
            radius = 100f;
        }

        public void OnUpdate(GameTime gameTime)
        {
            timeUntilUsage -= gameTime.ElapsedGameTime;
            beginPosition = GameObject.Position;

            if (timeUntilUsage.TotalSeconds <= 0)
            {
                var positionDifference = Target.Position - beginPosition;
                if (positionDifference.Length() < radius)
                {
                    AttackPlayer();
                    timeUntilUsage = cooldown;
                }
            }


        }

        public void AttackPlayer()
        {
            if (GameObject.IsDrawable)
            {
                if (!Sword.HasBehaviourOfType(typeof(WeaponBehaviour))) return;
                var weaponBehaviour = Sword.GetBehaviourOfType(typeof(WeaponBehaviour));
                (weaponBehaviour as WeaponBehaviour).BotAttack = true;
            }
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
