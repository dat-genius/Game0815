﻿using System;
using Microsoft.Xna.Framework;

namespace ProjectGame.Voorbeeld
{
    public class MonsterMovementBehaviourVB : IBehaviour
    {
        public GameObject GameObject { get; set; }

        private readonly Vector2[] pathNodes;
        private int currentNodeIndex;
        private readonly TimeSpan timePerPath;
        private TimeSpan walkTimer;
        private Vector2 departurePosition;

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
            currentNodeIndex = 0;
            timePerPath = TimeSpan.FromSeconds(1);
            departurePosition = Vector2.Zero;
        }

        public void OnUpdate(GameTime gameTime)
        {
            walkTimer += gameTime.ElapsedGameTime;
            var lerpFactor = (float)walkTimer.TotalMilliseconds / (float)timePerPath.TotalMilliseconds;
            if (lerpFactor >= 1.0f)
            {
                departurePosition = pathNodes[currentNodeIndex];
                if (++currentNodeIndex > pathNodes.Length - 1) currentNodeIndex = 0;
                walkTimer = TimeSpan.FromSeconds(0);
                lerpFactor = 0;
            }
            GameObject.Position = Vector2.Lerp(departurePosition, pathNodes[currentNodeIndex], lerpFactor);
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}