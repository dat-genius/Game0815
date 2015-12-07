namespace ProjectGame
{
    public enum MessageType
    {
        CollisionEnter,
        CollisionExit
    }

    public interface IMessage
    {
        MessageType MessageType { get; }
    }
}