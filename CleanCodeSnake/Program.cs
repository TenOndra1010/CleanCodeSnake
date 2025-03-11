using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(32, 16);
            game.Run();
        }
    }

    class Game
    {
        public Screen TheScreen { get; private set; }
        public Snake TheSnake { get; private set; }
        public int Score { get; set; }
        public bool GameOver { get; set; }
        private Random randomNumber;
        public Berry berry { get; private set; }

        public Game(int width, int height)
        {
            TheScreen = new Screen(this, width, height, ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Magenta);
            TheSnake = new Snake(this);
            Score = 5;
            GameOver = false;
            randomNumber = new Random();
            berry = new Berry(this);
        }

        public void GameLogic()
        {
            SnakeMoveKeyInput();
            berry.EatingCheck();
            TheSnake.SnakeMoveExecution();
            TheSnake.SnakeDeathCheck();
        }

        public void Run()
        {
            TheScreen.ScreenSetup();
            while (!GameOver)
            {
                TheScreen.DrawEverything();
                GameLogic();
            }
            TheScreen.GameOverScreen();
        }

        private void SnakeMoveKeyInput()
        {
            DateTime actionTimerStartingTime = DateTime.Now;
            string buttonPressed = "no";
            while (true)
            {
                if ((DateTime.Now - actionTimerStartingTime).TotalMilliseconds > 500) break;
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    TheSnake.ChangeDirection(key, ref buttonPressed);
                }
            }
        }
    }

    class Screen
    {
        private Game game;
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        private ConsoleColor BorderColor;
        private ConsoleColor SnakeColor;
        private ConsoleColor SnakeHeadColor;
        private ConsoleColor BerryColor;

        public Screen(Game game, int width, int height, ConsoleColor borderColor, ConsoleColor snakeColor, ConsoleColor snakeHeadColor, ConsoleColor berryColor)
        {
            this.game = game;
            ScreenWidth = width;
            ScreenHeight = height;
            BorderColor = borderColor;
            SnakeColor = snakeColor;
            SnakeHeadColor = snakeHeadColor;
            BerryColor = berryColor;
        }

        public void ScreenSetup()
        {
            Console.WindowWidth = ScreenWidth;
            Console.WindowHeight = ScreenHeight;
        }

        public void DrawEverything()
        {
            Console.Clear();
            DrawBorder();
            DrawSnake();
            DrawBerry();
        }

        private void DrawBorder()
        {
            Console.ForegroundColor = BorderColor;
            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, ScreenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(ScreenWidth - 1, i);
                Console.Write("■");
            }
        }

        public void DrawSnake()
        {
            Console.ForegroundColor = SnakeColor;
            foreach (var part in game.TheSnake.BodyParts)
            {
                Console.SetCursorPosition(part.XPos, part.YPos);
                Console.Write("■");
            }
            Console.ForegroundColor = SnakeHeadColor;
            Console.SetCursorPosition(game.TheSnake.XPos, game.TheSnake.YPos);
            Console.Write("■");
        }

        public void DrawBerry()
        {
            Console.SetCursorPosition(game.berry.XPos, game.berry.YPos);
            Console.ForegroundColor = BerryColor;
            Console.Write("■");
        }

        public void GameOverScreen()
        {
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + game.Score);
        }
    }

    class Snake
    {
        private Game game;
        public int XPos { get; private set; }
        public int YPos { get; private set; }
        public string MoveDirection { get; private set; }
        public List<SnakeBodyPart> BodyParts { get; private set; }

        public Snake(Game game)
        {
            this.game = game;
            MoveDirection = "RIGHT";
            XPos = game.TheScreen.ScreenWidth / 2;
            YPos = game.TheScreen.ScreenHeight / 2;
            BodyParts = new List<SnakeBodyPart>();
        }

        public void ChangeDirection(ConsoleKeyInfo key, ref string buttonPressed)
        {
            if (buttonPressed == "no")
            {
                if (key.Key == ConsoleKey.UpArrow && MoveDirection != "DOWN") MoveDirection = "UP";
                if (key.Key == ConsoleKey.DownArrow && MoveDirection != "UP") MoveDirection = "DOWN";
                if (key.Key == ConsoleKey.LeftArrow && MoveDirection != "RIGHT") MoveDirection = "LEFT";
                if (key.Key == ConsoleKey.RightArrow && MoveDirection != "LEFT") MoveDirection = "RIGHT";
                buttonPressed = "yes";
            }
        }

        public void SnakeMoveExecution()
        {
            BodyParts.Add(new SnakeBodyPart(XPos, YPos));
            switch (MoveDirection)
            {
                case "UP": YPos--; break;
                case "DOWN": YPos++; break;
                case "LEFT": XPos--; break;
                case "RIGHT": XPos++; break;
            }
            if (BodyParts.Count > game.Score)
            {
                BodyParts.RemoveAt(0);
            }
        }

        public void SnakeDeathCheck()
        {
            if (XPos == game.TheScreen.ScreenWidth - 1 || XPos == 0 ||
                YPos == game.TheScreen.ScreenHeight - 1 || YPos == 0)
            {
                game.GameOver = true;
            }

            foreach (var part in BodyParts)
            {
                if (XPos == part.XPos && YPos == part.YPos)
                {
                    game.GameOver = true;
                    break;
                }
            }
        }
    }
    class SnakeBodyPart
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public SnakeBodyPart(int x, int y)
        {
            XPos = x;
            YPos = y;
        }
    }
    class Berry
    {
        private Game game;
        public int XPos { get; set; }
        public int YPos { get; set; }

        public Berry(Game game)
        {
            this.game = game;
            SpawnBerry();
        }

        public void SpawnBerry()
        {
            Random random = new Random();
            XPos = random.Next(1, game.TheScreen.ScreenWidth - 2);
            YPos = random.Next(1, game.TheScreen.ScreenHeight - 2);
        }

        public void EatingCheck()
        {
            if (XPos == game.TheSnake.XPos && YPos == game.TheSnake.YPos)
            {
                game.Score++;
                SpawnBerry();
            }
        }
    }
}
