using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class StatBehaviour : IBehaviour
    {
        public float Testos;
        public float RegenSpeed;
        public float Health;
        public float InitialTestos;
        public bool Regen { get; set; }
        public bool HealthRegenSword { get; set; }

        public GameObject GameObject { get; set; }
        public void OnMessage(IMessage message) { }

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
            if (Testos < InitialTestos && Regen)
                Testos += RegenSpeed;
            if (HealthRegenSword)
                Health += RegenSpeed;
        }

        /// <summary>
        /// Testos decrease when attack
        /// </summary>
        /// <param name="TestosLevel"> Different decrease level with different attack method </param>

        public void TestosDown(int TestosLevel)
        {
            if (TestosLevel == 1) // stab attack
                Testos -= 20;
            if (TestosLevel == 2) // swing attack
                Testos -= 30;
        }

        /// <summary>
        /// Damage if object (player/mob) gets hit
        /// </summary>
        /// <param name="Damage"> Hit Power Level </param>

        public void HealthDown(int Damage)
        {
            Health -= Damage;
        }

        public void HealthPotion()
        {
            Health += 10;
        }

        public void TestosPotion()
        {
            Testos += 10;
        }
    }
}
