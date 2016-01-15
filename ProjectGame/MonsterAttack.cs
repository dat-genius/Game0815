﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace ProjectGame
{
    class MonsterAttack : IBehaviour
    {
        public GameObject GameObject { get; set; }
        public GameObject Target { get; set; }
        private float radius;
        private bool attack;
        private bool inSight = false;

        public MonsterAttack(GameObject target)
        {
            Target = target;
            radius = 90f;
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
            

            if (/*inSight &&*/ CheckRange())
                attack = true;
            else
                attack = false;

            if(GameObject.HasBehaviourOfType(typeof(AttackBehaviour)))
            {
                var behaviour = GameObject.GetBehaviourOfType(typeof(AttackBehaviour));
                (behaviour as AttackBehaviour).Attack = attack;
            }

        }

        private bool CheckRange()
        {
            var positionDifference = Target.Position - GameObject.Position;
            if (positionDifference.Length() <= radius)
                return true;
            else
                return false;
        }

        public void OnMessage(IMessage message)
        {
        }
    }
}
