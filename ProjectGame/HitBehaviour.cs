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
        public Vector2 NewPosition;
        private bool player;
        private bool canTeleport;
        private int hitCount = 0;

        public HitBehaviour(GameObject sword, Texture2D boss1SwordTexture)
        {
            OwnSword = sword;
            Boss1Swordtexture = boss1SwordTexture;
            player = true;
        }
        
        public HitBehaviour(GameObject sword, bool teleport = false)
        {
            OwnSword = sword;
            player = false;
            canTeleport = teleport;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (!canTeleport) return;
            if (hitCount == 3)
            {
                var newX = CreateRandom(384, 1440);
                var newY = CreateRandom(864, 1536);
                NewPosition = new Vector2(newX, newY);
                GameObject.Position = NewPosition;
                hitCount = 0;
                SetNewSpwan();
            }
        }

        private int CreateRandom(int min, int max)
        {
            var random = new Random();
            int randomN = random.Next(min, max);
            return randomN;
        }

        private void SetNewSpwan()
        {
            if (!GameObject.HasBehaviourOfType(typeof(ChaseBehaviour))) return;
            var behaviour = GameObject.GetBehaviourOfType(typeof(ChaseBehaviour));
            (behaviour as ChaseBehaviour).SpawnPoint = NewPosition;
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
                            hitCount++;
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
