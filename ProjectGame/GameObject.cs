using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class GameObject
    {
        private readonly List<IBehaviour> behaviours;

        #region Common Properties

        public bool IsDrawable { get; set; }

        private bool isCollidable;
        public bool IsCollidable
        {
            get { return isCollidable; }
            set
            {
                isCollidable = value;
                if (value) return;
                foreach (var collidingGameObject in CollidingGameObjects)
                {
                    collidingGameObject.OnMessage(new CollisionExitMessage(this));
                    collidingGameObject.CollidingGameObjects.Remove(this);
                }
                CollidingGameObjects.Clear();
            }
        }

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }
        public List<GameObject> CollidingGameObjects { get; set; }

        // Used for rendering
        private Rectangle sourceRectangle;

        public Rectangle SourceRectangle
        {
            get
            {
                if (!sourceRectangle.IsEmpty) return sourceRectangle;
                if (Texture != null)
                {
                    sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                } // else: warning "You try to set the source rectangle of an object without a texture"
                return sourceRectangle;
            }
            set { sourceRectangle = value; }
        }

        // Used for collision box
        private Point size = Point.Zero;

        public Point Size
        {
            get
            {
                if (size.X != 0 || size.Y != 0) return size;
                if (Texture != null)
                {
                    size = new Point(Texture.Width, Texture.Height);
                } // else: warning "You try to get the size of an empty object"
                return size;
            }
            set { size = value; }
        }

        #endregion

        #region Constructors

        private GameObject()
        {
            behaviours = new List<IBehaviour>();
            CollidingGameObjects = new List<GameObject>();
            Color = Color.White;
        }

        public GameObject(bool isDrawable = true, bool isCollidable = true) : this()
        {
            IsDrawable = isDrawable;
            IsCollidable = isCollidable;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates all behaviours.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void OnUpdate(GameTime gameTime)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.OnUpdate(gameTime);
            }
        }


        /// <summary>
        ///     Forwards incoming messages to all attached behaviours.
        /// </summary>
        /// <param name="message">The message.</param>
        public void OnMessage(IMessage message)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.OnMessage(message);
            }
        }


        /// <summary>
        ///     Draws the game object with its current transformation.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch rendering the game object.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null) return;
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, new Vector2(Size.X/2.0f, Size.Y/2.0f), new Vector2(1, 1),
                SpriteEffects.None, 0);
        }


        /// <summary>
        ///     Adds a behaviour to the game object.
        /// </summary>
        /// <param name="behaviour">The behaviour to add.</param>
        public void AddBehaviour(IBehaviour behaviour)
        {
            behaviour.GameObject = this;
            behaviours.Add(behaviour);
        }


        /// <summary>
        ///     Removes a behaviour from the game object.
        /// </summary>
        /// <param name="behaviour">The behaviour to remove.</param>
        public void RemoveBehaviour(IBehaviour behaviour)
        {
            if (!behaviours.Contains(behaviour)) return;
            behaviours.Remove(behaviour);
            behaviour.GameObject = null;
        }


        /// <summary>
        ///     Returns the game object behaviour of a certain type if it exists.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The behaviour of the type, or null if the game object doesn't have it.</returns>
        public IBehaviour GetBehaviourOfType(Type type)
        {
            return behaviours.FirstOrDefault(behaviour => behaviour.GetType() == type);
        }


        /// <summary>
        ///     Returns whether or not the game object possesses a behaviour of a certain type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if the game object has a behaviour of the type, false otherwise.</returns>
        public bool HasBehaviourOfType(Type type)
        {
            return behaviours.Any(behaviour => behaviour.GetType() == type);
        }

        #endregion
    }
}