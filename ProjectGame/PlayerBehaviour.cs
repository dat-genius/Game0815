using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class PlayerBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public void OnUpdate(GameTime gameTime)
        {

        }
        public void OnMessage(IMessage message) { }
    }
}
