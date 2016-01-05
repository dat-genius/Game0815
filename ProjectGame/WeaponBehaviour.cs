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

            const float offset = 50.0f;
            var displacement = new Vector2
            {
                X = (float) Math.Sin(Wielder.Rotation),
                Y = (float) -Math.Cos(Wielder.Rotation)
            };
            displacement *= offset;
            GameObject.Position = Wielder.Position + displacement;

            if (timeUntilUsable.TotalSeconds <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
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
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
