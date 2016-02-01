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


        public Vector2 SpawnPoint { get; set; }
        private Vector2 beginPosition;

        private bool found = false;
        private bool chasing = false;
        private bool boss;

        private float lerpFactor = 0.01f;
        private int distance;

        public ChaseBehaviour(float radius, GameObject target, Vector2 spawn , bool _boss = false)
        {
            Radius = radius;
            Target = target;
            SpawnPoint = spawn;
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
            else 
            { 
                getBack();
            }
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

        private void CheckFOVBehavior()
        {
            if (!GameObject.HasBehaviourOfType("FOVBehavior"))
            {
                found = true;
            }
        }

        public void Chase()
        {
            Vector2 positionDifference = Target.Position - beginPosition;

            if (boss)
                setDistance(140);
            else
                setDistance(90);

            if (positionDifference.Length() < Radius)
            {
                caseOfNoCollision(positionDifference);
            }
            else
            {
                chasing = false;               
            }
        }

        private void caseOfNoCollision(Vector2 positiondifference)
        {
            if (!Collision)
            {
                walkToTarget(positiondifference);
            }
            chasing = true;
        }

        private void walkToTarget(Vector2 difference)
        {
            if (difference.Length() > distance)
            {
                GameObject.Position = Vector2.Lerp(beginPosition, Target.Position, lerpFactor);
            }
            GameObject.Rotation = (float)Math.Atan2(Target.Position.Y - GameObject.Position.Y, Target.Position.X - GameObject.Position.X) + MathHelper.ToRadians(90);
        }

        private void getBack()
        {
            if (!chasing)
            {

                GameObject.Rotation = (float)Math.Atan2(SpawnPoint.Y, SpawnPoint.X) + MathHelper.ToRadians(90);
                GameObject.Position = Vector2.Lerp(beginPosition, SpawnPoint, lerpFactor);

            }
        }

        private void setDistance(int dist)
        {
                distance = dist;
        }
    }
}
