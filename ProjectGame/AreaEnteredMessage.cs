namespace ProjectGame
{
    public class AreaEnteredMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.AreaEntered; } }
        public GameObject Subject { get; set; }

        public AreaEnteredMessage(GameObject subject)
        {
            Subject = subject;
        }
    }
}