namespace ProjectGame
{
    public enum MessageType
    {
        CollisionEnter,
        CollisionExit,
        EnemyEnterFoV,
        EnemyExitFoV
    }

    public interface IMessage
    {
        MessageType MessageType { get; }
    }
}