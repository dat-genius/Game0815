﻿using System;
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
        private List<GameObject> gameObjects;
 //       private GameObject visionBox;
        private GameObject player;
        private bool playerInViewLastFrame = false;
        public int ViewDistance { get; set; }

        public FOVBehavior(List<GameObject> GameObjects)
        {
            gameObjects = GameObjects;
            ViewDistance = 300;
        }

        public void OnUpdate(GameTime gameTime)
        {
            var found = DetectPlayer();
            if (found && !playerInViewLastFrame)
            {
                GameObject.OnMessage(new AreaEnteredMessage(player));
            }
            if (!found && playerInViewLastFrame)
            {
                GameObject.OnMessage(new AreaEnteredMessage(player));
            }
            playerInViewLastFrame = found;
        }

        public void OnMessage(IMessage message)
        {

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
            float x = GameObject.Position.X + GameObject.Size.X / 2;
            float y = GameObject.Position.Y + GameObject.Size.Y / 2;

            float width = ViewDistance, height = ViewDistance;
            double rotation = MathHelper.ToDegrees(GameObject.Rotation) % 360;
            if (rotation < 360)
            {
                if (rotation < 22.5 || rotation >= 337.5)
                {
                    x -= width / 2;
                    y -= height;
                }
                if (rotation >= 22.5 && rotation < 67.5)
                {
                    y -= height;
                }
                if (rotation >= 67.5 && rotation < 112.5)
                {
                    y -= height / 2;
                }
                if (rotation >= 112.5 && rotation < 157.5)
                {
                    x -= width;
                    y -= height;
                }
                if (rotation >= 157.5 && rotation < 202.5)
                {
                    x -= width / 2;
                }
                if (rotation >= 202.5 && rotation < 247.5)
                {
                    x -= width;
                }
                if (rotation >= 247.5 && rotation < 292.5)
                {
                    x -= width;
                    y -= height / 2;
                }
                if (rotation >= 292.5 && rotation < 337.5)
                {
                    x -= width;
                    y -= height;
                }
            }
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
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