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

        private Vector2 SpawnPoint;
        private Vector2 BeginPosition;

        private bool Found = false;
        private bool Chasing = false;
        private bool Boss;

        private float LerpFactor = 0.01f;
        private int Distance;

        public ChaseBehaviour(float radius, GameObject target, Vector2 spawn ,bool boss = false)
        {
            Radius = radius;
            Target = target;
            SpawnPoint = spawn;
            Boss = boss;
        }

        public void OnUpdate(GameTime gameTime)
        {
            BeginPosition = GameObject.Position;
            CheckFOVBehavior();
            if (Found)
            {
                Chase();
            }
            else 
            { 
                GetBack();
            }
        }

        public void OnMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.AreaEntered:
                    Found = true;
                    break;
                case MessageType.AreaExited:
                    Found = false;
                    break;
            }
            BeginPosition = GameObject.Position;
        }

        public void CheckFOVBehavior()
        {
            if (!GameObject.HasBehaviourOfType(typeof(FOVBehavior)))
            {
                Found = true;
            }
        }

        public void Chase()
        {
            CalculateDistance();
            var positionDifference = Target.Position - BeginPosition;
            if (positionDifference.Length() < Radius)
            {
                CaseOfNoCollision(positionDifference);
                Chasing = true;
            }
            else
            {
                Chasing = false;               
            }
        }

        private void CaseOfNoCollision(Vector2 positiondifference)
        {
            if (!Collision)
            {
                WalkToTarget(positiondifference);
                GameObject.Rotation = (float)Math.Atan2(Target.Position.Y - GameObject.Position.Y, Target.Position.X - GameObject.Position.X) + MathHelper.ToRadians(90);
            }
        }

        private void WalkToTarget(Vector2 difference)
        {
            if (difference.Length() > Distance)
            {
                GameObject.Position = Vector2.Lerp(BeginPosition, Target.Position, LerpFactor);
            }
        }

        private void GetBack()
        {
            if (!Chasing)
            {
                GameObject.Rotation = (float)Math.Atan2(SpawnPoint.Y, SpawnPoint.X) + MathHelper.ToRadians(90);
                GameObject.Position = Vector2.Lerp(BeginPosition, SpawnPoint, LerpFactor);
                
            }
        }

        private void CalculateDistance()
        {
            if (Boss)
                Distance = 150;
            else
                Distance = 90;
        }
    }
}
