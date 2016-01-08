using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class HelmetBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }

        public GameObject Owner { get; set; }
        //public int DefenseModifier { get; set; }
        

        public void OnUpdate(GameTime gameTime)
        {
            GameObject.Position = Owner.Position;
            GameObject.SourceRectangle = Owner.SourceRectangle;
            GameObject.Rotation = Owner.Rotation;
            GameObject.Size = Owner.Size;
        }

        public void OnMessage(IMessage message)
        {

        }
        
    }
}
