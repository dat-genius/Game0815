using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame.Voorbeeld
{
    public class InputMovementBehaviourVB : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public float MovementSpeed { get; set; }
        public bool Collision = false;

        public InputMovementBehaviourVB(float movementSpeed)
        {
            MovementSpeed = movementSpeed;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (Collision)
                MovementSpeed = 0;
            else
                MovementSpeed = 5;
            
            var displacement = Vector2.Zero;
            displacement.Y -= Keyboard.GetState().IsKeyDown(Keys.W) ? 1 : 0;
            displacement.X -= Keyboard.GetState().IsKeyDown(Keys.A) ? 1 : 0;
            displacement.Y += Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;
            displacement.X += Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

            if (displacement.Length() > 0)
            {
                GameObject.Position += Vector2.Normalize(displacement) * MovementSpeed;
            }

            Collision = false;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}