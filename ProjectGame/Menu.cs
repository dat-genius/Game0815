using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Threading;

namespace ProjectGame
{
    public class Menu
    {
        private readonly ContentManager Content;
        private Texture2D newButton;
        private List<Texture2D> titleSprites;
        private Texture2D howButton;
        private Texture2D endButton;
        private Texture2D continueButton;

        private int deltaTime;
        private int listpos;

        private Vector2 newButtonPosition;
        private Vector2 endButtonPosition;
        private Vector2 howButtonPosition;
        private Vector2 continueGameButtonPosition;
        private Vector2 continueButtonPosition;

        public enum GameState { Menu, GameLoading, Loading, New, Playing, How, Exit }
        public GameState State { get; set; }
        public bool PlayerAlive { get; set; }

        private void LoadMenu()
        {
            newButton = Content.Load<Texture2D>("menu/New");
            endButton = Content.Load<Texture2D>("menu/End");
            howButton = Content.Load<Texture2D>("Menu/How");
            continueButton = Content.Load<Texture2D>("menu/continuegame");
            titleSprites = new List<Texture2D>();
            for (int i = 0; i < 9; i++) { titleSprites.Add(Content.Load<Texture2D>("menu/title" + i)); }

            newButtonPosition = new Vector2((0.5f * 800) - 100, 200);
            howButtonPosition = new Vector2((0.5f * 800) - 100, newButtonPosition.Y + 60);
            endButtonPosition = new Vector2((0.5f * 800) - 100, howButtonPosition.Y + 60);
            continueGameButtonPosition = new Vector2((0.5f * 800) - 100, newButtonPosition.Y - 60);
            continueButtonPosition = new Vector2(800 - 100, 450);

            State = GameState.Loading;
        }

        public Menu(ContentManager content)
        {
            Content = content;
            LoadMenu();
        }

        public void InitMenu()
        {
            PlayerAlive = false;
            LoadMenu();
        }

        public void Update(GameTime gameTime)
        {
            if (State == GameState.Loading)
            {
                int delta = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if ((deltaTime - delta) > 2000)
                {
                    listpos++;
                    if (listpos >= 3)
                    {
                        listpos = 3;
                        State = GameState.Menu;
                    }
                    deltaTime = 0;
                }
                deltaTime += delta;
            }
            if (State == GameState.Menu)
            {
                listpos = 3;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleSprites[listpos], new Vector2(0, 0), Color.White);
            if (State == GameState.Menu)
            {
                Color temp = Color.White;
                for (int layer = 0; layer < 2; layer++)
                {
                    spriteBatch.Draw(titleSprites[listpos], new Vector2(0 - layer, 0 - layer), temp);
                    if (PlayerAlive)
                    {
                        spriteBatch.Draw(continueButton, continueGameButtonPosition - new Vector2(layer, layer), temp);
                    }
                    spriteBatch.Draw(newButton, newButtonPosition - new Vector2(layer, layer), temp);
                    spriteBatch.Draw(howButton, howButtonPosition - new Vector2(layer, layer), temp);
                    spriteBatch.Draw(endButton, endButtonPosition - new Vector2(layer, layer), temp);
                    temp = Color.Red;
                }
            }
            if (State == GameState.New)
            {
                spriteBatch.Draw(continueButton, new Rectangle((int)continueButtonPosition.X, (int)continueButtonPosition.Y, 100, 25), Color.Red);
            }
            if (State == GameState.How)
            {
                spriteBatch.Draw(titleSprites[8], new Vector2(0, 0), Color.White);
            }
        }

        public void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x - 5, y - 5, 10, 10);

            if (State == GameState.Menu)
            {
                Rectangle newButtonRect = new Rectangle((int)newButtonPosition.X, (int)newButtonPosition.Y, 200, 50);
                Rectangle continueGameButtonRect = new Rectangle((int)continueGameButtonPosition.X, (int)continueGameButtonPosition.Y, 200, 50);
                Rectangle endButtonRect = new Rectangle((int)endButtonPosition.X, (int)endButtonPosition.Y, 200, 50);
                Rectangle howButtonRect = new Rectangle((int)howButtonPosition.X, (int)howButtonPosition.Y, 200, 50);
                Rectangle continueButtonRect = new Rectangle((int)continueGameButtonPosition.X, (int)continueGameButtonPosition.Y, 200, 50);
                if (mouseClickRect.Intersects(newButtonRect))
                {
                    State = GameState.New;
                    listpos = 4;
                }
                else if (mouseClickRect.Intersects(continueGameButtonRect))
                {
                    if (PlayerAlive)
                    {
                        State = GameState.Playing;
                    }
                }
                else if (mouseClickRect.Intersects(endButtonRect))
                {
                    State = GameState.Exit;
                }
                else if (mouseClickRect.Intersects(howButtonRect))
                {
                    State = GameState.How;
                }
            }
            if (State == GameState.New)
            {
                Rectangle continueButtonRect = new Rectangle((int)continueButtonPosition.X, (int)continueButtonPosition.Y, 200, 50);
                if (mouseClickRect.Intersects(continueButtonRect))
                {
                    listpos++;
                    if (listpos >= 7)
                    {
                        State = GameState.GameLoading;
                        listpos = 3;
                    }
                }
            }
        }
    }
}