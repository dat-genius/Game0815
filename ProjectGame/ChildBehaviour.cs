using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class ChildBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Parent { get; set; }

        public void OnUpdate(GameTime gameTime)
        {
            GameObject.Position = Parent.Position;
            GameObject.SourceRectangle = Parent.SourceRectangle;
            GameObject.Rotation = Parent.Rotation;
            GameObject.Size = Parent.Size;
            GameObject.IsDrawable = Parent.IsDrawable;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
