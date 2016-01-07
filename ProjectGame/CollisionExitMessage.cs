namespace ProjectGame
{
    public class CollisionExitMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.CollisionExit; } }
        public GameObject CollidingObject { get; set; }

        public CollisionExitMessage(GameObject collidingObject)
        {
            CollidingObject = collidingObject;
        } 
    }
}