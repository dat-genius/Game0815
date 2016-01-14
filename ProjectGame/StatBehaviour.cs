using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    class StatBehaviour : IBehaviour
    {
        public float Testos;
        public float RegenSpeed;
        public float Health;
        public float InitialTestos;

        public StatBehaviour(float health, float testos, float regenspeed)
        {
            Testos = testos;
            InitialTestos = testos;
            RegenSpeed = regenspeed;
            Health = health;
        }

        public void OnUpdate(GameTime gametime)
        {
            if (Health <= 0)
            {
                //dood
            }
            if (Testos <= InitialTestos) Testos++;


        }
    }
}
