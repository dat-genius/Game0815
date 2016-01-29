using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGame
{
    public class ShieldBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject shield {get; private set; }

        public bool defend;
        private StatBehaviour stats;

        public ShieldBehaviour(Texture2D _shield)
        {
            shield = new GameObject(false, false);
            shield.Texture = _shield;
        }

        private void calculatePosition()
        {
            shield.Rotation = GameObject.Rotation;

            var displacement = new Vector2
            {
                X = (float)Math.Sin(GameObject.Rotation),
                Y = (float)-Math.Cos(GameObject.Rotation)
            };
            var middleLine = Vector3.Cross(new Vector3(displacement.X, displacement.Y, 0), new Vector3(0, 0, 1));

            // Move sword out of player's center
            const float offsetCenterToOuter = 40.0f;
            displacement *= offsetCenterToOuter;

            // Move sword 'horizontally' along the player (relative using cross product)
            const float offsetMiddleToHand = 25.0f;
            displacement += new Vector2(middleLine.X, middleLine.Y) * offsetMiddleToHand;

            shield.Position = GameObject.Position + displacement;
        }

        public bool AllowDraw()
        {
            stats = GameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            return stats.Testos > 0;
        }
        
        public bool CheckToDefend()
        {
            return defend && AllowDraw();
        }

        private void shieldUseTestos()
        {
            stats = GameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            stats.TestosDown(2);
        }

        private void toggleDefending(bool i)
        {
            var hitbehavior = GameObject.GetBehaviourOfType(typeof(HitBehaviour)) as HitBehaviour;

            hitbehavior.defend = i;
        }

        public void DefendNow()
        {
            calculatePosition();
            shield.IsDrawable = true;

            shieldUseTestos();
            toggleDefending(true);
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (CheckToDefend())
            {
                DefendNow();
            }
            else
            {
                shield.IsDrawable = false;
                toggleDefending(false);
            }
                
        }
        public void OnMessage(IMessage message) { }
    }
}
