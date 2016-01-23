using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    // At the moment just 'sword'
    public class WeaponBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }

        // => ParentTransform
        public GameObject Wielder { get; set; }
        public bool SwingSword { get; set; }

        private readonly TimeSpan cooldownTime;
        private readonly TimeSpan durationTime;
        private TimeSpan timeUntilUsable;
        private TimeSpan timeSinceUsage;

        public WeaponBehaviour()
        {
            cooldownTime = TimeSpan.FromMilliseconds(500);
            durationTime = TimeSpan.FromMilliseconds(200);
            timeUntilUsable = TimeSpan.FromSeconds(0);
        }

        public void OnUpdate(GameTime gameTime)
        {
            timeUntilUsable -= gameTime.ElapsedGameTime;

            GameObject.Rotation = Wielder.Rotation;

            var displacement = new Vector2
            {
                X = (float) Math.Sin(Wielder.Rotation),
                Y = (float) -Math.Cos(Wielder.Rotation)
            };
            var middleLine = Vector3.Cross(new Vector3(displacement.X, displacement.Y, 0), new Vector3(0, 0, 1));

            // Move sword out of player's center
            const float offsetCenterToOuter = 60.0f;
            displacement *= offsetCenterToOuter;        
            
            // Move sword 'horizontally' along the player (relative using cross product)
            const float offsetMiddleToHand = -25.0f;
            displacement += new Vector2(middleLine.X, middleLine.Y) * offsetMiddleToHand;

            GameObject.Position = Wielder.Position + displacement;

            if (timeUntilUsable.TotalSeconds <= 0)
            {
                if (SwingSword)
                {
                    GameObject.IsDrawable = true;
                    GameObject.IsCollidable = true;
                    timeUntilUsable = cooldownTime;
                    timeSinceUsage = TimeSpan.FromSeconds(0);
                }
            }

            timeSinceUsage += gameTime.ElapsedGameTime;
            if (timeSinceUsage < durationTime) return;
            GameObject.IsDrawable = false;
            GameObject.IsCollidable = false;
            SwingSword = false;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
