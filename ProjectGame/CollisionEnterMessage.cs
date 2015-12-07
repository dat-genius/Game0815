namespace ProjectGame
{
    public class CollisionEnterMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.CollisionEnter; } }
        public GameObject CollidingObject { get; set; }

        public CollisionEnterMessage(GameObject collidingObject)
        {
            CollidingObject = collidingObject;
        }
    }
}