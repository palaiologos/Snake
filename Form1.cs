using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        // Create player and food circles.
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            // Set settings to default.
            new Settings();

            // Set game sped and start timer.
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            // Start the new game.
            StartGame();
        }

        private void StartGame()
        {
            // Set settings to default upon new game.
            new Settings();

            // Set the game over label visibility to false.
            labelGameOver.Visible = false;

            // Clear the player snake.
            Snake.Clear();

            // New player character.
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            // Set score as string.
            labelScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        // Place random food object in the game screen.
        private void GenerateFood()
        {
            // Set maximum X and Y positions.
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            // New random object.
            Random random = new Random();
            food = new Circle();

            // Make random somewhere between 0 and max grid positions.
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        // Update the screen by checking for game over and listening for inputs.
        private void UpdateScreen(object sender, EventArgs e)
        {
            // Check for game over.
            if (Settings.GameOver == true)
            {
                // Check if enter is pressed.
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }

            else
            {
                // Make sure you can't turn 180 degrees and collide with your own body.
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                {
                    Settings.direction = Direction.Right;
                }
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                {
                    Settings.direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }

                MovePlayer();
            }
            // Make sure canvas refreshed each time UpdateScreen() is called.
            pbCanvas.Invalidate();
        }

        private void PbCanvas_Paint(object sender, PaintEventArgs e)
        {
            // Tells program which canvas to use.
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                // Set color of snake.
                Brush snakeColor;

                // Draw the snake.
                for (int i = 0; i < Snake.Count; i++)
                {
                    // Color the head of the snake.
                    if (i == 0)
                    {
                        snakeColor = Brushes.Black;
                    }
                    // Color the body of the snake.
                    else
                    {
                        snakeColor = Brushes.Green;
                    }

                    // Draw snake and food every time canvas is refreshed.
                    // Draw snake.
                    canvas.FillEllipse(snakeColor, new Rectangle(Snake[i].X * Settings.Width,
                                                                 Snake[i].Y * Settings.Height,
                                                                 Settings.Width, Settings.Height));

                    // Draw food.
                    canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width, 
                                                                  food.Y * Settings.Height, 
                                                                  Settings.Width, Settings.Height));
                }
            }
            // Otherwise, game is over.
            else
            {
                string gameOver = "Game Over \nYour final score is: " + Settings.Score + "\nPress Enter to try again"; ;

                // Set game over message and reveal it.
                labelGameOver.Text = gameOver;
                labelGameOver.Visible = true;
            }
        }

        // Moves the player.
        private void MovePlayer()
        {
            // Loop to ensure each circle gets moved on the screen.
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                // Move the head.
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    // Get maximum X and Y positions.
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Width / Settings.Height;

                    // Detect collisions with game borders.
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    // Detect collisions with body.
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    // Detect collisions with food pieces.
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }



                }
                // Otherwise move the body parts.
                else
                {
                    // Move the body to follow the head.
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

        }

        // Snake dies and game is over.
        private void Die()
        {
            Settings.GameOver = true;
        }

        // The snake eats.
        private void Eat()
        {
            // Add circle to body.
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            // Add the food to the snake's body.
            Snake.Add(food);

            // Update the total score.
            Settings.Score += Settings.Points;
            labelScore.Text = Settings.Score.ToString();

            // Make more food since the last one was eaten.
            GenerateFood();
        }

        // Lets us know a key was was pressed up or down.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
