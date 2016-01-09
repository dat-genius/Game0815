namespace ProjectGame
{
    public enum MessageType
    {
        CollisionEnter,
        CollisionExit,
        AreaEntered,
        AreaExited
    }

    public interface IMessage
    {
        MessageType MessageType { get; }
    }
}