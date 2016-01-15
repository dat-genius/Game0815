using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGame
{
    public class HUDBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }

        StatBehaviour Stats;
        
        Texture2D TextureHealth;
        Texture2D TextureTestos;

        Rectangle RectangleHealth;
        Vector2 PositionHealth;

        Rectangle RectangleTestos;
        Vector2 PositionTestos;

        int BeginPosX;

        public void OnMessage(IMessage message) { }

        public HUDBehaviour(Texture2D textureHealth, Texture2D textureTestos, GameObject gameObject, int ScreenWidth)
        {
            TextureHealth = textureHealth;
            TextureTestos = textureTestos;

            GameObject = gameObject;
            Stats = gameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            BeginPosX = (ScreenWidth / 2) - ((int)Stats.Health / 2);
            PositionHealth = new Vector2(BeginPosX, 5);
            PositionTestos = new Vector2(BeginPosX, 20);
        }

        public void OnUpdate(GameTime gameTime)
        {
            RectangleHealth = new Rectangle((int)PositionHealth.X, (int)PositionHealth.Y, (int)Stats.Health, TextureHealth.Height);
            RectangleTestos = new Rectangle((int)PositionTestos.X, (int)PositionTestos.Y, (int)Stats.Testos, TextureTestos.Height);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureHealth, RectangleHealth, Color.White);
            spriteBatch.Draw(TextureTestos, RectangleTestos, Color.White);
        }
    }
}

            //spriteBatch.Begin();
            //foreach (var gameObject in gameObjects)
            //    if (gameObject.HasBehaviourOfType(typeof(StatBehaviour)))
            //    {
            //        StatBehaviour statBehaviour = gameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;
            //        string text = "Health: " + statBehaviour.Health;

            //        Vector2 size = textFont.MeasureString(text);
            //        float i = ((GraphicsDevice.Viewport.Width / 2) - (size.X / 2));

            //        spriteBatch.DrawString(textFont, text , new Vector2(i, 10), Color.Black);
            //    }
            //spriteBatch.End();