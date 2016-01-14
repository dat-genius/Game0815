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
        private bool hasTextureList = false;
        private Texture2D Idle;
        private List<Texture2D> Texturelist;
        private int listpos;
        private int deltaTime = 0;
        
        public MovementBehaviour() { }

        public MovementBehaviour(List<Texture2D> textureList)
        {
            hasTextureList = true;
            Texturelist = textureList;
            Idle = textureList[0];
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
                    if (listpos >= Texturelist.Count)
                    {
                        listpos = 0;
                    }
                    deltaTime = 0;
                    GameObject.Texture = Texturelist[listpos];
                }
                deltaTime += delta;
                
            }
            else
            {
                GameObject.Texture = Idle;
                listpos = 0;
            }
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}