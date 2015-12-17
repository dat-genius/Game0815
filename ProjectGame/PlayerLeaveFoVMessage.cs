namespace ProjectGame
{
    public class PlayerLeaveFoVMessage : IMessage
    {
        public MessageType MessageType { get { return MessageType.EnemyExitFoV; } }
        public GameObject Player { get; set; }

        public PlayerLeaveFoVMessage(GameObject player)
        {
            Player = player;
        }
    }
}