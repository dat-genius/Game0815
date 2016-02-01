using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGame
{
    public class DropItem : IBehaviour
    {
        public GameObject GameObject { get; set; }
        private StatBehaviour behaviourStat = null;


        public DropItem(GameObject player)
        {
            GameObject = player;
            behaviourStat = GameObject.GetBehaviourOfType("StatBehaviour") as StatBehaviour;
        }

        public void OnUpdate(GameTime gameTime)
        {            
        }
        
        public void OnMessage(IMessage message)
        {
        }

        public void AddPotion()
        {
            if (RN(0, 10) % 3 == 0)
            {
                behaviourStat.AddPotion();
            }

        }

        private int RN(int min, int max)
        {
            var random = new Random();
            int randomN = random.Next(min, max);
            return randomN;
        }

    }
}