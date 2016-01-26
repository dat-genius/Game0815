using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class BondBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Sword { get; set; }
        public GameObject Helmet { get; set; }
        
        public BondBehaviour(GameObject sword, GameObject helmet)
        {
            Sword = sword;
            Helmet = helmet;
        }
        
        public void OnUpdate(GameTime gameTime)
        { 
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
