using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGame
{
    class TeleportBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }

        private Dictionary<int, List<Rectangle>> levels;
        public Dictionary<int, bool> WhichLevel;
        public Dictionary<int, Vector2> SpawnPoint;
        private List<Rectangle> test;


        public TeleportBehaviour()
        {
            levels = new Dictionary<int,List<Rectangle>>();
            WhichLevel = new Dictionary<int,bool>();
            SpawnPoint = new Dictionary<int, Vector2>();

            List<Rectangle> Level1 = new List<Rectangle>();
            Level1.Add(new Rectangle(39 * 32, 79 * 32, 3 * 32, 2 * 32));
            Level1.Add(new Rectangle(80 * 32, 39 * 32, 2 * 32, 3 * 32));

            List<Rectangle> Level2 = new List<Rectangle>();
            Level2.Add(new Rectangle(39 * 32, 239 * 32, 3 * 32, 2 * 32));
            Level2.Add(new Rectangle(81 * 32, 227 * 32, 2 * 32, 3 * 32));

            List<Rectangle> Level3 = new List<Rectangle>();
            Level3.Add(new Rectangle(237 * 32, 39 * 32, 2 * 32, 3 * 32));
            Level3.Add(new Rectangle(277 * 32, 79 * 32, 3 * 32, 2 * 32));

            List<Rectangle> Level4 = new List<Rectangle>();
            Level4.Add(new Rectangle(236 * 32, 277 * 32, 2 * 32, 3 * 32));
            Level4.Add(new Rectangle(277 * 32, 238 * 32, 3 * 32, 2 * 32));

            levels.Add(1, Level1);
            levels.Add(2, Level2);
            levels.Add(3, Level3);
            levels.Add(4, Level4);

            for(int i = 1; i < 5; i++)
                WhichLevel.Add(i, false);

            SpawnPoint.Add(0, new Vector2(1312, 5088));
            SpawnPoint.Add(1, new Vector2(29 * 16, 47 * 16));
            SpawnPoint.Add(2, new Vector2(5 * 32, 17 * 32));
            SpawnPoint.Add(3, new Vector2(31 * 16, 58 * 16));
            SpawnPoint.Add(4, new Vector2(27 * 32, 46 * 32));
        }

        public void OnUpdate(GameTime gameTime)
        {
            var playerRectangle = new Rectangle((int)GameObject.Position.X, (int)GameObject.Position.Y, (int)GameObject.Texture.Width, (int)GameObject.Texture.Height);
            for(int i = 1; i <= levels.Keys.Count; i++)
            {
                for (int y = 0; y < levels[i].Count; y++)
                {
                    var intersect = Rectangle.Intersect(playerRectangle, levels[i][y]);

                    if(!intersect.IsEmpty)
                        setMap(i);
                }
            }
        }


        private void setMap(int i)
        {
            WhichLevel[i] = true;
        }

        public void OnMessage(IMessage message) { }


    }
}
