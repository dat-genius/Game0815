﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System;
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
        private static ICamera camera;
        private SpriteBatch spriteBatch;
        private static Tilemap tilemap;
        private MouseState lastMouseState;
        private MouseState mouseState;
        private Menu mainMenu;
        private SpriteFont textFont;

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
        /// potential collisions in the world with other objects.
        /// </summary>
        /// <param name="gameObject">The game object</param>
        /// <param name="displacement">The movement</param>
        /// <returns>The new position</returns>
        public static bool HasObjectCollision(GameObject gameObject, Vector2 displacement)
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
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Takes a moving object and determines the new position for it based on
        /// potential collisions in the world with the map.
        /// </summary>
        /// <param name="gameObject">The game object</param>
        /// <param name="displacement">The movement</param>
        /// <returns>The new position</returns>
        public static bool HasMapCollision(GameObject gameObject, Vector2 displacement)
        {
            var oldPosition = new Vector2(gameObject.Position.X, gameObject.Position.Y);
            var newPosition = oldPosition + displacement;
            var thisRectangle = new Rectangle((int)newPosition.X, (int)newPosition.Y, gameObject.Size.X, gameObject.Size.Y);

            var isColliding = false;
            foreach (var objectGroup in tilemap.ObjectGroups)
            {
                foreach (var otherObject in objectGroup.Objects)
                {
                    var otherRectangle = new Rectangle(
                            otherObject.X,
                            otherObject.Y,
                            otherObject.Width, 
                            otherObject.Height);

                    if (thisRectangle.Intersects(otherRectangle))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Vector2 ResolveWorldCollision(GameObject gameObject, Vector2 displacement)
        {
            if (!HasObjectCollision(gameObject, displacement) && !HasMapCollision(gameObject, displacement))
            {
                return gameObject.Position + displacement;
            }
            return gameObject.Position;
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
            // Create a new SpriteBatch, which can be used to Draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Resources
            var playerTexture = Content.Load<Texture2D>("player/basicperson0");
            var monsterTexture = Content.Load<Texture2D>("Roman");
            var swordTexture = Content.Load<Texture2D>("sword1");
            var helmetTexture = Content.Load<Texture2D>("Head");
            List<Texture2D> monsterHelmets = new List<Texture2D>();
            for (int i = 0; i < 3; i++)
            {
                monsterHelmets.Add(Content.Load<Texture2D>("helmets/helm" + i));
            }       
            var boss1 = Content.Load<Texture2D>("Boss1");
            var boss2 = Content.Load<Texture2D>("Boss2");
            var boss3 = Content.Load<Texture2D>("Boss3");
            var boss4 = Content.Load<Texture2D>("Boss4");
            var khan = Content.Load<Texture2D>("khan");
            var bossStart = Content.Load<Texture2D>("BossLopen/BossLopen0");
            textFont = Content.Load<SpriteFont>("TextFont");

            List<Texture2D> bossAnimations = new List<Texture2D>();
            for(int i =0; i < 8; i++)
            {
                bossAnimations.Add(Content.Load<Texture2D>("BossLopen/BossLopen" + i));
            }
            
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
                Position = new Vector2(1216, 2976),
                Texture = playerTexture
            };
            somePlayer.AddBehaviour(new MovementBehaviour(playerAnimations));
            somePlayer.AddBehaviour(new StatBehaviour(100, 200, 0.5f));
            somePlayer.AddBehaviour(new HUDBehaviour(
                Content.Load<Texture2D>("HealthBar"),
                Content.Load<Texture2D>("TestosBar"),
                Content.Load<SpriteFont>("textFont"),
                somePlayer));
            var someHelmet = new GameObject(true, false)
            {
                Texture = helmetTexture
            };
            
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

            somePlayer.AddBehaviour(new AttackBehaviour(swordPlayer));
            somePlayer.AddBehaviour(new PlayerHitBehaviour(swordPlayer));

            //------test--------
            GameObject someMonster = new GameObject()
            {
                Position = new Vector2(1216,  3100),
                Texture = monsterTexture
            };

            var swordMonster = new GameObject(false, false)
            {
                Texture = swordTexture
            };
            swordMonster.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = someMonster
            });

            FOVBehavior FOV = new FOVBehavior(gameObjects);
            someMonster.AddBehaviour(FOV);
            someMonster.AddBehaviour(new MovementBehaviour());
            someMonster.AddBehaviour(new MonsterAttack(somePlayer));
            someMonster.AddBehaviour(new AttackBehaviour(swordMonster));
            someMonster.AddBehaviour(new StatBehaviour(50, 100, 0.1f));
            //someMonster.AddBehaviour(new ChaseBehaviour(200, somePlayer));
            //someMonster.AddBehaviour(new MonsterMovementBehaviour(someMonster.Position)
            //{
            //    Sword = swordMonster
            //});

            gameObjects.Add(someMonster);
            gameObjects.Add(swordMonster);
            //-----------einde test--------------------------------------

            SpawnMonsters(100, somePlayer, playerAnimations, monsterHelmets, swordTexture);
            //LoadMonster(new Vector2(65,160), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(40,160), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(40, 218), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(40, 110), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(104, 48), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(150, 75), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(177, 71), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(219, 46), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(160, 101), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(201, 113), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(224, 159), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(214, 195), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(160, 225), somePlayer, monsterTexture, swordTexture);
            //LoadMonster(new Vector2(150, 270), somePlayer, monsterTexture, swordTexture);

            var testBoss = new GameObject()
            {
                Position = new Vector2(1216,3500),
                Texture = bossStart
            };
            var swordboss = new GameObject(false, false)
                {
                    Texture = swordTexture
                };
            swordboss.AddBehaviour(new WeaponBehaviour()
                {
                    Wielder = testBoss
                });
            var bossHelmet = new GameObject(true, false)
            {
                Texture = boss2
            };
            bossHelmet.AddBehaviour(new ChildBehaviour()
                {
                    Parent = testBoss
                });

            testBoss.AddBehaviour(new MovementBehaviour(bossAnimations));
            testBoss.AddBehaviour(new MonsterAttack(somePlayer));
            testBoss.AddBehaviour(new AttackBehaviour(swordboss));
            testBoss.AddBehaviour(new FOVBehavior(gameObjects));
            testBoss.AddBehaviour(new StatBehaviour(600, 100, 0.1f));
            testBoss.AddBehaviour(new MonsterMovementBehaviour(testBoss.Position)
                {
                    Sword = swordboss
                });
            
            
            gameObjects.Add(somePlayer);
            gameObjects.Add(someHelmet);
            gameObjects.Add(swordPlayer);
            gameObjects.Add(swordboss);
            gameObjects.Add(testBoss);
            gameObjects.Add(bossHelmet);


            // Follow player with camera:
            //  ----> Remove the MonsterMovementBehaviourVB, then uncomment below to get a look at the results
            var followCamera = new FollowCamera();
            followCamera.Offset = new Vector2((float)GraphicsDevice.Viewport.Width / 2, (float)GraphicsDevice.Viewport.Height / 2);
            followCamera.Target = somePlayer;
            camera = followCamera;

            somePlayer.AddBehaviour(new InputMovementBehaviour(movementSpeed: 5, camera: camera));
            mainMenu = new Menu(Content);
            //someMonster.Position = somePlayer.Position - new Vector2(100, 100);
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || mainMenu.State == Menu.GameState.Exit)
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && mainMenu.State != Menu.GameState.Menu)
            {
                mainMenu.State = Menu.GameState.Menu;		
            }
            
            // TODO: Add your update logic here
            if (mainMenu.State != Menu.GameState.Playing)		
            {		         
                mouseState = Mouse.GetState();	
                if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)		
                {		
                    mainMenu.MouseClicked(mouseState.X, mouseState.Y);		
                }		
                lastMouseState = mouseState;		
            }    
            else{		
                CheckCollisions();		
                foreach (var gameObject in gameObjects){		
                    gameObject.OnUpdate(gameTime);		
                }		
                if (camera != null) camera.Update(gameTime);		
            }
            mainMenu.Update(gameTime);
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should Draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            if (mainMenu.State != Menu.GameState.Playing){
                spriteBatch.Begin();
                mainMenu.Draw(spriteBatch);
            }
            else {
                spriteBatch.Begin(
                    transformMatrix: camera == null ? Matrix.Identity : camera.ViewMatrix,
                    samplerState: SamplerState.PointClamp);
                    tilemap.Draw(spriteBatch, camera);
            
            
                foreach (var gameObject in gameObjects.Where(gameObject => gameObject.IsDrawable)) {
                    gameObject.Draw(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin();
                foreach (var gameObject in gameObjects)
                {
                    if(gameObject.HasBehaviourOfType(typeof(HUDBehaviour)))
                    {
                        HUDBehaviour hud = gameObject.GetBehaviourOfType(typeof(HUDBehaviour)) as HUDBehaviour;
                        hud.Draw(spriteBatch);
                    }
                }
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void LoadMonster(Vector2 position,GameObject target, Texture2D monsterTexture, Texture2D swordTexture)
        {
            //var monsterTexture = Content.Load<Texture2D>("Roman");
            //var swordTexture = Content.Load<Texture2D>("sword1");
            GameObject someMonster = new GameObject()
            {
                Position = new Vector2(position.X *32,position.Y *32),
                Texture = monsterTexture
            };

            GameObject swordMonster = new GameObject(false, false)
            {
                Texture = swordTexture
            };
            swordMonster.AddBehaviour(new WeaponBehaviour()
            {
                Wielder = someMonster
            });

            FOVBehavior FOV = new FOVBehavior(gameObjects);
            someMonster.AddBehaviour(FOV);
            someMonster.AddBehaviour(new MovementBehaviour());
            someMonster.AddBehaviour(new MonsterAttack(target));
            someMonster.AddBehaviour(new AttackBehaviour(swordMonster));
            someMonster.AddBehaviour(new ChaseBehaviour(200.0f, target));

            gameObjects.Add(someMonster);
            gameObjects.Add(swordMonster);
        }

        public void SpawnMonsters(uint i, GameObject target, List<Texture2D> monstertexture, List<Texture2D> helmet, Texture2D swordTexture)
        {
            List<GameObject> spawnlist = new List<GameObject>();
            Random r = new Random();
            while (spawnlist.Count <= i)
            {
                GameObject monster = new GameObject();
                monster.Position = new Vector2(r.Next(0, tilemap.Width * 32), r.Next(0, tilemap.Height * 32));
                monster.Texture = monstertexture[0];
                if (CollisionFree(monster))
                {
                    var swordMonster = new GameObject(false, false)
                    {
                        Texture = swordTexture
                    };
                    swordMonster.AddBehaviour(new WeaponBehaviour()
                    {
                        Wielder = monster
                    });

                    FOVBehavior FOV = new FOVBehavior(gameObjects);
                    monster.AddBehaviour(FOV);
                    monster.AddBehaviour(new MovementBehaviour());
                    monster.AddBehaviour(new MonsterMovementBehaviour(monster.Position) { Sword = swordMonster });
                    monster.AddBehaviour(new MonsterAttack(target));
                    monster.AddBehaviour(new AttackBehaviour(swordMonster));
                    monster.AddBehaviour(new StatBehaviour(5, 100, 0.1f));
                    monster.AddBehaviour(new ChaseBehaviour(200, target));

                    GameObject Helmet = new GameObject();
                    Helmet.Texture = helmet[r.Next(0,helmet.Count)];
                    ChildBehaviour helmetbehaviour = new ChildBehaviour();
                    helmetbehaviour.Parent = monster;
                    Helmet.AddBehaviour(helmetbehaviour);
                    spawnlist.Add(monster);

                    gameObjects.Add(monster);
                    gameObjects.Add(Helmet);
                    gameObjects.Add(swordMonster);
                }
            }
        }

        private bool CollisionFree(GameObject botsing)
        {
            if (HasMapCollision(botsing, new Vector2(0, 0)))
            {
                return false;
            }
            if (HasObjectCollision(botsing, new Vector2(0, 0)))
            {
                return false;
            }
            return true;
        }
    }
}
