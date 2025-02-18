using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WindowHeight = 16;
			Console.WindowWidth = 32;
			int screenWidth = Console.WindowWidth;
			int screenHeight = Console.WindowHeight;
			Random randomNumber = new Random();
			int score = 5;
			bool gameOver = false;
			pixel snakeHead = new pixel();
			snakeHead.xpos = screenWidth / 2;
			snakeHead.ypos = screenHeight / 2;
			snakeHead.color = ConsoleColor.Red;
			string snakeMovement = "RIGHT";
			List<int> snakeBodyXPos = new List<int>();
			List<int> snakeBodyYPos = new List<int>();
			int berryXPos = randomNumber.Next(0, screenWidth);
			int berryYPos = randomNumber.Next(0, screenHeight);
			DateTime actionTimerStartingTime = DateTime.Now;
			DateTime actonTimerCurrentTime = DateTime.Now;
			string buttonPressed = "no";
			while (true)
			{
				Console.Clear();
				// Snake dies when border is hit
				if (snakeHead.xpos == screenWidth - 1 || snakeHead.xpos == 0 || snakeHead.ypos == screenHeight - 1 || snakeHead.ypos == 0)
				{
					gameOver = true;
				}
				// Creation of the borders around the playable area
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
				// wtf
				Console.ForegroundColor = ConsoleColor.Green;

				// Check if snake head is on berry...
				if (berryXPos == snakeHead.xpos && berryYPos == snakeHead.ypos)
				{
					//... add points and create a new berry
					score++;
					berryXPos = randomNumber.Next(1, screenWidth - 2);
					berryYPos = randomNumber.Next(1, screenHeight - 2);
				}
				// Create snake tail (body) and check if head hit body
				for (int i = 0; i < snakeBodyXPos.Count(); i++)
				{
					Console.SetCursorPosition(snakeBodyXPos[i], snakeBodyYPos[i]);
					Console.Write("■");
					if (snakeBodyXPos[i] == snakeHead.xpos && snakeBodyYPos[i] == snakeHead.ypos)
					{
						gameOver = true;
					}
				}
				// Ends the main loop if game over
				if (gameOver)
				{
					break;
				}
				// Show snake head
				Console.SetCursorPosition(snakeHead.xpos, snakeHead.ypos);
				Console.ForegroundColor = snakeHead.color;
				Console.Write("■");
				// Show berry
				Console.SetCursorPosition(berryXPos, berryYPos);
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("■");

				// Wait set time if the user press a key
				actionTimerStartingTime = DateTime.Now;
				buttonPressed = "no";
				while (true)
				{
					actonTimerCurrentTime = DateTime.Now;
					if (actonTimerCurrentTime.Subtract(actionTimerStartingTime).TotalMilliseconds > 500) { break; }
					if (Console.KeyAvailable)
					{
						ConsoleKeyInfo toets = Console.ReadKey(true);
						//Console.WriteLine(toets.Key.ToString());
						if (toets.Key.Equals(ConsoleKey.UpArrow) && snakeMovement != "DOWN" && buttonPressed == "no")
						{
							snakeMovement = "UP";
							buttonPressed = "yes";
						}
						if (toets.Key.Equals(ConsoleKey.DownArrow) && snakeMovement != "UP" && buttonPressed == "no")
						{
							snakeMovement = "DOWN";
							buttonPressed = "yes";
						}
						if (toets.Key.Equals(ConsoleKey.LeftArrow) && snakeMovement != "RIGHT" && buttonPressed == "no")
						{
							snakeMovement = "LEFT";
							buttonPressed = "yes";
						}
						if (toets.Key.Equals(ConsoleKey.RightArrow) && snakeMovement != "LEFT" && buttonPressed == "no")
						{
							snakeMovement = "RIGHT";
							buttonPressed = "yes";
						}
					}
				}
				// Move the snake (if key was pressed move that way, if not use last direction)
				snakeBodyXPos.Add(snakeHead.xpos);
				snakeBodyYPos.Add(snakeHead.ypos);
				switch (snakeMovement)
				{
					case "UP":
						snakeHead.ypos--;
						break;
					case "DOWN":
						snakeHead.ypos++;
						break;
					case "LEFT":
						snakeHead.xpos--;
						break;
					case "RIGHT":
						snakeHead.xpos++;
						break;
				}
				// Delete last part of the snake (to keep the right size)
				if (snakeBodyXPos.Count() > score)
				{
					snakeBodyXPos.RemoveAt(0);
					snakeBodyYPos.RemoveAt(0);
				}
			}
			
			Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
			Console.WriteLine("Game over, Score: " + score);
			Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
		}
		class pixel
		{
			public int xpos { get; set; }
			public int ypos { get; set; }
			public ConsoleColor color { get; set; }
		}
	}
}
//¦