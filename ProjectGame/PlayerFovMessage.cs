namespace ProjectGame
{
    public class PlayerFovMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.EnemyExitFoV; } }
        public GameObject Player { get; set; }

        public PlayerFovMessage(GameObject player)
        {
            Player = player;
        }
    }
}