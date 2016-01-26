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
        public float InitialHealth;
        public uint Potions;
        private bool IsHDown = false;
        public bool Regen { get; set; }
        public bool HealthRegenSword { get; set; }
        public DateTime time;

        public GameObject GameObject { get; set; }
        public void OnMessage(IMessage message) { }

        public StatBehaviour(float health, float testos, float regenspeed)
        {
            Testos = testos;
            InitialTestos = testos;
            RegenSpeed = regenspeed;
            Health = health;
            InitialHealth = health;
            Potions = 5;
        }

        public StatBehaviour(float health)
        {
            Health = health;
        }

        public void OnUpdate(GameTime gametime)
        {
            if (Health <= 0)
            {
                GameObject.IsDrawable = false;
                GameObject.IsCollidable = false;
            }

            if (Testos < InitialTestos && (DateTime.Now - time).Seconds > 5)
            {
                Testos += RegenSpeed;
            }
            if (HealthRegenSword)
                Health += RegenSpeed;
            TakePotion();
        }

        /// <summary>
        /// Testos decrease when attack
        /// </summary>
        /// <param name="TestosLevel"> Different decrease level with different attack method </param>

        public void TestosDown(int TestosLevel)
        {
            if (TestosLevel == 1) // stab attack
                Testos -= 20;
            if (TestosLevel == 2) // Sprint
                Testos -= 0.5f;

            if (Testos < 0)
            {
                Testos = 0;
                time = DateTime.Now;
            }
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
            if ((Health + 30) >= InitialHealth)
            {
                Health = InitialHealth;
            }
            else { Health += 30; }
            Potions -= 1;
        }

        public void TakePotion()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.H) && !IsHDown && Potions>0){                
                HealthPotion();
                IsHDown = true;
            }else if(Keyboard.GetState().IsKeyUp(Keys.H)){
                    IsHDown = false;
                }
            
        }

        public void AddPotion()
        {
            Potions += 1;
        }

    }
}
