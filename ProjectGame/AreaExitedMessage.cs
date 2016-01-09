namespace ProjectGame
{
    public class AreaExitedMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.AreaExited; } }
        public GameObject Subject { get; set; }

        public AreaExitedMessage(GameObject subject)
        {
            Subject = subject;
        }
    }
}