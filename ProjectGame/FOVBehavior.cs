using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class FOVBehavior : IBehaviour
    {
        public GameObject GameObject { get; set; }
        private float foV = 1;
        private int viewDistance = 300;

        float FoV
        {
            get
            {
                return foV;
            }
            set
            {
                foV = value;
            }
        }

        int ViewDistance { 
            get 
            {
                return viewDistance;
            } 
            set 
            {
                viewDistance = value;
            } 
        }

        public void OnUpdate(GameTime gameTime)
        {

        }

        public void OnMessage(IMessage message)
        {

        }

        public bool DetectPlayer()
        {
            Rectangle visionBox = createVisionBox();
            List<GameObject> visibleObjects = CreateVisibleObjectsList(visionBox);
            return CheckForPLayers(visibleObjects);
        }

        public bool CheckForPLayers(List<GameObject> visibleObjects)
        {

            return false;
        }

        public Rectangle createVisionBox()
        {
            float x = GameObject.Position.X + GameObject.SourceRectangle.Width / 2;
            float y = GameObject.Position.Y + GameObject.SourceRectangle.Height / 2; 
            float width = 300, height = 300;
            double rotation = (double)GameObject.Rotation % 2f*(Math.PI);
            if (rotation < Math.PI)
            {
                if (rotation < Math.PI / 8 || rotation > 15*Math.PI/8)
                {
                    y -= height / 2;
                }
                if (rotation >= Math.PI / 8 && rotation < 3 * Math.PI / 8)
                {
                    y -= height;
                }
                if (rotation >= Math.PI * 3 / 8 && rotation < 5 * Math.PI / 8)
                {
                    x -= width / 2;
                    y -= height;
                }
                if (rotation >= Math.PI * 5 / 8 && rotation < 7 * Math.PI / 8)
                {
                    x -= width;
                    y -= height;
                }
                if (rotation >= Math.PI * 7 / 8 && rotation < 9 * Math.PI / 8)
                {
                    x -= width;
                    y -= height / 2;
                }
                if (rotation >= Math.PI * 9 / 8 && rotation < 11 * Math.PI / 8)
                {
                    x -= width;
                }
                if (rotation >= Math.PI * 11 / 8 && rotation < 13 * Math.PI / 8)
                {
                    x -=width/2;
                }
            }
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public List<GameObject> CreateVisibleObjectsList(Rectangle visionBox)
        {
            List<GameObject> objectList = new List<GameObject>();
            foreach (GameObject gameObjectFromList in objectList)
            {
                if (gameObjectFromList.SourceRectangle.Intersects(GameObject.SourceRectangle))
                {
                    Rectangle banaan = new Rectangle(10,10,10,10);
                    objectList.Add(gameObjectFromList);
                }
            }
            return objectList;
        }
    }
}
