using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace Movement
{
    class Program
    {
        static char FieldChar = ' ';
        static char ShipChar = 'X';
        static char YourShotChar = '*';
        static char EnemyShotChar = '&';
        static char EnemyChar = '%';
        static char BorderChar = '#';
        static int EnemyShoting = 0;
        static int EnemyShotDelayInFrames = 5;

        static bool Playing = true;  // If false => Game Over.
        static ConsoleKeyInfo keyInfo; // Saves a keypress from the console.
        static int Delay = 10; // Delay for frame.
        static int rows = 25; //Rowl of the field.
        static int cols = 100; //Colomns of the field.
        static char[,] field = new char[rows, cols]; //Matrix for the field.
        static int ship = cols / 2; //Remembers on witch colomn is the top of the ship.
        static int lives = 10; // Lives of your ship.
        static int LineEnemyShips = 1; // Lines of enemy ships.
        static int EnemyLives = 4; // Lives of enemy ships.
        static int ShipReset = 0;

        static int points = 0; //Points at the end of the game.
        // Structure that holds a cordinates of the shots.
        public struct shot
        {
            public int row;
            public int col;
        }
        static shot record; // Records a shot.
        static List<shot> YourShots = new List<shot>();// A list to hold the cordinates of your shots.
        static List<shot> EnemyShots = new List<shot>(); // A list to hold the cordinates of the enemy shots.
        static List<shot> AcurateShots = new List<shot>();
        static Random EnemyShotPosition = new Random();

        // Structure for a enemy ship.
        public struct EnemyShip
        {
            public int row;
            public int col;
            public int lives;
        }

        static EnemyShip[] EnemyShips = new EnemyShip[(cols - 2) / 4 * LineEnemyShips]; // Array of structures that hold the cordinates of enemy ships.

        //***********************************************************************************************MAIN MADAFAKA**************************************************************************************
        static void Main()
        {
            Console.SetWindowSize(cols + 2, rows + 3);
            ResetField();
            Ship();
            Print();
            EnemyParameters();
            Play();
            Ending();
        }

        // Enters the enemy ships to the field.
        private static void EnemiesToField()
        {
            for (int i = 0; i < EnemyShips.Length; i++)
            {
                if (EnemyShips[i].lives > 0)
                {
                    record.row = EnemyShips[i].row;
                    record.col = EnemyShips[i].col;
                    if (AcurateShots.Contains(record))
                        EnemyShips[i].lives--;
                    if (EnemyShips[i].lives > 0)
                        field[EnemyShips[i].row, EnemyShips[i].col] = EnemyChar;
                    for (int j = EnemyShips[i].col - 1; j < EnemyShips[i].col + 2; j++)
                    {
                        record.row = EnemyShips[i].row - 1;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShips[i].lives--;
                        if (EnemyShips[i].lives > 0)
                            field[EnemyShips[i].row - 1, j] = EnemyChar;
                    }
                }
            }
            AcurateShots.Clear();
        }

        // Enters the parameters of the enemy ships.
        private static void EnemyParameters()
        {
            int EnemyCol = 2;
            int EnemyRow = 3;
            for (int i = 0; i < EnemyShips.Length; i++)
            {
                if (EnemyCol < cols - 3)
                {
                    EnemyShips[i].row = EnemyRow;
                    EnemyShips[i].col = EnemyCol;
                    EnemyShips[i].lives = EnemyLives;
                    EnemyCol += 4;
                }
                else
                {
                    EnemyRow = EnemyRow + 3;
                    EnemyCol = 2;
                    EnemyShips[i].row = EnemyRow;
                    EnemyShips[i].col = EnemyCol;
                    EnemyShips[i].lives = EnemyLives;
                    EnemyCol += 4;
                }

            }
        }

        // End of the game.
        private static void Ending()
        {
            Console.Clear();
            Console.WriteLine("XxX Game Over XxX ");
            Console.WriteLine("Your points are {0}", points);
            if (points < 0)
                Console.WriteLine("Very Noob");
            else if (points >= 0 && points < 200)
                Console.WriteLine("You Suck");
            else if (points >= 200 && points < 500)
                Console.WriteLine("Good");
            else if (points >= 500)
                Console.WriteLine("WOW");
            Console.WriteLine(Console.LargestWindowHeight);
        }

        // Main loop.
        private static void Play()
        {
            while (Playing)
            {
                DateTime check2 = DateTime.Now;
                DateTime check = DateTime.Now.AddMilliseconds(Delay);
                while (check > DateTime.Now)
                {
                    if (Console.KeyAvailable)
                    {
                        keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.W:
                                ShotRecord();
                                break;
                            case ConsoleKey.A:
                                if (ship != 3)
                                    ship--;
                                break;
                            case ConsoleKey.D:
                                if (ship != cols - 4)
                                    ship++;
                                break;
                            case ConsoleKey.X:
                                Playing = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = 0; i < EnemyShips.Length; i++)
                {
                    if (EnemyShips[i].lives == 0)
                        ShipReset++;
                }
                if (ShipReset == EnemyShips.Length)
                    EnemyParameters();
                ResetField();
                Ship();
                Borders();
                EnemiesToField();
                EnemyShotsToField();
                ShotsToField();
                if (EnemyShoting == EnemyShotDelayInFrames)
                    EnemyShotRecord();
                else EnemyShoting++;
                Print();
                if (lives <= 0)
                    Playing = false;
                ShipReset = 0;
                Console.WriteLine(DateTime.Now - check2);
                Thread.Sleep(500);
            }
        }

        // Records an enemy shot.
        private static void EnemyShotRecord()
        {
            List<int> DeadRows = new List<int>();
            NoShipsAlive:
            if (DeadRows.Count == (cols - 2) / 4 - 1)
                goto NoShipsAliveReally;
            int EnemyShotPositionRecord = EnemyShotPosition.Next(0, (cols - 2) / 4);
            if (DeadRows.Contains(EnemyShotPositionRecord))
                goto NoShipsAlive;
            int EnemyShipShoting = EnemyShotPositionRecord + (LineEnemyShips - 1) * ((cols - 2) / 4);
            while (true)
            {
                if (EnemyShips[EnemyShipShoting].lives > 0)
                {
                    record.row = EnemyShips[EnemyShipShoting].row + 1;
                    record.col = EnemyShips[EnemyShipShoting].col;
                    EnemyShots.Add(record);
                    break;
                }
                else
                {
                    EnemyShipShoting -= (cols - 2) / 4;
                    if (EnemyShotPositionRecord <= 0 || EnemyShipShoting < 0)
                    {
                        DeadRows.Add(EnemyShotPositionRecord + (LineEnemyShips - 1));
                        goto NoShipsAlive;
                    }
                }

            }
            NoShipsAliveReally:
            EnemyShoting = 0;
        }

        // Prints the borders.
        private static void Borders()
        {
            for (int i = 0; i < rows; i++)
            {
                field[i, 0] = BorderChar;
                field[i, cols - 1] = BorderChar;
            }
            for (int i = 0; i < cols; i++)
            {
                field[0, i] = BorderChar;
                field[rows - 1, i] = BorderChar;
            }
        }

        // Enters the enemyshots to the matrix.
        private static void EnemyShotsToField()
        {
            for (int j = 0; j < EnemyShots.Count; j++)
            {
                if (field[EnemyShots[j].row, EnemyShots[j].col] == BorderChar)
                {
                    EnemyShots.RemoveAt(j);
                }
                else if (field[EnemyShots[j].row, EnemyShots[j].col] == ShipChar)
                {
                    EnemyShots.RemoveAt(j);
                    lives--;
                }
                else
                {
                    field[EnemyShots[j].row, EnemyShots[j].col] = EnemyShotChar;
                    record.col = EnemyShots[j].col;
                    record.row = EnemyShots[j].row + 1;
                    EnemyShots.RemoveAt(j);
                    EnemyShots.Insert(j, record);
                }
            }
        }

        // Enters the shots to the matrix.
        private static void ShotsToField()
        {
            for (int j = 0; j < YourShots.Count; j++)
            {
                if (field[YourShots[j].row, YourShots[j].col] == BorderChar)
                {
                    YourShots.RemoveAt(j);
                }
                else if (field[YourShots[j].row, YourShots[j].col] == EnemyChar)
                {
                    AcurateShots.Add(YourShots[j]);
                    YourShots.RemoveAt(j);
                    points++;
                }
                else
                {
                    field[YourShots[j].row, YourShots[j].col] = YourShotChar;
                    record.col = YourShots[j].col;
                    record.row = YourShots[j].row - 1;
                    YourShots.RemoveAt(j);
                    YourShots.Insert(j, record);
                }
            }
        }

        // Resets the field to only background symbols.
        private static void ResetField()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    field[i, j] = FieldChar;
                }
            }
        }

        // Enters the ship into the matrix.
        private static void Ship()
        {
            field[rows - 4, ship] = ShipChar;
            for (int i = ship - 1; i < ship + 2; i++)
            {
                field[rows - 3, i] = ShipChar;
            }
            for (int i = ship - 2; i < ship + 3; i++)
            {
                field[rows - 2, i] = ShipChar;
            }
        }

        // Prints the whole field.
        private static void Print()
        {
            string print = "";
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (field[i, j] == BorderChar)
                    {
                        print += field[i, j];
                    }
                    else print += field[i, j];
                }
                print += '\n';
            }
            Console.Clear();
            Console.Write(print);
            Console.WriteLine("Points: {0}", points);
        }

        // Records the shots.
        private static void ShotRecord()
        {
            record.row = rows - 5;
            record.col = ship;
            YourShots.Add(record);
        }
    }
}
