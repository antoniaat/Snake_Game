namespace SnakeGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    struct Position
    {
        public int Row;
        public int Col;

        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    }

    public class Game
    {
        public static void Main()
        {
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;


            Position[] directions = new Position[]
            {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), //top 
            };

            var direction = 0;
            Random randomNumberGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            Position food = new Position(randomNumberGenerator.Next(0, Console.WindowHeight), randomNumberGenerator.Next(0, Console.WindowWidth));
            Console.SetCursorPosition(food.Col, food.Row);
            Console.Write('@');
            double sleepTime = 100;
            Queue<Position> snakeElements = new Queue<Position>();

            for (int i = 0; i < 6; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();

                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left) direction = right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down) direction = up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up) direction = down;
                    }
                }

                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];
                Position snakeNewHead = new Position(snakeHead.Row + nextDirection.Row, snakeHead.Col + nextDirection.Col);

                if (snakeNewHead.Row < 0 || snakeNewHead.Col < 0 || snakeNewHead.Row >= Console.WindowHeight ||
                    snakeNewHead.Col >= Console.WindowWidth || snakeElements.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();
                    Console.WriteLine("Game over!");
                    Console.WriteLine($"Your points are: {(snakeElements.Count - 6) * 100}");

                    return;
                }

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.Col, snakeNewHead.Row);
                Console.Write('*');

                if (snakeNewHead.Col == food.Col && snakeNewHead.Row == food.Row)
                {
                    // feeding the snake
                    food = new Position(randomNumberGenerator.Next(0, Console.WindowHeight),
                        randomNumberGenerator.Next(0, Console.WindowWidth));
                }
                else
                {
                    // mooving ...
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.Col, last.Row);
                    Console.Write(' ');
                }

                Console.Clear();

                foreach (Position position in snakeElements)
                {
                    Console.SetCursorPosition(position.Col, position.Row);
                    Console.Write('*');
                }

                Console.SetCursorPosition(food.Col, food.Row);
                Console.Write('@');

                sleepTime -= 0.1;
                Thread.Sleep((int)sleepTime);
            }
        }
    }
}
