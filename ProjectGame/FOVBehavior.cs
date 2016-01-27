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

        private GameObject player;
        private List<GameObject> gameObjects;
        private bool playerInViewLastFrame = false;
        public int ViewDistance { get; set; }

        public FOVBehavior(List<GameObject> GameObjects)
        {
            gameObjects = GameObjects;
            ViewDistance = 300;
        }

        public void OnUpdate(GameTime gameTime)
        {
            bool found = DetectPlayer();
            if (checkNewTarget(found))
            {
                GameObject.OnMessage(new AreaEnteredMessage(player));
            }
            if (checkLostTarget(found))
            {
                GameObject.OnMessage(new AreaExitedMessage(player));
            }
            playerInViewLastFrame = found;
        }

        public void OnMessage(IMessage message)
        {

        }

        private bool checkNewTarget(bool _found)
        {
            return _found && !playerInViewLastFrame;
        }

        private bool checkLostTarget(bool _found)
        {
            return !_found && playerInViewLastFrame;
        }

        public bool DetectPlayer()
        {
            Rectangle visionBox = CreateVisionBox();
            List<GameObject> visibleObjects = CreateVisibleObjectsList(visionBox);
            return CheckForPLayers(visibleObjects);
        }

        public bool CheckForPLayers(List<GameObject> visibleObjects)
        {
            foreach (GameObject objectInList in visibleObjects)
            {
                if (objectInList.HasBehaviourOfType(typeof(InputMovementBehaviour)))
                {
                    player = objectInList;
                    return true;
                }
            }
            return false;
        }

        public Rectangle CreateVisionBox()
        {
            while (checkRotation(0)) ;

            if (checkRotation(1, 22.5f, 337.5f))
            {
                return createRect(0);
            }
            if (checkRotation(2, 22.5f, 67.5f))
            {
                return createRect(2);
            }
            if (checkRotation(2, 67.5f, 112.5f))
            {
                return createRect(3);
            }
            if (checkRotation(2, 157.5f, 202.5f))
            {
                return createRect(4);
            }
            if (checkRotation(2, 202.5f, 247.5f))
            {
                return createRect(5);
            }
            if (checkRotation(2, 247.5f, 292.5f))
            {
                return createRect(6);
            }
            if (checkRotation(2, 292.5f, 337.5f))
            {
                return createRect(7);
            }

            return createRect(0);
        }

        private Rectangle createRect(int type)
        {
            Vector2 XY = GameObject.Position;

            float width = ViewDistance, height = ViewDistance;

            switch(type)
            {
                case 0:
                    break;
                case 1:
                    XY.X -= width / 2;
                    XY.Y -= height;
                    break;
                case 2:
                    XY.Y -= height;
                    break;
                case 3:
                    XY.Y -= height / 2;
                    break;
                case 4:
                    XY.X -= width / 2;
                    break;
                case 5:
                    XY.X -= width;
                    break;
                case 6:
                    XY.X -= width;
                    XY.Y -= height / 2;
                    break;
                case 7:
                    XY.X -= width;
                    XY.Y -= height;
                    break;
            }
            
            return new Rectangle((int)XY.X, (int)XY.Y, (int)width, (int)height);
        }

        private bool checkRotation(int type = 0, float r1 = 0, float r2 = 0)
        {
            double rotation = MathHelper.ToDegrees(GameObject.Rotation) % 360;

            switch (type)
            {
                case 0:
                    if (rotation < 0)
                        rotation += 360;
                    return false;
                case (1):
                    return (rotation < r1 || rotation >= r2);
                case (2):
                    return (rotation >= r1 && rotation < r2);
                default:
                    return false;
            }
        }

        public List<GameObject> CreateVisibleObjectsList(Rectangle visionBox)
        {
            List<GameObject> objectList = new List<GameObject>();
            foreach (GameObject gameObjectFromList in gameObjects)
            {
                Rectangle intersection = new Rectangle((int)gameObjectFromList.Position.X, (int)gameObjectFromList.Position.Y, gameObjectFromList.SourceRectangle.Width, gameObjectFromList.SourceRectangle.Height);
                if (intersection.Intersects(visionBox))
                {
                    objectList.Add(gameObjectFromList);
                }
            }
            return objectList;
        }
    }
}