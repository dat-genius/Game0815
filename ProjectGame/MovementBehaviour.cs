using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class MovementBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public Vector2 Velocity { get; set; }

        public void OnUpdate(GameTime gameTime)
        {
            if (Velocity.Length() > 0)
            {
                GameObject.Position += Velocity;
            }
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}