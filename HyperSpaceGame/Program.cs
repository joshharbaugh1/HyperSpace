using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperSpaceGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Hyperspace game = new Hyperspace();
            game.PlayGame();
        }
    }

    class Unit
    {
        private int _x;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        ConsoleColor _color;
        ConsoleColor Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private bool _isSpaceRift;
        public bool IsSpaceRift
        {
            get { return _isSpaceRift; }
            set { _isSpaceRift = value; }
        }

        static List<string> ObstacleList = new List<string> { "*", "!", ".", ":", ";", ":", "'" };
        static Random rng = new Random();

        public Unit(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Color = ConsoleColor.Cyan;
            this.Symbol = ObstacleList[rng.Next(0, ObstacleList.Count())];
            this.IsSpaceRift = false;
        }

        public Unit(int x, int y, ConsoleColor color, string symbol, bool isSpaceRift)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Symbol = symbol;
            this.IsSpaceRift = true;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X,Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
        } //end of constructors
    } //end of Unit class

    class Hyperspace
    {
        private int _score;
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private List<Unit> _unitObstacleList;
        public List<Unit> UnitObstacleList
        {
            get { return _unitObstacleList; }
            set { _unitObstacleList = value; }
        }

        private Unit _spaceShip;
        public Unit SpaceShip
        {
            get { return _spaceShip; }
            set { _spaceShip = value; }
        }

        private bool _smashed;
        public bool Smashed
        {
            get { return _smashed; }
            set { _smashed = value; }
        }

        private Random randomNumberGenerator = new Random();

        public Hyperspace()
        {
            Console.WindowWidth = 60;
            Console.WindowHeight = 30;
            Console.BufferHeight = 30;
            Console.BufferWidth = 60;
            this.Score = 0;
            this.Speed = 0;
            this.UnitObstacleList = new List<Unit>();
            this.SpaceShip = new Unit((Console.WindowWidth / 2) - 1, Console.WindowHeight - 1, ConsoleColor.Red, "@", false);
            this.Smashed = false;
        }

        public void PlayGame()
        {
            while (Smashed == false)
            {
                int spawnRift = randomNumberGenerator.Next(101);
                if (spawnRift < 10)
                {
                    Unit spaceRift = new Unit(randomNumberGenerator.Next(Console.WindowWidth - 2), 5, ConsoleColor.Yellow, "%", true);
                    UnitObstacleList.Add(spaceRift);
                }
                else
                {
                    Unit Obstacle = new Unit(randomNumberGenerator.Next(Console.WindowWidth - 2), 5);
                    UnitObstacleList.Add(Obstacle);
                }
                MoveShip();
                MoveObstacles();
                DrawGame();

                if (Speed < 170)
                {
                    Speed++;
                }
                System.Threading.Thread.Sleep(170 - Speed);
            }
        }

        public void MoveShip()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                if (keyPressed.Key == ConsoleKey.LeftArrow && SpaceShip.X > 0)
                {
                    SpaceShip.X--;
                }
                if (keyPressed.Key == ConsoleKey.RightArrow && SpaceShip.X < Console.WindowWidth - 2)
                {
                    SpaceShip.X++;
                }
            }
        }

        public void MoveObstacles()
        {
            List<Unit> newObstacleList = new List<Unit>();
            foreach (Unit item in UnitObstacleList)
            {
                item.Y++;

                if ((item.IsSpaceRift) && item.X == SpaceShip.X && item.Y == SpaceShip.Y)
                {
                    Speed -= 50;
                }
                else if (! item.IsSpaceRift && item.X == SpaceShip.X && item.Y == SpaceShip.Y)
                {
                    Smashed = true;
                }
                else if (item.Y < Console.WindowHeight)
                {
                    newObstacleList.Add(item);
                }
                else
                {
                    Score++;
                }
            }
            UnitObstacleList = newObstacleList;
        }
        public void DrawGame()
        {
            Console.Clear();
            SpaceShip.Draw();
            foreach (Unit item in UnitObstacleList)
            {
                item.Draw();
            }
            PrintAtPosition(20, 2, "Score:" + this.Score, ConsoleColor.Green);
            PrintAtPosition(20, 3, "Speed:" + this.Speed, ConsoleColor.Green);
            
        }

        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(text);
        }
    }
}
