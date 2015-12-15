using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Moet nog zorgen dat als defend false is het shield object gedraaid is //Bij de draw functie dus
// verweken in de code, met de draw enzo en intialiseren
// moet nog wat gedaan worden aan de stama points aftrekken


namespace ProjectGame
{
    public class ShieldBehaviour :  IBehaviour
    {
        public Vector2 Position { get; set; }
        public GameObject GameObject { get; set; }
        public bool Defend = false; 
        public Texture2D Texture { get; set; }

        public void OnUpdate(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.J))
            {
                Position = new Vector2(GameObject.Position.X, GameObject.Position.Y - 33);
                Defend = true;
                
            }
            else
            {
                Position = new Vector2(GameObject.Position.X - 33, GameObject.Position.Y);
                Defend = false;
            }


        }

        public void OnMessage(IMessage message)
        {
        }
        


  
    }
}
