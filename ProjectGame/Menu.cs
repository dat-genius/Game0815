using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Threading;

namespace ProjectGame
{
    class Menu
    {
        ContentManager Content;
        private Texture2D newButton;
        private Texture2D continueButton;
        private List<Texture2D> titleSprites;
        private Texture2D endButton;

        private int deltaTime, listpos =0;

        private Vector2 newButtonPosition;
        private Vector2 endButtonPosition;
        private Vector2 continueButtonPosition;

        public enum GameState { Menu, Loading, New, Playing, Exit }
        private GameState gameState;
        public GameState state { get { return gameState; } set { gameState = value; } }

        private void LoadMenu()
        {
            newButton = Content.Load<Texture2D>("menu/New");
            endButton = Content.Load<Texture2D>("menu/End");
            continueButton = Content.Load<Texture2D>("menu/continue");
            titleSprites = new List<Texture2D>();
            for (int i = 0; i < 7; i++) { titleSprites.Add(Content.Load<Texture2D>("menu/title" + i));}

            newButtonPosition = new Vector2((0.5f * 800) - 100, 200);
            endButtonPosition = new Vector2((0.5f * 800) - 100, newButtonPosition.Y + 80);
            continueButtonPosition = new Vector2(800 - 100, 430);

            state = GameState.Loading;
        }

        public Menu(ContentManager content)
        {
            Content = content;
            LoadMenu();
        }

        public void InitMenu()
        {
            LoadMenu();
        }

        public void Update(GameTime gameTime)
        {
            if (gameState == GameState.Loading)
            {
                int delta = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if ((deltaTime - delta) > 2000)
                {
                    listpos++;
                    if (listpos >= 3)
                    {
                        listpos = 3;
                        gameState = GameState.Menu;
                    }
                    deltaTime = 0;
                }
                deltaTime += delta;
            }
            if (gameState == GameState.Menu)
            {
                listpos = 3;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleSprites[listpos], new Vector2(0, 0), Color.White);
            if (gameState == GameState.Menu)
            {
                Color temp = Color.White;
                for (int layer = 0; layer < 2; layer++)
                {
                    spriteBatch.Draw(titleSprites[listpos], new Vector2(0 - layer, 0 - layer), temp);
                    spriteBatch.Draw(newButton, newButtonPosition - new Vector2(layer, layer), temp);
                    /*
                    if (!graphics.IsFullScreen)
                    {
                        spriteBatch.Draw(fullscreenButton[0], fullscreenButtonPosition - new Vector2(layer, layer), temp);
                    }
                    else
                    {
                        spriteBatch.Draw(fullscreenButton[1], fullscreenButtonPosition - new Vector2(layer, layer), temp);
                    }*/
                    spriteBatch.Draw(endButton, endButtonPosition - new Vector2(layer, layer), temp);
                    temp = Color.Red;
                }
            }
            if (gameState == GameState.New)
            {
                spriteBatch.Draw(continueButton, continueButtonPosition, Color.Red);
            }
        }

        public void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x - 5, y - 5, 10, 10);

            if (gameState == GameState.Menu)
            {
                Rectangle newButtonRect = new Rectangle((int)newButtonPosition.X, (int)newButtonPosition.Y, 200, 50);
                Rectangle endButtonRect = new Rectangle((int)endButtonPosition.X, (int)endButtonPosition.Y, 200, 50);

                if (mouseClickRect.Intersects(newButtonRect))
                {
                    gameState = GameState.New;
                    listpos = 4;
                }
                else if (mouseClickRect.Intersects(endButtonRect))
                {
                    gameState = GameState.Exit;
                }
            }
            if (gameState == GameState.New)
            {
                Rectangle continueButtonRect = new Rectangle((int)continueButtonPosition.X, (int)continueButtonPosition.Y, 200, 50);
                if (mouseClickRect.Intersects(continueButtonRect))
                {
                    listpos++;
                    if (listpos >= 7)
                    {
                        gameState = GameState.Playing;
                        listpos = 3;
                    }
                }
            }
        }
    }
}
