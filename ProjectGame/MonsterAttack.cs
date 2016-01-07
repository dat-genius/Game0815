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
            cooldown = TimeSpan.FromMilliseconds(800);
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
                    TurnToPLayer();
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

        public void TurnToPLayer()
        {
            var gameObject = new Rectangle((int)GameObject.Position.X, (int)GameObject.Position.Y, GameObject.Size.X, GameObject.Size.Y);
            var target = new Rectangle((int)Target.Position.X, (int)Target.Position.Y, Target.Size.X, Target.Size.Y);

            int position =  PlaceOfPLayer(gameObject, target);

            switch (position)
            {
                case 1: GameObject.Rotation = MathHelper.ToRadians(270); break;     //links
                case 2: GameObject.Rotation = MathHelper.ToRadians(90); break;      //rechts
                case 3: GameObject.Rotation = MathHelper.ToRadians(0); break;       // boven
                case 4: GameObject.Rotation = MathHelper.ToRadians(180); break;     //onder
            }
        }

        private int PlaceOfPLayer(Rectangle a, Rectangle b)
        {
            var MidAx = (a.Right + a.Left) / 2;
            var MidAy = (a.Top + a.Bottom) / 2;

            if (b.Right < MidAx)
                return 1;
            if (b.Left > MidAx)
                return 2;
            if (b.Bottom < MidAy)
                return 3;
            if (b.Top > MidAy)
                return 4;

            return 0;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
