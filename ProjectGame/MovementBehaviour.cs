using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace ProjectGame
{
    public class MovementBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public Vector2 Velocity { get; set; }

        private bool hasTextureList;
        private Texture2D idle;
        private List<Texture2D> texturelist;
        private int listpos;
        private int deltaTime;
        
        public MovementBehaviour() { }

        public MovementBehaviour(List<Texture2D> textureList)
        {
            hasTextureList = true;
            texturelist = textureList;
            idle = textureList[0];
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (hasTextureList)
            {
                UpdateTexture(gameTime);
            }

            if (Velocity.Length() > 0)
            {
                GameObject.Position = Game1.ResolveWorldCollision(GameObject, Velocity);
            }
        }

        public void UpdateTexture(GameTime gameTime)
        {
            if (Math.Abs(Velocity.X) > 0.01 || Math.Abs(Velocity.Y) > 0.01)
            {
                int delta = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if( (deltaTime - delta) > 50){
                    listpos++;
                    if (listpos >= texturelist.Count)
                    {
                        listpos = 0;
                    }
                    deltaTime = 0;
                    GameObject.Texture = texturelist[listpos];
                }
                deltaTime += delta;
                
            }
            else
            {
                GameObject.Texture = idle;
                listpos = 0;
            }
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}