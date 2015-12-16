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
        private GameObject Player;
        private float Radius;
        private float lerpFactor = 0.01f;

        public ChaseBehaviour(float radius, GameObject player)
        {
            Radius = radius;
            Player = player;
        }

        public void OnUpdate(GameTime gameTime)
        {
            Vector2 beginPosition = GameObject.Position;
            Vector2 positionDifference = beginPosition - (Player.Position);

            if (positionDifference.X >= -Radius && positionDifference.X <= Radius || positionDifference.Y >= -Radius && positionDifference.Y <= Radius)
            {
                GameObject.Position = Vector2.Lerp(beginPosition, Player.Position, lerpFactor);
            }
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
