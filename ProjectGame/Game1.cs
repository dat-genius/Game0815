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
        private readonly List<GameObject> gameObjects;
        private ICamera camera;
        private SpriteBatch spriteBatch;
        private Tilemap tilemap;

        //menu
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D saveButton;
        private Texture2D loadButton;

        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 saveButtonPosition;
        private Vector2 loadButtonPosition;

        //private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;

        enum GameState { StartMenu, Loading, Playing, Saving }
        private GameState gameState;
        //endmenu


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameObjects = new List<GameObject>();

            var xmlSerializer = new XmlSerializer(typeof(Tilemap));
            tilemap = (Tilemap)xmlSerializer.Deserialize(new FileStream("Content/Main_level.tmx", FileMode.Open));
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
                        DoNotWalkTrough(a, PlaceCollision(rectangleA, rectangleB));
                        DoNotWalkTrough(b, PlaceCollision(rectangleB, rectangleA));
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

            startButtonPosition = new Vector2((GraphicsDevice.DisplayMode.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.DisplayMode.Width / 2) - 50, 280);
            saveButtonPosition = new Vector2((GraphicsDevice.DisplayMode.Width / 2) - 50, 360);
            loadButtonPosition = new Vector2((GraphicsDevice.DisplayMode.Width / 2) - 50, 440);

            //set the gamestate to start menu
            gameState = GameState.StartMenu;
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
            startButton = Content.Load<Texture2D>("Start");
            exitButton = Content.Load<Texture2D>("Exit");
            saveButton = Content.Load<Texture2D>("Save");
            loadButton = Content.Load<Texture2D>("Load");
            var playerTexture = Content.Load<Texture2D>("basicperson0");
            var monsterTexture = Content.Load<Texture2D>("Roman");
            var swordTexture = Content.Load<Texture2D>("sword1");
            var helmetTexture = Content.Load<Texture2D>("Head");

            List<Texture2D> playerAnimations = new List<Texture2D>();
            for (int i = 0; i < 7; i++)
            {
                playerAnimations.Add(Content.Load<Texture2D>("basicperson" + i));
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

            var someHelmet = new GameObject()
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
            swordPlayer.AddBehaviour(new WeaponBehaviour(true)
            {
                Wielder = somePlayer
            });

            var swordMonster = new GameObject(false, false)
            {
                Texture = swordTexture
            };
            swordMonster.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = someMonster
            });

            someMonster.AddBehaviour(new MonsterAttack(somePlayer, swordMonster));
            gameObjects.Add(somePlayer);
            gameObjects.Add(someHelmet);
            gameObjects.Add(someMonster);
            gameObjects.Add(swordPlayer);
            gameObjects.Add(swordMonster);

            // Follow player with camera:
            //  ----> Remove the MonsterMovementBehaviourVB, then uncomment below to get a look at the results
            var followCamera = new FollowCamera();
            followCamera.Offset = new Vector2((float)GraphicsDevice.Viewport.Width / 2, (float)GraphicsDevice.Viewport.Height / 2);
            followCamera.Target = somePlayer;
            camera = followCamera;

            somePlayer.AddBehaviour(new InputMovementBehaviour(movementSpeed: 5, camera: camera));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed /*|| Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            if (gameState == GameState.StartMenu)
            {
                mouseState = Mouse.GetState();
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;
            }

            if (gameState == GameState.Playing && isLoading)
            {
                //LoadGame();
                isLoading = false;
            }

            if (gameState == GameState.Playing)
            {
                // TODO: Add your update logic here
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

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                spriteBatch.Draw(saveButton, saveButtonPosition, Color.White);
                spriteBatch.Draw(loadButton, loadButtonPosition, Color.White);
            }
            //draw the the game when playing
            if (gameState == GameState.Playing)
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

        /// <summary>
        /// Makes sure that the objects don't move trough eachother
        /// </summary>
        /// <param name="gameObject">Is the object what you want to stop </param>
        /// <param name="position">An int what provide the side of the collision </param>
        private void DoNotWalkTrough(GameObject gameObject, int position)
        {
            if (!gameObject.HasBehaviourOfType(typeof(MovementBehaviour))) return;
            var behaviour = gameObject.GetBehaviourOfType(typeof(MovementBehaviour));

            switch (position)
            {
                case 1:
                    (behaviour as MovementBehaviour).CollisionLeft = true;
                    break;
                case 2:
                    (behaviour as MovementBehaviour).CollisionRight = true;
                    break;
                case 3:
                    (behaviour as MovementBehaviour).CollisionTop = true;
                    break;
                case 4:
                    (behaviour as MovementBehaviour).CollisionBottom = true;
                    break;
            }

            if (gameObject.HasBehaviourOfType(typeof(MonsterMovementBehaviour)))
            {
                behaviour = gameObject.GetBehaviourOfType(typeof(MonsterMovementBehaviour));
                (behaviour as MonsterMovementBehaviour).Collision = true;
            }

            if (gameObject.HasBehaviourOfType(typeof(ChaseBehaviour)))
            {
                behaviour = gameObject.GetBehaviourOfType(typeof(ChaseBehaviour));
                (behaviour as ChaseBehaviour).Collision = true;
            }
        }


        /// <summary>
        /// calculates at what side the collision was
        /// </summary>
        /// <param name="a">firts rectangle, it wil be calculated for this rectangle</param>
        /// <param name="b">second rectangle, the rectangle withs collides with a </param>
        /// <returns>1 = left, 2 = right, 3 = top, 4 = bottom</returns>
        private int PlaceCollision(Rectangle a, Rectangle b)
        {
            var MidAx = (a.Right + a.Left) / 2;
            var MidAy = (a.Top + a.Bottom) / 2;

            if (b.Right < MidAx)
                return 1;
            if (b.Left > MidAx)
                return 2;
            if (b.Bottom < MidAy)
                return 3;
            if (b.Top > MidAy)
                return 4;

            return 0;
        }

        private void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 200, 50);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 200, 50);
                Rectangle saveButtonRect = new Rectangle((int)saveButtonPosition.X, (int)saveButtonPosition.Y, 200, 50);
                Rectangle loadButtonRect = new Rectangle((int)loadButtonPosition.X, (int)loadButtonPosition.Y, 200, 50);

                if (mouseClickRect.Intersects(startButtonRect))
                {
                    gameState = GameState.Playing;
                    isLoading = true;
                }
                else if (mouseClickRect.Intersects(exitButtonRect))
                {
                    Exit();
                }
                else if (mouseClickRect.Intersects(saveButtonRect))
                {
                    //to be implemented
                }
                else if (mouseClickRect.Intersects(loadButtonRect))
                {
                    //to be implemented
                }
            }
        }
    }
}
