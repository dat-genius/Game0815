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

            if (chasing)
            {
                ToggleMonsterBehavior();
            }
        }

        public void OnMessage(IMessage message)
        {
            if (message.GetType() == typeof(PlayerEnterFoVMessage))
            {
                found = true;
            }
            if (message.GetType() == typeof(PlayerFovMessage))
            {
                found = false;
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

        public void Chase()
        {
            var positionDifference = Target.Position - beginPosition;
            if (positionDifference.Length() < Radius)
            {
                if (!Collision)
                {
                    GameObject.Position = Vector2.Lerp(beginPosition, Target.Position, lerpFactor);
                }
                chasing = true;
            }
            else
            {
                chasing = false;
            }
        }

        public void ToggleMonsterBehavior()
        {
            var positionDifference = Target.Position - beginPosition;
            if (positionDifference.Length() < Radius)
            {
                if (!Collision)
                {
                    GameObject.Position = Vector2.Lerp(beginPosition, Target.Position, lerpFactor);
        }
                chasing = true;
            }
            else 
            {
                chasing = false;
            }
            Collision = false;
        }
    }
}
