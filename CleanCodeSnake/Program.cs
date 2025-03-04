using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
	class Program
	{
		static void Main(string[] args)
		{
			// variable init
			Console.WindowHeight = 16;
			Console.WindowWidth = 32;
			int screenWidth = Console.WindowWidth;
			int screenHeight = Console.WindowHeight;
			Random randomNumber = new Random();
			int score = 5;
			bool gameOver = false;

            Pixel snakeHead = new Pixel(screenWidth / 2, screenHeight / 2, ConsoleColor.Red);
			string snakeMoveDirection = "RIGHT";
			List<int> snakeBodyXPos = new List<int>();
			List<int> snakeBodyYPos = new List<int>();
			int berryXPos = randomNumber.Next(0, screenWidth);
			int berryYPos = randomNumber.Next(0, screenHeight);
			DateTime actionTimerStartingTime = DateTime.Now;
			DateTime actionTimerCurrentTime = DateTime.Now;
			string buttonPressed = "no";

			void createBorders() {
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight - 1);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(screenWidth - 1, i);
                    Console.Write("■");
                }
            }
			void snakeDeathCheck()
			{
                if (snakeHead.XPos == screenWidth - 1 || snakeHead.XPos == 0 || snakeHead.YPos == screenHeight - 1 || snakeHead.YPos == 0)
                {
                    gameOver = true;
                }
            }
			void eatingCheck()
			{
                if (berryXPos == snakeHead.XPos && berryYPos == snakeHead.YPos)
                {
                    //... add points and create a new berry
                    score++;
                    berryXPos = randomNumber.Next(1, screenWidth - 2);
                    berryYPos = randomNumber.Next(1, screenHeight - 2);
                }
            }
			void snakeBodyPartDraw(int snake_body_part_index)
			{
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(snakeBodyXPos[snake_body_part_index], snakeBodyYPos[snake_body_part_index]);
                Console.Write("■");
            }
			void snakeBodyPartCollisionCheck(int snake_body_part_index)
			{
                if (snakeBodyXPos[snake_body_part_index] == snakeHead.XPos && snakeBodyYPos[snake_body_part_index] == snakeHead.YPos)
                {
                    gameOver = true;
                }
            }
            void snakeBodyLoop()
            {
                for (int i = 0; i < snakeBodyXPos.Count(); i++)
                {
                    snakeBodyPartDraw(i);
                    snakeBodyPartCollisionCheck(i);
                }
            }
			void berryDraw()
			{
                Console.SetCursorPosition(berryXPos, berryYPos);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
            }
			void snakeMoveKeyInput()
			{
                actionTimerStartingTime = DateTime.Now;
                buttonPressed = "no";
                while (true)
                {
                    actionTimerCurrentTime = DateTime.Now;
                    if (actionTimerCurrentTime.Subtract(actionTimerStartingTime).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && snakeMoveDirection != "DOWN" && buttonPressed == "no")
                        {
                            snakeMoveDirection = "UP";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && snakeMoveDirection != "UP" && buttonPressed == "no")
                        {
                            snakeMoveDirection = "DOWN";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && snakeMoveDirection != "RIGHT" && buttonPressed == "no")
                        {
                            snakeMoveDirection = "LEFT";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && snakeMoveDirection != "LEFT" && buttonPressed == "no")
                        {
                            snakeMoveDirection = "RIGHT";
                            buttonPressed = "yes";
                        }
                    }
                }
            }
            void snakeMoveExecution()
            {
                snakeBodyXPos.Add(snakeHead.XPos);
                snakeBodyYPos.Add(snakeHead.YPos);
                switch (snakeMoveDirection)
                {
                    case "UP":
                        snakeHead.YPos--;
                        break;
                    case "DOWN":
                        snakeHead.YPos++;
                        break;
                    case "LEFT":
                        snakeHead.XPos--;
                        break;
                    case "RIGHT":
                        snakeHead.XPos++;
                        break;
                }

                if (snakeBodyXPos.Count() > score)
                {
                    snakeBodyXPos.RemoveAt(0);
                    snakeBodyYPos.RemoveAt(0);
                }
            }
            void gameOverScreen()
            {
                Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
                Console.WriteLine("Game over, Score: " + score);
                Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
            }

			while (true)
			{
				Console.Clear();
				snakeDeathCheck();
				createBorders();
				eatingCheck();
				snakeBodyLoop();
				if (gameOver) { break; }
                snakeHead.draw();
				berryDraw();
                snakeMoveKeyInput();
                snakeMoveExecution();
			}
            gameOverScreen();
		}
		class Pixel
		{
			public int XPos { get; set; }
			public int YPos { get; set; }
			public ConsoleColor Color { get; set; }


            public Pixel(int xPosition, int yPosition, ConsoleColor pixelColor)
            {
                XPos = xPosition;
                YPos = yPosition;
                Color = pixelColor;
            }

            public void draw()
            {
                Console.SetCursorPosition(XPos, YPos);
                Console.ForegroundColor = Color;
                Console.Write("■");
            }
		}

        class Snake
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public Snake(int screenWidth, int screenHeight)
            {
                XPos = screenWidth / 2;
                YPos = screenHeight / 2;
                Pixel snakeHead = new Pixel(XPos, YPos, ConsoleColor.Red);

            }
        }

        class Berry
        {
            public Berry() {

            }
        }
	}
}
//¦