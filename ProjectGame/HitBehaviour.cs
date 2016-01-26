using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class HitBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject OwnSword { get; set; }
        private IBehaviour behaviourStats;

        public HitBehaviour(GameObject sword)
        {
            OwnSword = sword;
        }

        public void OnUpdate(GameTime gameTime)
        {
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
                        behaviourStats = GameObject.GetBehaviourOfType(typeof(StatBehaviour));
                        if (!other.HasBehaviourOfType(typeof(WeaponBehaviour))) return;
                        if (other != OwnSword)
                            (behaviourStats as StatBehaviour).HealthDown(10);
                    }
                    break;
            }
        }
    }
}
