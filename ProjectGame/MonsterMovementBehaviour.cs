﻿using System;
using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class MonsterMovementBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public int Lives { get; set; }

        private readonly Vector2[] pathNodes;
        private int currentNodeIndex;
        private readonly TimeSpan timePerPath;
        private TimeSpan walkTimer;
        private Vector2 departurePosition;
        private MovementBehaviour movementBahaviour = null;
        public bool Collision = false;

        public MonsterMovementBehaviour(int lives = 5)
        {
            Lives = lives;
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
            if (!Collision)
            {
                if (movementBahaviour == null)
                {
                    var behaviour = GameObject.GetBehaviourOfType(typeof(MovementBehaviour));
                    if (behaviour != null)
                    {
                        movementBahaviour = behaviour as MovementBehaviour;
                    }
                    else return;
                }

                walkTimer += gameTime.ElapsedGameTime;
                var lerpFactor = (float)walkTimer.TotalMilliseconds / (float)timePerPath.TotalMilliseconds;
                if (lerpFactor >= 1.0f)
                {
                    departurePosition = pathNodes[currentNodeIndex];
                    if (++currentNodeIndex > pathNodes.Length - 1) currentNodeIndex = 0;
                    walkTimer = TimeSpan.FromSeconds(0);
                    lerpFactor = 0;
                }
                if (GameObject != null)
                    GameObject.Position = Vector2.Lerp(departurePosition, pathNodes[currentNodeIndex], lerpFactor);
            }
            Collision = false;

        }

        public void OnMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.CollisionEnter:
                    {
                        var collisionEnterMessage = message as CollisionEnterMessage;
                        if (collisionEnterMessage == null) return;
                        var other = collisionEnterMessage.CollidingObject;
                        if (!other.HasBehaviourOfType(typeof(WeaponBehaviour))) return;
                        GameObject.Color = Color.Red;
                        if (--Lives <= 0)
                        {
                            GameObject.IsDrawable = false;
                        }
                    }
                    break;
                case MessageType.CollisionExit:
                    {
                        var collisionExitMessage = message as CollisionExitMessage;
                        if (collisionExitMessage == null) return;
                        var other = collisionExitMessage.CollidingObject;
                        if (other.HasBehaviourOfType(typeof(WeaponBehaviour)))
                        {
                            GameObject.Color = Color.White;
                        }
                    }
                    break;
            }
        }
    }
}