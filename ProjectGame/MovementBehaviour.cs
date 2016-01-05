using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class MovementBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public Vector2 Velocity { get; set; }

        public bool CollisionTop = false;
        public bool CollisionBottom = false;
        public bool CollisionLeft = false;
        public bool CollisionRight = false;

        public void OnUpdate(GameTime gameTime)
        {
            if (Velocity.Length() > 0)
            {
                if (CollisionTop)
                {
                    if (Velocity.Y > 0)
                        GameObject.Position += Velocity;
                    GameObject.Position += new Vector2(Velocity.X, 0);
                }
                else if(CollisionBottom)
                {
                    if (Velocity.Y < 0)
                        GameObject.Position += Velocity;
                    GameObject.Position += new Vector2(Velocity.X, 0);
                }
                else if(CollisionLeft)
                {
                    if (Velocity.X > 0)
                        GameObject.Position += Velocity;
                    GameObject.Position += new Vector2(0, Velocity.Y);
                }
                else if(CollisionRight)
                {
                    if (Velocity.X < 0)
                        GameObject.Position += Velocity;
                    GameObject.Position += new Vector2(0, Velocity.Y);
                }
                else                
                    GameObject.Position += Velocity;
            }

            CollisionRight = false;
            CollisionTop = false;
            CollisionLeft = false;
            CollisionBottom = false;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}