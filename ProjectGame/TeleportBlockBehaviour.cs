using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class TeleportBlockBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public Vector2 TargetLocation { get; set; }

        public TeleportBlockBehaviour(Vector2 targetPos)
        {
            TargetLocation = targetPos;
        }

        public void OnUpdate(GameTime gameTime)
        {
            
        }

        public void OnMessage(IMessage message)
        {
            if (message.GetType() == typeof(CollisionEnterMessage))
            {
                CollisionEnterMessage temp = (CollisionEnterMessage)message;
                if (temp.CollidingObject.HasBehaviourOfType("InputMovementBehaviour"))
                {
                    temp.CollidingObject.Position = TargetLocation;
                }
            }
        }
    }
}
