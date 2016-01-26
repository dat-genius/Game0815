using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace ProjectGame
{
    public class MonsterAttack : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Target { get; set; }
        private float radius;
        private bool attack;
        private bool inSight = false;
        private bool isBoss;

        public MonsterAttack(GameObject target, bool boss = false)
        {
            Target = target;
            CalculateRange();
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (GameObject.HasBehaviourOfType(typeof(FOVBehavior)))
            {
                var behaviour = GameObject.GetBehaviourOfType(typeof(FOVBehavior));
                inSight = (behaviour as FOVBehavior).DetectPlayer();
            }
            else
                inSight = true;            
            

            if (inSight && CheckRange())
                attack = true;
            else
                attack = false;

            if(GameObject.HasBehaviourOfType(typeof(AttackBehaviour)))
            {
                var behaviour = GameObject.GetBehaviourOfType(typeof(AttackBehaviour));
                (behaviour as AttackBehaviour).Attack = attack;
            }

        }

        public bool CheckRange()
        {
            var positionDifference = Target.Position - GameObject.Position;
            if (positionDifference.Length() <= radius)
                return true;
            else
                return false;
        }

         private void CalculateRange()
        {
            if (isBoss)
                radius =  150f;
            else
                radius =  90f;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
