using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class HitBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject OwnSword { get; set; }
        private IBehaviour behaviourStats;
        public Texture2D Boss1Swordtexture { get; set; }
        private bool player;

        public HitBehaviour(GameObject sword, Texture2D boss1SwordTexture)
        {
            OwnSword = sword;
            Boss1Swordtexture = boss1SwordTexture;
            player = true;
        }
        
        public HitBehaviour(GameObject sword)
        {
            OwnSword = sword;
            player = false;
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
                        {
                            (behaviourStats as StatBehaviour).HealthDown(10);
                            CheckForBossAttack(other);

                        }
                    }
                    break;
            }
        }

        private void CheckForBossAttack(GameObject sword)
        {
            if (player)
            {
                if (sword.Texture == Boss1Swordtexture)
                {
                    behaviourStats = GameObject.GetBehaviourOfType(typeof(StatBehaviour));
                    (behaviourStats as StatBehaviour).TestosDown(1);
                }
            }
        }
    }
}
