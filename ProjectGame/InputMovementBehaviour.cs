using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class InputMovementBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public float MovementSpeed { get; set; }
        private float sprintSpeed;
        private MovementBehaviour movementBehaviour = null;
        private ICamera camera = null;

        public InputMovementBehaviour(float movementSpeed, ICamera camera)
        {
            MovementSpeed = movementSpeed;
            sprintSpeed = movementSpeed * 1.5f;
            this.camera = camera;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (camera == null)
                return;

            if (movementBehaviour == null)
            {
                var behaviour = GameObject.GetBehaviourOfType("MovementBehaviour");
                if (behaviour != null)
                {
                    movementBehaviour = behaviour as MovementBehaviour;
                }
                else return;
            }    

            var displacement = Vector2.Zero;
            displacement.Y -= Keyboard.GetState().IsKeyDown(Keys.W) ? 1 : 0;
            displacement.X -= Keyboard.GetState().IsKeyDown(Keys.A) ? 1 : 0;
            displacement.Y += Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;
            displacement.X += Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;

            // Rotate player
            MouseState mouse = Mouse.GetState();
            Vector2 whereMouseAt = new Vector2(mouse.X - GameObject.Position.X, mouse.Y - GameObject.Position.Y);
            whereMouseAt += camera.Position;
            whereMouseAt -= new Vector2(400, 240);
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

            
            // Moet nog wel testosterone van afgehaald worden
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && (GameObject.GetBehaviourOfType("StatBehaviour") as StatBehaviour).Testos > 0 && displacement.Length() > 0)
            {
                movementBehaviour.Velocity = Vector2.Normalize(displacement) * sprintSpeed;
                (GameObject.GetBehaviourOfType("StatBehaviour") as StatBehaviour).TestosDown(2);
            }
            else
                movementBehaviour.Velocity = Vector2.Normalize(displacement) * MovementSpeed;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && (GameObject.GetBehaviourOfType("StatBehaviour") as StatBehaviour).Testos > 0)
                SwingSword();

            Defend();

        }

        public void SwingSword()
        {
            if (!GameObject.HasBehaviourOfType("AttackBehaviour")) return;
            var behaviour = GameObject.GetBehaviourOfType("AttackBehaviour");
            (behaviour as AttackBehaviour).Attack = true;
        }

        public void Defend()
        {
            if (!GameObject.HasBehaviourOfType("ShieldBehaviour")) return;
            var behaviour = GameObject.GetBehaviourOfType("ShieldBehaviour") as ShieldBehaviour;

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                behaviour.defend = true;
            else
                behaviour.defend = false;

        }

        public void OnMessage(IMessage message)
        {
        } 
    }
}