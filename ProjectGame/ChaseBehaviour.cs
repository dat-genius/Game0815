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

        private Vector2 beginPosition;
        private float lerpFactor = 0.01f;

        public ChaseBehaviour(float radius, GameObject target)
        {
            Radius = radius;
            Target = target;
        }

        public void OnUpdate(GameTime gameTime)
        {
            //beginPosition = GameObject.Position;
            //var positionDifference = Target.Position - beginPosition;
            //if (positionDifference.X >= -Radius && positionDifference.X <= Radius || positionDifference.Y >= -Radius && positionDifference.Y <= Radius)
            //{
            //    GameObject.Position = Vector2.Lerp(beginPosition, Target.Position, lerpFactor);
            //}
        }

        public void OnMessage(IMessage message)
        {
            // OnBeginChaseMessage:
            // beginPosition = GameObject.Position;
        }
    }
}
