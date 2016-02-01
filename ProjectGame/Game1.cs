using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private static List<GameObject> gameObjects;
        private List<GameObject> portBlocks;
        private static ICamera camera;
        private SpriteBatch spriteBatch;
        private static Tilemap tilemap;
        private List<Tilemap> bossrooms;
        private Tilemap currentMap;
        private Tilemap lastMap;
        private MouseState lastMouseState;
        private MouseState mouseState;
        private Menu mainMenu;
        private SpriteFont textFont;
        public GameObject somePlayer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            var xmlSerializer = new XmlSerializer(typeof(Tilemap));

            tilemap = (Tilemap)xmlSerializer.Deserialize(new FileStream("Content/Main_level.tmx", FileMode.Open));
            
            bossrooms = new List<Tilemap>();
            for (int i = 0; i < 4; i++)
            {
                bossrooms.Add((Tilemap)xmlSerializer.Deserialize(new FileStream("Content/bossroom" + i + ".tmx", FileMode.Open)));
            }
             

            currentMap = tilemap;
            lastMap = currentMap;
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
                    if (a == b || (!b.IsCollidable && !b.HasBehaviourOfType("TeleportBlockBehaviour")))
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

        private void CheckTeleport()
        {
            GameObject player = new GameObject();
            foreach (GameObject potentialPlayer in gameObjects)
            {
                if (potentialPlayer.HasBehaviourOfType("InputMovementBehaviour"))
                {
                    player = potentialPlayer;
                    break;
                }
            }
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Size.X, player.Size.Y);
            foreach (GameObject teleportblock in portBlocks)
            {
                Rectangle portblockRect = new Rectangle((int)teleportblock.Position.X, (int)teleportblock.Position.Y, teleportblock.Size.X, teleportblock.Size.Y);
                if (playerRect.Intersects(portblockRect))
                {
                    teleportblock.OnMessage(new CollisionEnterMessage(player));
                    break;
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
            mainMenu = new Menu(Content);
            //someMonster.Position = somePlayer.Position - new Vector2(100, 100);
        }

        private void LoadGameObjects()
        {
            gameObjects = new List<GameObject>();
            var playerTexture = Content.Load<Texture2D>("player/basicperson0");
            var monsterTexture = Content.Load<Texture2D>("Roman");
            var swordTexture = Content.Load<Texture2D>("sword1");
            var shieldTexture = Content.Load<Texture2D>("shield2");
            var helmetTexture = Content.Load<Texture2D>("Head");
            List<Texture2D> monsterHelmets = new List<Texture2D>();
            for (int i = 0; i < 3; i++)
            {
                monsterHelmets.Add(Content.Load<Texture2D>("helmets/helm" + i));
            }
            var boss1Texture = Content.Load<Texture2D>("Boss1");
            var boss2Texture = Content.Load<Texture2D>("Boss2");
            var boss3Texture = Content.Load<Texture2D>("Boss3");
            var boss4Texture = Content.Load<Texture2D>("Boss4");
            var khanTexture = Content.Load<Texture2D>("khan");
            var bossStart = Content.Load<Texture2D>("BossLopen/BossLopen0");
            var swordBoss1Texture = Content.Load<Texture2D>("Sword_Boss1");
            var swordBoss2Texture = Content.Load<Texture2D>("Sword_Boss2");
            var swordBoss3Texture = Content.Load<Texture2D>("Sword_Boss3");
            var swordBoss4Texture = Content.Load<Texture2D>("Sword_Boss4");
            textFont = Content.Load<SpriteFont>("TextFont");

            List<Texture2D> bossAnimations = new List<Texture2D>();
            for (int i = 0; i < 8; i++)
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
            somePlayer = new GameObject
            {
                Position = new Vector2(200, 200),
                Texture = playerTexture
            };
            mainMenu.PlayerAlive = true;
            somePlayer.AddBehaviour("MovementBehaviour", new MovementBehaviour(playerAnimations));
            somePlayer.AddBehaviour("StatBehaviour", new StatBehaviour(100, 200, 0.5f));
            somePlayer.AddBehaviour("HUDBehaviour", new HUDBehaviour(
                Content.Load<Texture2D>("HealthBar"),
                Content.Load<Texture2D>("TestosBar"),
                Content.Load<SpriteFont>("textFont"),
                somePlayer));
            var someHelmet = new GameObject(true, false)
            {
                Texture = helmetTexture
            };

            someHelmet.AddBehaviour("ChildBehaviour", new ChildBehaviour()
            {
                Parent = somePlayer
            });

            var swordPlayer = new GameObject(false, false)
            {
                Texture = swordTexture
            };
            swordPlayer.AddBehaviour("WeaponBehaviour", new WeaponBehaviour()
            {
                Wielder = somePlayer
            });

            somePlayer.AddBehaviour("AttackBehaviour", new AttackBehaviour(swordPlayer));
            somePlayer.AddBehaviour("HitBehaviour", new HitBehaviour(swordPlayer, swordBoss1Texture));
            somePlayer.AddBehaviour("BondBehaviour", new BondBehaviour(swordPlayer, someHelmet));

            var shieldPlayer = new ShieldBehaviour(shieldTexture);
            somePlayer.AddBehaviour("ShieldBehaviour", shieldPlayer);

            portBlocks = new List<GameObject>();
            GameObject teleport = new GameObject();
            teleport.AddBehaviour("TeleportBlockBehaviour", new TeleportBlockBehaviour(new Vector2(40 * 32, 236 * 32)));
            teleport.Position = new Vector2(39 * 32, 73 * 32);
            teleport.Size = new Point(96, 32);
            teleport.IsCollidable = false;
            portBlocks.Add(teleport);

            
            if(currentMap == tilemap)
                SpawnMonsters(50, somePlayer, playerAnimations, monsterHelmets, swordTexture);
            if(currentMap == bossrooms[0])
                LoadBoss4(somePlayer, boss4Texture, swordBoss4Texture);
            if(currentMap == bossrooms[1])
                LoadBoss1_3(somePlayer, boss1Texture, swordBoss1Texture, 1);
            if(currentMap == bossrooms[2])
                LoadBoss2(somePlayer, boss2Texture, bossAnimations, swordBoss2Texture);
            if(currentMap == bossrooms[3])
            {
                LoadBoss1_3(somePlayer, boss3Texture, swordBoss3Texture, 2);
                SpawnMonsters(5, somePlayer, playerAnimations, monsterHelmets, swordTexture);
            }
                

            //LoadBoss1_3(somePlayer, boss1Texture, swordBoss1Texture,1);
            //LoadBoss2(somePlayer, boss2Texture, bossAnimations, swordBoss2Texture);
            //LoadBoss1_3(somePlayer, boss3Texture, swordBoss3Texture, 2);    //moeten nog wel monsters omheen worden gemaakt
            //LoadBoss4(somePlayer, boss4Texture, swordBoss4Texture);
            //LoadFinalBoss(somePlayer, khanTexture, bossAnimations, swordBoss1Texture);

            gameObjects.Add(somePlayer);
            gameObjects.Add(someHelmet);
            gameObjects.Add(swordPlayer);
            gameObjects.Add(shieldPlayer.shield);




            // Follow player with camera:
            //  ----> Remove the MonsterMovementBehaviourVB, then uncomment below to get a look at the results
            var followCamera = new FollowCamera();
            followCamera.Offset = new Vector2((float)GraphicsDevice.Viewport.Width / 2, (float)GraphicsDevice.Viewport.Height / 2);
            followCamera.Target = somePlayer;
            camera = followCamera;

            somePlayer.AddBehaviour("InputMovementBehaviour", new InputMovementBehaviour(movementSpeed: 5, camera: camera));

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
                if (mainMenu.State == Menu.GameState.GameLoading)
                {
                    LoadGameObjects();
                    mainMenu.State = Menu.GameState.Playing;
                }
            }
            else
            {
                CheckCollisions();
                CheckTeleport();
                foreach (var gameObject in gameObjects)
                {
                    gameObject.OnUpdate(gameTime);
                }
                if (camera != null) camera.Update(gameTime);
                DeleteMonster();
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
            var pixelSize = SetPixelSize();

            if (mainMenu.State != Menu.GameState.Playing)
            {
                spriteBatch.Begin();
                mainMenu.Draw(spriteBatch);
            }
            else
            {
                SetCurrentMap();
                spriteBatch.Begin(
                    transformMatrix: camera == null ? Matrix.Identity : camera.ViewMatrix,
                    samplerState: SamplerState.PointClamp);
                currentMap.Draw(spriteBatch, camera, pixelSize);

                foreach (var gameObject in gameObjects.Where(gameObject => gameObject.IsDrawable))
                {
                    gameObject.Draw(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin();
                foreach (var gameObject in gameObjects)
                {
                    if (gameObject.HasBehaviourOfType("HUDBehaviour"))
                    {
                        HUDBehaviour hud = gameObject.GetBehaviourOfType("HUDBehaviour") as HUDBehaviour;
                        hud.Draw(spriteBatch);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public int SetPixelSize()
        {
            if (currentMap == bossrooms[0])
                return 16;
            if (currentMap == bossrooms[2])
                return 16;

            return 32;
        }

        public void DeleteMonster()
        {
            foreach (var gameObject in gameObjects.ToList())
            {
                if (gameObject.HasBehaviourOfType("StatBehaviour"))
                {
                    var behaviour = gameObject.GetBehaviourOfType("StatBehaviour");
                    if ((behaviour as StatBehaviour).Health <= 0)
                    {
                        if (gameObject.HasBehaviourOfType("InputMovementBehaviour"))
                        {
                            mainMenu.PlayerAlive = false;
                        }
                        if (gameObject.HasBehaviourOfType("DropItem"))
                        {
                            var Drop = gameObject.GetBehaviourOfType("DropItem") as DropItem;
                            Drop.AddPotion();
                        }
                        var behaviour2 = gameObject.GetBehaviourOfType("BondBehaviour");
                        var sword = (behaviour2 as BondBehaviour).Sword;
                        if ((behaviour2 as BondBehaviour).HasHelmet)
                        {
                            var helmet = (behaviour2 as BondBehaviour).Helmet;
                            gameObjects.Remove(helmet);

                        }
                        gameObjects.Remove(sword);
                        gameObjects.Remove(gameObject);
                    }
                }
            }
        }

        public void SpawnMonsters(uint i, GameObject target, List<Texture2D> monstertexture, List<Texture2D> helmet, Texture2D swordTexture)
        {
            List<GameObject> spawnlist = new List<GameObject>();
            Random r = new Random();
            while (spawnlist.Count <= i)
            {
                GameObject monster = new GameObject();
                monster.Position = new Vector2(r.Next(0, currentMap.Width * currentMap.TileWidth), r.Next(0, currentMap.Height * currentMap.TileHeight));
                monster.Texture = monstertexture[0];
                if (CollisionFree(monster))
                {
                    var swordMonster = new GameObject(false, false)
                    {
                        Texture = swordTexture
                    };

                    swordMonster.AddBehaviour("WeaponBehaviour", new WeaponBehaviour()
                    {
                        Wielder = monster
                    });

                    FOVBehavior FOV = new FOVBehavior(gameObjects);
                    monster.AddBehaviour("FOVBehavior", FOV);
                    monster.AddBehaviour("MovementBehaviour", new MovementBehaviour());
                    monster.AddBehaviour("MonsterAttack", new MonsterAttack(target));
                    monster.AddBehaviour("AttackBehaviour", new AttackBehaviour(swordMonster));
                    monster.AddBehaviour("StatBehaviour", new StatBehaviour(50, 100, 0.1f));
                    monster.AddBehaviour("ChaseBehaviour", new ChaseBehaviour(300, target, monster.Position));
                    monster.AddBehaviour("HitBehaviour", new HitBehaviour(swordMonster));
                    monster.AddBehaviour("DropItem", new DropItem(target));

                    GameObject Helmet = new GameObject();
                    Helmet.Texture = helmet[r.Next(0, helmet.Count)];
                    ChildBehaviour helmetbehaviour = new ChildBehaviour();
                    helmetbehaviour.Parent = monster;
                    Helmet.AddBehaviour("ChildBehaviour", helmetbehaviour);
                    spawnlist.Add(monster);
                    monster.AddBehaviour("BondBehaviour", new BondBehaviour(swordMonster, Helmet));

                    gameObjects.Add(monster);
                    gameObjects.Add(Helmet);
                    gameObjects.Add(swordMonster);
                }
            }
        }

        public void LoadBoss1_3(GameObject target, Texture2D bosstexture, Texture2D swordtexture, int bossnmmr)
        {
            GameObject Boss = new GameObject()
            {
                Position = SetPosition(bossnmmr),
                Texture = bosstexture
            };
            var bossSword = new GameObject(false, false)
            {
                Texture = swordtexture
            };
            bossSword.AddBehaviour("WeaponBehaviour", new WeaponBehaviour()
            {
                Wielder = Boss
            });

            Boss.AddBehaviour("MovementBehaviour", new MovementBehaviour());
            Boss.AddBehaviour("ChaseBehaviour", new ChaseBehaviour(300, target, Boss.Position, true));
            Boss.AddBehaviour("MonsterAttack", new MonsterAttack(target, true));
            Boss.AddBehaviour("AttackBehaviour", new AttackBehaviour(bossSword));
            Boss.AddBehaviour("StatBehaviour", new StatBehaviour(50 + 50 * bossnmmr, 100, 0.1f));
            Boss.AddBehaviour("BondBehaviour", new BondBehaviour(bossSword));
            Boss.AddBehaviour("HitBehaviour", new HitBehaviour(bossSword));

            gameObjects.Add(Boss);
            gameObjects.Add(bossSword);
        }

        private Vector2 SetPosition(int bossnmmr)
        {
            if (bossnmmr == 1)
                return new Vector2(640, 512);
            else
                return new Vector2(352, 288);
        }

        public void LoadBoss2(GameObject target, Texture2D bosstexture, List<Texture2D> movementList, Texture2D swordtexture)
        {
            GameObject Boss2 = new GameObject()
            {
                Position = new Vector2(352, 288),
                Texture = movementList[0]
            };
            var bossSword2 = new GameObject(false, false)
            {
                Texture = swordtexture
            };
            bossSword2.AddBehaviour("WeaponBehaviour", new WeaponBehaviour()
            {
                Wielder = Boss2
            });
            var bossHelmet = new GameObject(true, false)
            {
                Texture = bosstexture
            };
            bossHelmet.AddBehaviour("ChildBehaviour", new ChildBehaviour()
            {
                Parent = Boss2
            });

            Boss2.AddBehaviour("MovementBehaviour", new MovementBehaviour());
            Boss2.AddBehaviour("ChaseBehaviour", new ChaseBehaviour(300, target, Boss2.Position, true));
            Boss2.AddBehaviour("MonsterAttack", new MonsterAttack(target, true));
            Boss2.AddBehaviour("AttackBehaviour", new AttackBehaviour(bossSword2));
            Boss2.AddBehaviour("FOVBehavior", new FOVBehavior(gameObjects));
            Boss2.AddBehaviour("StatBehaviour", new StatBehaviour(120, 100, 0.1f)
            {
                HealthRegenSword = true
            });
            Boss2.AddBehaviour("BondBehaviour", new BondBehaviour(bossSword2));
            Boss2.AddBehaviour("HitBehaviour", new HitBehaviour(bossSword2));

            gameObjects.Add(Boss2);
            gameObjects.Add(bossSword2);
            gameObjects.Add(bossHelmet);
        }

        public void LoadBoss4(GameObject target, Texture2D bosstexture, Texture2D swordtexture)
        {
            GameObject Boss = new GameObject()
            {
                Position = new Vector2(272, 464),
                Texture = bosstexture
            };
            var bossSword = new GameObject(false, false)
            {
                Texture = swordtexture
            };
            bossSword.AddBehaviour("WeaponBehaviour", new WeaponBehaviour()
            {
                Wielder = Boss
            });

            Boss.AddBehaviour("MovementBehaviour", new MovementBehaviour());
            Boss.AddBehaviour("ChaseBehaviour", new ChaseBehaviour(300, target, Boss.Position, true));
            Boss.AddBehaviour("MonsterAttack", new MonsterAttack(target, true));
            Boss.AddBehaviour("AttackBehaviour", new AttackBehaviour(bossSword));
            Boss.AddBehaviour("FOVBehavior", new FOVBehavior(gameObjects));
            Boss.AddBehaviour("StatBehaviour", new StatBehaviour(200, 100, 0.1f));
            Boss.AddBehaviour("BondBehaviour", new BondBehaviour(bossSword));
            Boss.AddBehaviour("HitBehaviour", new HitBehaviour(bossSword, true));

            gameObjects.Add(Boss);
            gameObjects.Add(bossSword);
        }
        public void LoadFinalBoss(GameObject target, Texture2D bosstexture, List<Texture2D> movementList, Texture2D swordtexture)
        {
            GameObject FinalBoss = new GameObject()
            {
                Position = new Vector2(272, 464),
                Texture = movementList[0]
            };
            var helmet = new GameObject(true, false)
            {
                Texture = bosstexture
            };
            helmet.AddBehaviour("ChildBehaviour", new ChildBehaviour()
            {
                Parent = FinalBoss
            });
            var bossSword = new GameObject(false, false)
            {
                Texture = swordtexture
            };
            bossSword.AddBehaviour("WeaponBehaviour", new WeaponBehaviour()
            {
                Wielder = FinalBoss
            });

            FinalBoss.AddBehaviour("MovementBehaviour", new MovementBehaviour(movementList));
            FinalBoss.AddBehaviour("ChaseBehaviour", new ChaseBehaviour(300, target, FinalBoss.Position, true));
            FinalBoss.AddBehaviour("MonsterAttack", new MonsterAttack(target, true));
            FinalBoss.AddBehaviour("AttackBehaviour", new AttackBehaviour(bossSword));
            FinalBoss.AddBehaviour("StatBehaviour", new StatBehaviour(250, 100, 0.1f)
            {
                HealthRegenSword = true
            });
            FinalBoss.AddBehaviour("FOVBehavior", new FOVBehavior(gameObjects));
            FinalBoss.AddBehaviour("BondBehaviour", new BondBehaviour(bossSword, helmet));
            FinalBoss.AddBehaviour("HitBehaviour", new HitBehaviour(bossSword, true));

            gameObjects.Add(FinalBoss);
            gameObjects.Add(helmet);
            gameObjects.Add(bossSword);
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

        private void SetCurrentMap()
        {
            var positionPLayer = somePlayer.Position;
            var differnce = new Vector2(896, 3296).Length() - positionPLayer.Length();
            if (differnce <= 20)
            {
                currentMap = bossrooms[0];
                bossrooms[0].Build(Content);
                LoadGameObjects();
            }
        }

    }
}