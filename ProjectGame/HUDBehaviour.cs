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
        SpriteFont Text;

        Rectangle RectangleHealth;
        Vector2 PositionHealth;
        string HealthText = "Health: ";
        
        Rectangle RectangleTestos;
        Vector2 PositionTestos;
        string TestosText = "Testosteron: ";

        int BeginPosXHealth;
        int BeginPosXTestos;

        public void OnMessage(IMessage message) { }

        public HUDBehaviour(Texture2D textureHealth, Texture2D textureTestos, SpriteFont text, GameObject gameObject, int ScreenWidth)
        {
            TextureHealth = textureHealth;
            TextureTestos = textureTestos;
            Text = text;

            GameObject = gameObject;
            Stats = gameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;

            BeginPosXHealth = (ScreenWidth / 2) - ((int)Stats.Health / 2);
            BeginPosXTestos = (ScreenWidth / 2) - ((int)Stats.Testos / 2);

            PositionHealth = new Vector2(BeginPosXHealth, 5);
            PositionTestos = new Vector2(BeginPosXTestos, 30);
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
            spriteBatch.DrawString(Text, HealthText, new Vector2(BeginPosXHealth + 2, 6), Color.Black);
            spriteBatch.DrawString(Text, TestosText, new Vector2(BeginPosXTestos + 2, 31), Color.Black);
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