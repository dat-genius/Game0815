using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGame
{
    class DropItem : IBehaviour

    {
        GameObject GameObject { get; set; }
        private StatBehaviour behaviourStat = null;
        

        public DropItem(GameObject player)
        {
            GameObject = player;
            behaviourStat = GameObject.GetBehaviourOfType(typeof(StatBehaviour)) as StatBehaviour;
            
        }

        void OnUpdate(GameTime gameTime)
        {

        }
        void OnMessage(IMessage message);

        public void AddPotion()
        {
            if (RN(0, 10) % 2 == 0)
            {
                behaviourStat.AddPotion();
            }

        }

        private int RN(int min, int max)
        {
            var random = new Random();
            int randomN = random.Next(min,max);
            return randomN;
        }
               
    }
}
