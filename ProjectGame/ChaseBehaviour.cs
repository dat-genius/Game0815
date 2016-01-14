using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class ChaseBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Target { get; set; }
        public float Radius { get; set; }
        public bool Collision = false;

        private bool found = false;
        private Vector2 beginPosition;
        private float lerpFactor = 0.01f;
        private bool chasing = false;

        public ChaseBehaviour(float radius, GameObject target)
        {
            Radius = radius;
            Target = target;
        }

        public void OnUpdate(GameTime gameTime)
        {
            beginPosition = GameObject.Position;
            CheckFOVBehavior();
            if (found)
            {
                Chase();
            }
            ToggleMonsterBehavior(chasing);
        }

        public void OnMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.AreaEntered:
                    found = true;
                    break;
                case MessageType.AreaExited:
                    found = false;
                    break;
            }
            beginPosition = GameObject.Position;
        }

        public void CheckFOVBehavior()
        {
            if (!GameObject.HasBehaviourOfType(typeof(FOVBehavior)))
            {
                found = true;
            }
        }
        /// <summary>
        /// chacking agro radius
        /// </summary>
        public void Chase()
        {
            var positionDifference = Target.Position - beginPosition;
            if (positionDifference.Length() < Radius)
            {
                if (!Collision)
                {
                    GameObject.Position = Vector2.Lerp(beginPosition, Target.Position, lerpFactor);
                    GameObject.Rotation = (float)Math.Atan2(Target.Position.Y, Target.position.X);// +MathHelper.ToRadians(90);

                }
                chasing = true;
            }
            else
            {
                chasing = false;
            }
        }

        public void ToggleMonsterBehavior(bool chasing)
        {
            if (GameObject.HasBehaviourOfType(typeof(MonsterMovementBehaviour)) && chasing)
            {
                var behaviour = GameObject.GetBehaviourOfType(typeof(MonsterMovementBehaviour));
                GameObject.RemoveBehaviour(behaviour);



            }
            else
            {
                if (!GameObject.HasBehaviourOfType(typeof(MonsterMovementBehaviour)) && !chasing)
                {
                    GameObject.AddBehaviour(new MonsterMovementBehaviour());
                }
            }
            Collision = false;
        }
    }
}
