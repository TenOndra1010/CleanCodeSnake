using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
	class Program
	{
		static void Main(string[] args)
		{
            Screen mainScreen = new Screen(32,16);
			Random randomNumber = new Random();
			int score = 5;
			bool gameOver = false;

            Snake snake1 = new Snake(mainScreen);
			List<int> snakeBodyXPos = new List<int>();
			List<int> snakeBodyYPos = new List<int>();
			int berryXPos = randomNumber.Next(0, mainScreen.ScreenWidth);
			int berryYPos = randomNumber.Next(0, mainScreen.ScreenHeight);
			DateTime actionTimerStartingTime = DateTime.Now;
			DateTime actionTimerCurrentTime = DateTime.Now;
			string buttonPressed = "no";

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
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && snake1.snakeMoveDirection != "DOWN" && buttonPressed == "no")
                        {
                            snake1.snakeMoveDirection = "UP";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && snake1.snakeMoveDirection != "UP" && buttonPressed == "no")
                        {
                            snake1.snakeMoveDirection = "DOWN";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && snake1.snakeMoveDirection != "RIGHT" && buttonPressed == "no")
                        {
                            snake1.snakeMoveDirection = "LEFT";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && snake1.snakeMoveDirection != "LEFT" && buttonPressed == "no")
                        {
                            snake1.snakeMoveDirection = "RIGHT";
                            buttonPressed = "yes";
                        }
                    }
                }
            }
            void snakeMoveExecution()
            {
                snakeBodyXPos.Add(snake1.XPos);
                snakeBodyYPos.Add(snake1.YPos);
                switch (snake1.snakeMoveDirection)
                {
                    case "UP":
                        snake1.YPos--;
                        break;
                    case "DOWN":
                        snake1.YPos++;
                        break;
                    case "LEFT":
                        snake1.XPos--;
                        break;
                    case "RIGHT":
                        snake1.XPos++;
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
				mainScreen.createBorders();
				eatingCheck();
				snakeBodyLoop();
				if (gameOver) { break; }
                
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

        class Screen
        {
            public int ScreenWidth {  get; set; }
            public int ScreenHeight { get; set; }
            public Screen(int screenWidth, int screenHeight) {ScreenWidth = screenWidth; ScreenHeight = screenHeight; }
            public void screenSetup()
            {
                Console.WindowWidth = ScreenWidth;
                Console.WindowHeight = ScreenHeight;
                createBorders();
            }

            public void createBorders()
            {
                for (int i = 0; i < ScreenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < ScreenWidth; i++)
                {
                    Console.SetCursorPosition(i, ScreenHeight - 1);
                    Console.Write("■");
                }
                for (int i = 0; i < ScreenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < ScreenHeight; i++)
                {
                    Console.SetCursorPosition(ScreenWidth - 1, i);
                    Console.Write("■");
                }
            }
        }

        class Snake
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public string snakeMoveDirection { get; set; }
            public Snake(Screen screen)
            {
                snakeMoveDirection = "RIGHT";
                XPos = screen.ScreenWidth / 2;
                YPos = screen.ScreenHeight / 2;
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