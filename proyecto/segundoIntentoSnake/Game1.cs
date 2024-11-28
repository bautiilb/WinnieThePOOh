using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace segundoIntentoSnake
{
    public class Game1 : Game
    {
        int gameAux = 0;
        Random random = new Random();
        List<Part> bodyParts = new List<Part>();
        Snake snake;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        bool GameOver = false;
        float delay = 0.3f;
        float lastDelay = 0f;
        Vector2 position = new Vector2(10, 10);
        const int cellSize = 32;
        int points = 0;
        SpriteFont spriteFont;
        bool canMove = true;
        int auxSad = 0;
        int width;
        int high;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            width = _graphics.PreferredBackBufferWidth / cellSize;
            high = _graphics.PreferredBackBufferHeight / cellSize;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            bodyParts = new List<Part>();
            for (int i = 0; i < 4; i++)
            {
                bodyParts.Add(new Part());
            }
            snake = new Snake(bodyParts);


            if (gameAux == 0)
            {
                snake.GenerateApplePosition(random, _graphics);
                gameAux++;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            snake.SnakeSheet = Content.Load<Texture2D>("snake_assets");
            spriteFont = Content.Load<SpriteFont>("Fonts/ArcadeFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GameOver != true && bodyParts.Count >= 4)
            {
                lastDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

                snake.PartDefinition();


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                                 Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();


                var kstate = Keyboard.GetState();


                if ((kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W)) && snake.SnakeDirection != 'D')
                {
                    snake.SnakeDirection = 'U';
                    canMove = false;
                }

                else if ((kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) && snake.SnakeDirection != 'U')
                {
                    snake.SnakeDirection = 'D';
                    canMove = false;
                }

                else if ((kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)) && snake.SnakeDirection != 'R')
                {
                    snake.SnakeDirection = 'L';
                    canMove = false;
                }

                else if ((kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) && snake.SnakeDirection != 'L')
                {
                    snake.SnakeDirection = 'R';
                    canMove = false;
                }

                if (lastDelay >= delay)
                {
                    if (bodyParts[0].Position.X >= width * cellSize || bodyParts[0].Position.X < 0 * cellSize || bodyParts[0].Position.Y >= high * cellSize || bodyParts[0].Position.Y < 0 * cellSize)
                    {
                        GameOver = true;  
                        return;
                    }

                    if (snake.SnakeDirection == 'U')
                        position.Y--;
                    else if (snake.SnakeDirection == 'D')
                        position.Y++;
                    else if (snake.SnakeDirection == 'L')
                        position.X--;
                    else if (snake.SnakeDirection == 'R')
                        position.X++;

                    snake.SnakePosition = new Vector2(
                        position.X * cellSize,
                        position.Y * cellSize
                    );

                    if (snake.SnakePosition == snake.ApplePosition)
                    {
                        snake.GenerateApplePosition(random, _graphics);
                        var lastPart = bodyParts[^1];
                        var newPart = new Part
                        {
                            Position = lastPart.Position + snake.SnakePosition,
                            Direction = lastPart.Direction
                        };
                        bodyParts.Add(newPart);
                        delay = Math.Max(0.1f, delay - 0.008f);
                        points++;
                    }

                    snake.UpdateBody();

                    lastDelay = 0f;

                    canMove = true;

                    auxSad++;
                }
                if(auxSad > 4)
                    GameOver = snake.GameOver();
            }
            else if (GameOver == true)
            {
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.R))
                {
                    //bodyParts.Clear();
                    RestartGame();
                }
                return;
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            snake.DrawSnake(_spriteBatch);
            snake.DrawApple(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, $"Manzanas: {points}", new Vector2 (0,0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void RestartGame()
        {
            bodyParts.Clear();
            position = new Vector2(10, 10); //--?
            Vector2 lastPosition = new Vector2(-10, -10);
            for (int i = 0; i < 4; i++)
            {
                bodyParts.Add(new Part()
                {
                    Position = position + lastPosition,
                    Direction = 'T',
                    Type = Part.SnakePartType.BodyHorizontal
                });
                lastPosition = bodyParts[i].Position;
            }
            snake = new Snake(bodyParts);

            snake.SnakeSheet = Content.Load<Texture2D>("snake_assets");

            snake.GenerateApplePosition(random, _graphics);
            GameOver = false;
            points = 0;
            delay = 0.3f;
        }

    }
}
