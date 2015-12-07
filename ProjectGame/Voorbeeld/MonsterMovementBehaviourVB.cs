using System;
using Microsoft.Xna.Framework;

namespace ProjectGame.Voorbeeld
{
    public class MonsterMovementBehaviourVB : IBehaviour
    {
        public GameObject GameObject { get; set; }

        private readonly Vector2[] pathNodes;
        private readonly TimeSpan timePerPath;
        private TimeSpan walkTimer;
        private int currentPathIndex;

        public MonsterMovementBehaviourVB()
        {
            pathNodes = new Vector2[]
            {
                new Vector2(50, 50),
                new Vector2(100, 50),
                new Vector2(300, 300),
                new Vector2(300, 200),
                new Vector2(100, 20),
            };
            currentPathIndex = 0;
            timePerPath = TimeSpan.FromSeconds(5);
        }

        public void OnUpdate(GameTime gameTime)
        {
            walkTimer += gameTime.ElapsedGameTime;
            if (Vector2.Distance(GameObject.Position, pathNodes[currentPathIndex]) < 5)
            {
                if (++currentPathIndex > pathNodes.Length - 1) currentPathIndex = 0;
                walkTimer = TimeSpan.FromSeconds(0);
            }
            var lerpFactor = (float)walkTimer.TotalMilliseconds / (float)timePerPath.TotalMilliseconds;
            GameObject.Position = Vector2.Lerp(GameObject.Position, pathNodes[currentPathIndex], lerpFactor);
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}