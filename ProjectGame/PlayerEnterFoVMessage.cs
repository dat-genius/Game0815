namespace ProjectGame
{
    public class PlayerEnterFoVMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.EnemyEnterFoV; } }
        public GameObject Player { get; set; }

        public PlayerEnterFoVMessage(GameObject player)
        {
            Player = player;
        }
    }
}