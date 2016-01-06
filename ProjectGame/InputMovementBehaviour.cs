using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class InputMovementBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public float MovementSpeed { get; set; }
        private MovementBehaviour movementBehaviour = null;
        public Vector2 whereMouseAt;
        
        public bool CollisionTop = false;
        public bool CollisionBottom = false;
        public bool CollisionLeft = false;
        public bool CollisionRight= false;

        public InputMovementBehaviour(float movementSpeed)
        {
            MovementSpeed = movementSpeed;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (movementBehaviour == null)
            {
                var behaviour = GameObject.GetBehaviourOfType(typeof (MovementBehaviour));
                if (behaviour != null)
                {
                    movementBehaviour = behaviour as MovementBehaviour;
                }
                else return;
            }

            var displacement = Vector2.Zero;
            if(!CollisionTop)
                displacement.Y -= Keyboard.GetState().IsKeyDown(Keys.W) ? 1 : 0;
            if(!CollisionLeft)
                displacement.X -= Keyboard.GetState().IsKeyDown(Keys.A) ? 1 : 0;
            if(!CollisionBottom)
                displacement.Y += Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;
            if(!CollisionRight)
                displacement.X += Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

            // Rotate player

            MouseState mouse = Mouse.GetState();
            whereMouseAt.Y = mouse.Y - GameObject.Position.Y;
            whereMouseAt.X = mouse.X - GameObject.Position.X;

            GameObject.Rotation = (float)Math.Atan2(whereMouseAt.Y, whereMouseAt.X) + MathHelper.ToRadians(90);

            /*
            if (displacement.Length() > 0)
            {
                var dotProduct = Vector2.Dot(new Vector2(0, -1), Vector2.Normalize(displacement));
                var angleRadians = (float)Math.Acos(dotProduct);
                var angleDegrees = MathHelper.ToDegrees(angleRadians);
                if (displacement.X < 0)
                    angleDegrees = 360 - angleDegrees;
                angleRadians = MathHelper.ToRadians(angleDegrees);

                GameObject.Rotation = angleRadians;
            }
            */
            movementBehaviour.Velocity = Vector2.Normalize(displacement)*MovementSpeed;
            
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