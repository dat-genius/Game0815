﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame.Voorbeeld
{
    public class InputMovementBehaviourVB : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public float MovementSpeed { get; set; }
        public bool CollisionTop = false;
        public bool CollisionBottom = false;
        public bool CollisionLeft = false;
        public bool CollisionRight= false;

        public InputMovementBehaviourVB(float movementSpeed)
        {
            MovementSpeed = movementSpeed;
        }

        public void OnUpdate(GameTime gameTime)
        {
        
            
            var displacement = Vector2.Zero;
            if(!CollisionTop)
                displacement.Y -= Keyboard.GetState().IsKeyDown(Keys.W) ? 1 : 0;
            if(!CollisionLeft)
                displacement.X -= Keyboard.GetState().IsKeyDown(Keys.A) ? 1 : 0;
            if(!CollisionBottom)
                displacement.Y += Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;
            if(!CollisionRight)
                displacement.X += Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

            if (displacement.Length() > 0)
            {
                GameObject.Position += Vector2.Normalize(displacement) * MovementSpeed;

                var dotProduct = Vector2.Dot(new Vector2(0, -1), Vector2.Normalize(displacement));
                var angleRadians = (float)Math.Acos(dotProduct);
                var angleDegrees = MathHelper.ToDegrees(angleRadians);
                if (displacement.X < 0)
                    angleDegrees = 360 - angleDegrees;
                angleRadians = MathHelper.ToRadians(angleDegrees);
                GameObject.Rotation = angleRadians;
            }

            CollisionLeft = false;
            CollisionRight = false;
            CollisionTop = false;
            CollisionBottom = false;
            
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}