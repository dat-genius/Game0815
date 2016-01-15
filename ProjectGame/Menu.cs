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
        private List<Texture2D> fullscreenButton;
        private Texture2D endButton;

        private Vector2 newButtonPosition;
        private Vector2 endButtonPosition;
        private Vector2 fullscreenButtonPosition;

        private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;

        public enum GameState { Menu, Loading, Playing, Exit }
        private GameState gameState;
        public GameState state { get { return gameState; } set { gameState = value; } }

        private void LoadMenu()
        {
            newButton = Content.Load<Texture2D>("menu/New");
            endButton = Content.Load<Texture2D>("menu/End");
            fullscreenButton = new List<Texture2D>();
            for (int i = 0; i < 2; i++) { fullscreenButton.Add(Content.Load<Texture2D>("menu/fullscreen" + i)); }

            newButtonPosition = new Vector2((0.5f * 800) - 100, 200);
            fullscreenButtonPosition = new Vector2((0.5f * 800) - 100, newButtonPosition.Y + 80);
            endButtonPosition = new Vector2((0.5f * 800) - 100, fullscreenButtonPosition.Y + 80);

            state = GameState.Menu;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            Color temp = Color.White;
            for (int layer = 0; layer < 2; layer++)
            {
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

        public void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x - 5, y - 5, 10, 10);

            if (gameState == GameState.Menu)
            {
                Rectangle newButtonRect = new Rectangle((int)newButtonPosition.X, (int)newButtonPosition.Y, 200, 50);
                Rectangle fullscreenButtonRect = new Rectangle((int)fullscreenButtonPosition.X, (int)fullscreenButtonPosition.Y, 200, 50);
                Rectangle endButtonRect = new Rectangle((int)endButtonPosition.X, (int)endButtonPosition.Y, 200, 50);

                if (mouseClickRect.Intersects(newButtonRect))
                {
                    gameState = GameState.Playing;
                }
                else if (mouseClickRect.Intersects(endButtonRect))
                {
                    gameState = GameState.Exit;
                }
            }
        }
    }
}
