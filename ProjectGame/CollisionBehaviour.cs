using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class CollisionBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }

        public void OnUpdate(GameTime gameTime)
        {
        }

        public void OnMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.CollisionEnter:
                    {
                        if (!GameObject.HasBehaviourOfType(typeof(AttackBehaviour))) return;

                        var behaviourStats = GameObject.GetBehaviourOfType(typeof(StatBehaviour));
                        (behaviourStats as StatBehaviour).HealthDown(10);
                    }
                    break;

            }
        }
    }
}
