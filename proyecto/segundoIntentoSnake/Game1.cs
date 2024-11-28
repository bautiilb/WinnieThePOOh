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
        List<Part> bodyParts2 = new List<Part>();
        Snake snake;
        Snake snake2;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        bool GameOver = false;
        bool GameOver2 = false;
        float delay = 0.3f;
        float lastDelay = 0f;
        Vector2 position = new Vector2(10, 10);
        Vector2 position2 = new Vector2(5, 5);
        const int cellSize = 32;
        int points = 0;
        int points2 = 0;
        SpriteFont spriteFont;
        bool canMove = true;
        int auxSad = 0;
        int width;
        int high;
        Color snakeColor = Color.White;
        Color snake2Color = Color.MediumPurple;

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

            bodyParts2 = new List<Part>();
            for (int i = 0; i < 4; i++)
            {
                bodyParts2.Add(new Part());
            }
            snake2 = new Snake(bodyParts2);

            if (gameAux == 0)
            {
                snake.GenerateApplePosition(random, _graphics);
                snake2.GenerateApplePosition(random, _graphics);
                gameAux++;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            snake.SnakeSheet = Content.Load<Texture2D>("snake_assets");
            snake2.SnakeSheet = Content.Load<Texture2D>("snake_assets");
            spriteFont = Content.Load<SpriteFont>("Fonts/ArcadeFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GameOver != true  || GameOver2 != true && bodyParts.Count >= 4)
            {
                lastDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

                snake.PartDefinition();
                snake2.PartDefinition();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                                 Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();


                var kstate = Keyboard.GetState();


                if (kstate.IsKeyDown(Keys.W) && snake.SnakeDirection != 'D')
                {
                    snake.SnakeDirection = 'U';
                    canMove = false;
                }

                else if (kstate.IsKeyDown(Keys.S) && snake.SnakeDirection != 'U')
                {
                    snake.SnakeDirection = 'D';
                    canMove = false;
                }

                else if (kstate.IsKeyDown(Keys.A) && snake.SnakeDirection != 'R')
                {
                    snake.SnakeDirection = 'L';
                    canMove = false;
                }

                else if (kstate.IsKeyDown(Keys.D) && snake.SnakeDirection != 'L')
                {
                    snake.SnakeDirection = 'R';
                    canMove = false;
                }

                if (kstate.IsKeyDown(Keys.Up) && snake2.SnakeDirection != 'D')
                {
                    snake2.SnakeDirection = 'U';
                    canMove = false;
                }

                else if (kstate.IsKeyDown(Keys.Down) && snake2.SnakeDirection != 'U')
                {
                    snake2.SnakeDirection = 'D';
                    canMove = false;
                }

                else if (kstate.IsKeyDown(Keys.Left) && snake2.SnakeDirection != 'R')
                {
                    snake2.SnakeDirection = 'L';
                    canMove = false;
                }

                else if (kstate.IsKeyDown(Keys.Right) && snake2.SnakeDirection != 'L')
                {
                    snake2.SnakeDirection = 'R';
                    canMove = false;
                }

                if (lastDelay >= delay)
                {
                    if (bodyParts[0].Position.X >= width * cellSize || bodyParts[0].Position.X < 0 * cellSize || bodyParts[0].Position.Y >= high * cellSize || bodyParts[0].Position.Y < 0 * cellSize)
                    {
                        GameOver = true;
                        if (kstate.IsKeyDown(Keys.R) && (GameOver == true || GameOver2 == true))
                        {
                            //bodyParts.Clear();
                            RestartGame();
                        }
                        return;
                    }
                    if (bodyParts2[0].Position.X >= width * cellSize || bodyParts2[0].Position.X < 0 * cellSize || bodyParts2[0].Position.Y >= high * cellSize || bodyParts2[0].Position.Y < 0 * cellSize)
                    {
                        GameOver2 = true;
                        if (kstate.IsKeyDown(Keys.R) && (GameOver == true || GameOver2 == true))
                        {
                            //bodyParts.Clear();
                            RestartGame();
                        }
                        return;
                    }


                    if (snake.SnakeDirection == 'U') position.Y--;
                    else if (snake.SnakeDirection == 'D') position.Y++;
                    else if (snake.SnakeDirection == 'L') position.X--;
                    else if (snake.SnakeDirection == 'R') position.X++;

                    snake.SnakePosition = new Vector2(position.X * cellSize, position.Y * cellSize);

                    if (snake.SnakePosition == snake.ApplePosition || snake.SnakePosition == snake2.ApplePosition)
                    {
                        
                        if (snake.SnakePosition == snake.ApplePosition)
                        {
                            snake.GenerateApplePosition(random, _graphics);
                        }
                        else
                        {
                            snake2.GenerateApplePosition(random, _graphics);
                        }

                        var lastPart = bodyParts[^1];
                        var newPart = new Part
                        {
                            Position = lastPart.Position + snake.SnakePosition,
                            Direction = lastPart.Direction
                        };
                        bodyParts.Add(newPart);
                        points++;
                    }


                    snake.UpdateBody();

                    if (snake2.SnakeDirection == 'U') position2.Y--;
                    else if (snake2.SnakeDirection == 'D') position2.Y++;
                    else if (snake2.SnakeDirection == 'L') position2.X--;
                    else if (snake2.SnakeDirection == 'R') position2.X++;

                    snake2.SnakePosition = new Vector2(position2.X * cellSize, position2.Y * cellSize);

                    if (snake2.SnakePosition == snake2.ApplePosition || snake2.SnakePosition == snake.ApplePosition)
                    {
                      
                        if (snake2.SnakePosition == snake2.ApplePosition)
                        {
                            snake2.GenerateApplePosition(random, _graphics);
                        }
                        else
                        {
                            snake.GenerateApplePosition(random, _graphics);
                        }

                        var lastPart = bodyParts2[^1];
                        var newPart = new Part
                        {
                            Position = lastPart.Position + snake2.SnakePosition,
                            Direction = lastPart.Direction
                        };
                        bodyParts2.Add(newPart);
                        points2++;
                    }

                    snake2.UpdateBody();

                    lastDelay = 0f;
                    canMove = true;
                    auxSad++;
                }
                if (auxSad > 3)
                {
                    GameOver = snake.GameOver(bodyParts2);
                    GameOver2 = snake2.GameOver(bodyParts);
                }
            }
            else if (GameOver == true || GameOver2 == true)
            {
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.R) && (GameOver == true || GameOver2 == true))
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
            snake.DrawSnake(_spriteBatch, snakeColor);
            snake2.DrawSnake(_spriteBatch, snake2Color);
            snake.DrawApple(_spriteBatch);
            snake2.DrawApple(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, $"Manzanas Player 1: {points}", new Vector2 (0,0), Color.White);
            _spriteBatch.DrawString(spriteFont, $"Manzanas Player 2: {points2}", new Vector2(640, 0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void RestartGame()
        {
            bodyParts.Clear();
            position = new Vector2(10, 10);
            Vector2 lastPosition = position;

            for (int i = 0; i < 4; i++)
            {
                bodyParts.Add(new Part()
                {
                    Position = lastPosition,
                    Direction = 'R',
                });
                lastPosition = bodyParts[i].Position;
            }

            snake = new Snake(bodyParts);
            snake.SnakeSheet = Content.Load<Texture2D>("snake_assets");

            snake.GenerateApplePosition(random, _graphics);

            GameOver = false;
            points = 0;

            bodyParts2.Clear();
            position2 = new Vector2(5, 5);
            Vector2 lastPosition2 = position2;

            for (int i = 0; i < 4; i++)
            {
                bodyParts2.Add(new Part()
                {
                    Position = lastPosition2,
                    Direction = 'R',
                });
                lastPosition2 = bodyParts2[i].Position;
            }

            snake2 = new Snake(bodyParts2);
            snake2.SnakeSheet = Content.Load<Texture2D>("snake_assets");

            snake2.GenerateApplePosition(random, _graphics);

            GameOver = false;

            points2 = 0;

            delay = 0.3f;
        }

    }
}
