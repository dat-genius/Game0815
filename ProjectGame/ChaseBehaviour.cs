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

        private Vector2 spawnPoint;
        private bool found = false;
        private Vector2 beginPosition;
        private float lerpFactor = 0.01f;
        private bool chasing = false;
        private bool boss;
        private int distance;

        public ChaseBehaviour(float radius, GameObject target, Vector2 spawn ,bool _boss = false)
        {
            Radius = radius;
            Target = target;
            spawnPoint = spawn;
            boss = _boss;
        }

        public void OnUpdate(GameTime gameTime)
        {
            beginPosition = GameObject.Position;
            CheckFOVBehavior();
            if (found)
            {
                Chase();
            }
            else { GetBack(); }
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
            CalculateDistance();
            var positionDifference = Target.Position - beginPosition;
            if (positionDifference.Length() < Radius)
            {
                if (!Collision)
                {
                    if (positionDifference.Length() > distance)
                    {
                        GameObject.Position = Vector2.Lerp(beginPosition, Target.Position, lerpFactor);
                    }                                      
                    GameObject.Rotation = (float)Math.Atan2(Target.Position.Y - GameObject.Position.Y , Target.Position.X - GameObject.Position.X) +MathHelper.ToRadians(90);

                }
                chasing = true;
            }
            else
            {               
                chasing = false;               
            }
        }

        public void GetBack()
        {
            if (chasing == false)
            {
                GameObject.Position = Vector2.Lerp(beginPosition, spawnPoint, lerpFactor);
            }
        }

        private void CalculateDistance()
        {
            if (boss)
                distance =  150;
            else
                distance =  90;
        }
    }
}
