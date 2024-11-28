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
        float delay = 0.25f;
        float lastDelay = 0f;
        Vector2 position = new Vector2(10, 10);
        Vector2 position2 = new Vector2(5, 5);
        const int cellSize = 32;
        int points = 0;
        int points2 = 0;
        SpriteFont spriteFont;
        bool canMove = true;
        bool canMove2 = true;
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
            Vector2 auxPosition = new Vector2(position.X * cellSize, position.Y * cellSize);
            bodyParts = new List<Part>();
            
            for (int i = 0; i < 4; i++)
            {
                if (bodyParts.Count == 0)
                {
                    var newPart = new Part
                    {
                        Position = auxPosition,
                        Direction = 'R'
                    };
                    bodyParts.Add(newPart);
                }
                else
                {
                    var lastPart = bodyParts[^1];
                    var newPart = new Part
                    {
                        Position = lastPart.Position - new Vector2(cellSize, 0),
                        Direction = lastPart.Direction
                    };
                    bodyParts.Add(newPart);
                }
            }
            snake = new Snake(bodyParts);

            Vector2 auxPosition2 = new Vector2(position2.X * cellSize, position2.Y * cellSize);
            bodyParts2 = new List<Part>();

            for (int i = 0; i < 4; i++)
            {
                if (bodyParts2.Count == 0)
                {
                    var newPart = new Part
                    {
                        Position = auxPosition2,
                        Direction = 'R'
                    };
                    bodyParts2.Add(newPart);
                }
                else
                {
                    var lastPart = bodyParts2[^1];
                    var newPart = new Part
                    {
                        Position = lastPart.Position - new Vector2(cellSize, 0),
                        Direction = lastPart.Direction
                    };
                    bodyParts2.Add(newPart);
                }
            }
            snake2 = new Snake(bodyParts2);

            if (gameAux == 0)
            {
                snake.GenerateApplePosition(random, _graphics, bodyParts2, snake2.ApplePosition);
                snake2.GenerateApplePosition(random, _graphics, bodyParts, snake.ApplePosition);
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

                if (canMove == true)
                {
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
                }
                if (canMove2 == true)
                {
                    if (kstate.IsKeyDown(Keys.Up) && snake2.SnakeDirection != 'D')
                    {
                        snake2.SnakeDirection = 'U';
                        canMove2 = false;
                    }

                    else if (kstate.IsKeyDown(Keys.Down) && snake2.SnakeDirection != 'U')
                    {
                        snake2.SnakeDirection = 'D';
                        canMove2 = false;
                    }

                    else if (kstate.IsKeyDown(Keys.Left) && snake2.SnakeDirection != 'R')
                    {
                        snake2.SnakeDirection = 'L';
                        canMove2 = false;
                    }

                    else if (kstate.IsKeyDown(Keys.Right) && snake2.SnakeDirection != 'L')
                    {
                        snake2.SnakeDirection = 'R';
                        canMove2 = false;
                    }
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
                            snake.GenerateApplePosition(random, _graphics, bodyParts2, snake2.ApplePosition);
                        }
                        else
                        {
                            snake2.GenerateApplePosition(random, _graphics, bodyParts, snake.ApplePosition);
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
                            snake2.GenerateApplePosition(random, _graphics, bodyParts, snake.ApplePosition);
                        }
                        else
                        {
                            snake.GenerateApplePosition(random, _graphics, bodyParts2, snake2.ApplePosition);
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
                    canMove2 = true;
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
            _spriteBatch.DrawString(spriteFont, $"Manzanas Player 1: {points}", new Vector2 (0,0), Color.Black);
            _spriteBatch.DrawString(spriteFont, $"Manzanas Player 2: {points2}", new Vector2(640, 0), Color.Black);
            if (GameOver == true || GameOver2 == true)
            {
                if (snake.SnakePosition == snake2.SnakePosition)
                {
                    _spriteBatch.DrawString(spriteFont, $"AMBOS PIERDEN", new Vector2(7.9f * cellSize, 6 * cellSize), Color.White, 0f, new Vector2(0, 0), 2.1f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"AMBOS PIERDEN", new Vector2(8 * cellSize, 6 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2.1f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Puntos conseguidos: {points}", new Vector2(7.3f * cellSize, 7 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Puntos conseguidos: {points2}", new Vector2(7.3f * cellSize, 8 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Presiona R para reiniciar", new Vector2(6.9f * cellSize, 10 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                }
                else if (GameOver == true)
                {
                    _spriteBatch.DrawString(spriteFont, $"PERDEDOR Snake Verde", new Vector2(6.9f * cellSize, 6 * cellSize), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"PERDEDOR Snake Verde", new Vector2(7 * cellSize, 6 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Puntos conseguidos: {points}", new Vector2(7 * cellSize, 7 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Presiona R para reiniciar", new Vector2(6.9f * cellSize, 10 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                }
                else
                {
                    _spriteBatch.DrawString(spriteFont, $"PERDEDOR Snake Gris", new Vector2(6.9f * cellSize, 6 * cellSize), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"PERDEDOR Snake Gris", new Vector2(7 * cellSize, 6 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Puntos conseguidos: {points2}", new Vector2(7 * cellSize, 7 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(spriteFont, $"Presiona R para reiniciar", new Vector2(6.9f * cellSize, 10 * cellSize), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void RestartGame()
        {
            bodyParts.Clear();
            position = new Vector2(10, 10);
            Vector2 auxPosition = new Vector2(position.X * cellSize, position.Y * cellSize);

            for (int i = 0; i < 4; i++)
            {
                if (bodyParts.Count == 0)
                {
                    var newPart = new Part
                    {
                        Position = auxPosition,
                        Direction = 'R'
                    };
                    bodyParts.Add(newPart);
                }
                else
                {
                    var lastPart = bodyParts[^1];
                    var newPart = new Part
                    {
                        Position = lastPart.Position - new Vector2(cellSize, 0),
                        Direction = lastPart.Direction
                    };
                    bodyParts.Add(newPart);
                }
            }
            snake = new Snake(bodyParts);
            snake.SnakeSheet = Content.Load<Texture2D>("snake_assets");

            snake.GenerateApplePosition(random, _graphics, bodyParts2, snake2.ApplePosition);

            GameOver = false;
            points = 0;

            bodyParts2.Clear();
            position2 = new Vector2(5, 5);
            Vector2 auxPosition2 = new Vector2(position2.X * cellSize, position2.Y * cellSize);

            for (int i = 0; i < 4; i++)
            {
                if (bodyParts2.Count == 0)
                {
                    var newPart = new Part
                    {
                        Position = auxPosition2,
                        Direction = 'R'
                    };
                    bodyParts2.Add(newPart);
                }
                else
                {
                    var lastPart = bodyParts2[^1];
                    var newPart = new Part
                    {
                        Position = lastPart.Position - new Vector2(cellSize, 0),
                        Direction = lastPart.Direction
                    };
                    bodyParts2.Add(newPart);
                }
            }
            snake2 = new Snake(bodyParts2);
            snake2.SnakeSheet = Content.Load<Texture2D>("snake_assets");

            snake2.GenerateApplePosition(random, _graphics, bodyParts, snake.ApplePosition);

            GameOver = false;

            points2 = 0;

            delay = 0.3f;
        }

    }
}
