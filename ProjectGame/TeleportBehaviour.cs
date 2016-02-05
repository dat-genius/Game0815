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
        private Dictionary<int, Rectangle> doors;
        public Dictionary<int, bool> WhichLevel { get; set; }
        public Dictionary<int, Vector2> SpawnPoint;
        private int currentMap;

        public TeleportBehaviour(int _currentMap)
        {
            currentMap = _currentMap;

            levels = new Dictionary<int,List<Rectangle>>();
            doors = new Dictionary<int, Rectangle>();
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

            doors.Add(1, new Rectangle(26 * 16, 52 * 16, 7 * 16, 2 * 16));
            doors.Add(2, new Rectangle(2 * 32, 15 * 32, 1 * 32, 6 * 32));
            doors.Add(3, new Rectangle(29 * 16, 58 * 16, 3 * 16, 2 * 16));
            doors.Add(4, new Rectangle(23 * 32, 48 * 32, 8 * 32, 2 * 32));

            for(int i = 0; i < 5; i++)
                WhichLevel.Add(i, false);

            SpawnPoint.Add(0, new Vector2(1312, 5088));
            SpawnPoint.Add(1, new Vector2(29 * 16, 47 * 16));
            SpawnPoint.Add(2, new Vector2(5 * 32, 17 * 32));
            SpawnPoint.Add(3, new Vector2(31 * 16, 58 * 16));
            SpawnPoint.Add(4, new Vector2(27 * 32, 46 * 32));
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (currentMap != 0)
                checkDoors(currentMap);
            else if (currentMap == 0)
            {
                for (int i = 1; i <= levels.Keys.Count; i++)
                {
                    for (int y = 0; y < levels[i].Count; y++)
                    {
                        if (checkIntersect(levels[i][y]))
                        {
                            setMap(i);
                        }
                    }
                }
            }
        }

        private void checkDoors(int i)
        {
            if (checkIntersect(doors[i]))
                setMap(0);
        }

        private bool checkIntersect(Rectangle i)
        {
            var playerRectangle = new Rectangle((int)GameObject.Position.X, (int)GameObject.Position.Y, (int)GameObject.Texture.Width, (int)GameObject.Texture.Height);

            var intersect = Rectangle.Intersect(playerRectangle, i);

            return (!intersect.IsEmpty);
        }

        private void setMap(int selectedMap)
        {
            for (int i = 0; i < WhichLevel.Count; i++)
                WhichLevel[i] = false;

            WhichLevel[selectedMap] = true; 
        }

        public void OnMessage(IMessage message) { }


    }
}
