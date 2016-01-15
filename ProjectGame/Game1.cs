using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectGame.Voorbeeld;

namespace ProjectGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private static List<GameObject> gameObjects;
        private ICamera camera;
        private SpriteBatch spriteBatch;
        private Tilemap tilemap;
        private MouseState lastMouseState;
        private MouseState mouseState;
        private Menu mainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameObjects = new List<GameObject>();

            var xmlSerializer = new XmlSerializer(typeof(Tilemap));
            tilemap = (Tilemap)xmlSerializer.Deserialize(new FileStream("Content/Main_level.tmx", FileMode.Open));
        }


        /// <summary>
        /// Takes a moving object and determines the new position for it based on
        /// potential collisions in the world.
        /// </summary>
        /// <param name="gameObject">The game object</param>
        /// <param name="displacement">The movement</param>
        /// <returns>The new position</returns>
        public static Vector2 ResolveWorldCollision(GameObject gameObject, Vector2 displacement)
        {
            var oldPosition = new Vector2(gameObject.Position.X, gameObject.Position.Y);
            var newPosition = oldPosition + displacement;
            var thisRectangle = new Rectangle((int)newPosition.X, (int)newPosition.Y, gameObject.Size.X, gameObject.Size.Y);

            var isColliding = false;
            foreach (var otherObject in gameObjects)
            {
                if (!otherObject.IsCollidable || otherObject == gameObject)
                    continue;

                var otherRectangle = new Rectangle((int)otherObject.Position.X, (int)otherObject.Position.Y,
                        otherObject.Size.X, otherObject.Size.Y);

                if (thisRectangle.Intersects(otherRectangle))
                {
                    isColliding = true;
                    break;
                }
            }
            return isColliding ? oldPosition : newPosition;
        }

        /// <summary>
        ///     Sends CollisionEnter and CollisionExit messages to the game objects that enter and exit a collision
        ///     with another game object respectively.
        /// </summary>
        private void CheckCollisions()
        {
            if (gameObjects.Count < 2)
                return;

            for (var i = 0; i < gameObjects.Count - 1; ++i)
            {
                var a = gameObjects[i];
                if (!a.IsCollidable)
                    continue;

                var rectangleA = new Rectangle((int)a.Position.X, (int)a.Position.Y, a.Size.X, a.Size.Y);

                for (var j = i + 1; j < gameObjects.Count; ++j)
                {
                    var b = gameObjects[j];
                    if (a == b || !b.IsCollidable)
                        continue;

                    var rectangleB = new Rectangle((int)b.Position.X, (int)b.Position.Y, b.Size.X, b.Size.Y);
                    if (rectangleA.Intersects(rectangleB))
                    {
                        if (!a.CollidingGameObjects.Contains(b))
                        {
                            a.OnMessage(new CollisionEnterMessage(b));
                            a.CollidingGameObjects.Add(b);
                        }
                        if (b.CollidingGameObjects.Contains(a)) continue;
                        b.OnMessage(new CollisionEnterMessage(a));
                        b.CollidingGameObjects.Add(a);

                    }
                    else
                    {
                        if (a.CollidingGameObjects.Contains(b))
                        {
                            a.OnMessage(new CollisionExitMessage(b));
                            a.CollidingGameObjects.Remove(b);
                        }
                        if (!b.CollidingGameObjects.Contains(a)) continue;
                        b.OnMessage(new CollisionExitMessage(a));
                        b.CollidingGameObjects.Remove(a);
                    }
                }
            }
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add game objects that aren't rendered here
            base.Initialize();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Resources
            var playerTexture = Content.Load<Texture2D>("player/basicperson0");
            var monsterTexture = Content.Load<Texture2D>("Roman");
            var swordTexture = Content.Load<Texture2D>("sword1");
            var helmetTexture = Content.Load<Texture2D>("Head");

            List<Texture2D> playerAnimations = new List<Texture2D>();
            for (int i = 0; i < 7; i++)
            {
                playerAnimations.Add(Content.Load<Texture2D>("player/basicperson" + i));
            }


            if (tilemap != null)
                tilemap.Build(Content);

            // Add Game Objects
            var somePlayer = new GameObject
            {
                Position = new Vector2(200, 300),
                Texture = playerTexture
            };
            somePlayer.AddBehaviour(new MovementBehaviour(playerAnimations));

            var someMonster = new GameObject()
            {
                Position = new Vector2(20, 20),
                Texture = monsterTexture
            };

            var someHelmet = new GameObject(true, false)
            {
                Texture = helmetTexture
            };


            /*TEST SHIT FOR FOV*/
            FOVBehavior testFOV = new FOVBehavior();
            someMonster.Rotation = 0;
            someMonster.AddBehaviour(testFOV);
            /* end test shit*/

            //someMonster.AddBehaviour(new MonsterMovementBehaviour());
            someMonster.AddBehaviour(new MovementBehaviour());
            //someMonster.AddBehaviour(new ChaseBehaviour(200.0f, somePlayer));

            someHelmet.AddBehaviour(new ChildBehaviour()
            {
                Parent = somePlayer
            });

            var swordPlayer = new GameObject(false, false)
            {
                Texture = swordTexture
            };
            swordPlayer.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = somePlayer
            });

            //var swordMonster = new GameObject(false, false)
            //{
            //    Texture = swordTexture
            //};
            //swordMonster.AddBehaviour(new WeaponBehaviour()
            //{
            //    Wielder = someMonster
            //});

            gameObjects.Add(somePlayer);
            gameObjects.Add(someHelmet);
            gameObjects.Add(someMonster);
            gameObjects.Add(swordPlayer);
            //gameObjects.Add(swordMonster);

            // Follow player with camera:
            //  ----> Remove the MonsterMovementBehaviourVB, then uncomment below to get a look at the results
            var followCamera = new FollowCamera();
            followCamera.Offset = new Vector2((float)GraphicsDevice.Viewport.Width / 2, (float)GraphicsDevice.Viewport.Height / 2);
            followCamera.Target = somePlayer;
            camera = followCamera;

            somePlayer.AddBehaviour(new InputMovementBehaviour(movementSpeed: 5, camera: camera));

            mainMenu = new Menu(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || mainMenu.state == Menu.GameState.Exit)
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && mainMenu.state != Menu.GameState.Menu)
            {
                mainMenu.state = Menu.GameState.Menu;
            }
            

            // TODO: Add your update logic here
            if (mainMenu.state != Menu.GameState.Playing)
            {
                mouseState = Mouse.GetState();
                if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    mainMenu.MouseClicked(mouseState.X, mouseState.Y);
                }
                lastMouseState = mouseState;
            }
            else
            {
                CheckCollisions();
                foreach (var gameObject in gameObjects)
                {
                    gameObject.OnUpdate(gameTime);
                }
                if (camera != null) camera.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            if (mainMenu.state != Menu.GameState.Playing)
            {
                spriteBatch.Begin();
                mainMenu.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Begin(
                    transformMatrix: camera == null ? Matrix.Identity : camera.ViewMatrix,
                    samplerState: SamplerState.PointClamp);
                tilemap.Draw(spriteBatch, camera);
                foreach (var gameObject in gameObjects.Where(gameObject => gameObject.IsDrawable))
                {
                    gameObject.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

