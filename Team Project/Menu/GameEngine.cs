using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using GameSounds;

public class GameEngine
{
    #region Fields

    public static char FieldChar = ' ';
    public static char ShipChar = 'X';
    public static char YourShotChar = '*';
    public static char EnemyShotChar = '$';
    public static char BossChar = '\u25A0';
    public static char BossShotChar = '+';
    public static char EnemyChar = '%';
    public static char BorderChar = '#';
    public static char BonusChar = '@';
    public static int EnemyShoting = 0;
    public static int EnemyShotDelay = 5;
    public static int ShipLvl = 1;
    public static int BonusDelay = 0;
    // Progress in the game.
    public static int Level = 1;
    public static string BossDirecion = "Left";
    public static bool MoveLeft = true;
    public static bool MoveRight = true;
    public static bool Shoot = true;
    // State of the game.
    public static bool Playing = true;
    // Saves a keypress from the console.
    public static ConsoleKeyInfo keyInfo;
    // Delay for frame.
    public static int Delay = 100;
    //Rows of the field.
    public static int rows = 25;
    //Colomns of the field.
    public static int cols = 100;
    //Matrix for the field.
    public static char[,] field = new char[rows, cols];
    //Remembers on witch colomn is the top of the ship.
    public static int ship = cols / 2;
    // Max health of your ship.
    public static int MaxHealth = 10;
    // Health of your ship.
    public static int Health = MaxHealth;
    // Lives of your ship.
    public static int Lives = 5;
    // Lines of enemy ships.
    public static int LineEnemyShips = 1;
    // Lives of enemy ships.
    public static int EnemyHealth = 4;
    // Demage of your shot.
    public static int YourShotDmg = 1;
    // Demage of enemy shots.
    public static int EnemyShotDmg = 1;
    public static bool ShipReset = true;
    //Points at the end of the game.
    public static int Points = 0;
    public static Bonus BonusCords;
    // Structure that holds a cordinates of the shots.
    public struct Shot
    {
        public int row;
        public int col;
    }
    public struct Bonus
    {
        public int row;
        public int col;
        public bool OnBoard;
    }
    // Records a shot.
    public static Shot record;
    // A list to hold the cordinates of your shots.
    public static List<Shot> YourShots = new List<Shot>();
    // A list to hold the cordinates of the enemy shots.
    public static List<Shot> EnemyShots = new List<Shot>();
    // Shots that have hit a target.
    public static List<Shot> AcurateShots = new List<Shot>();
    public static Random EnemyShotPosition = new Random();

    // Structure for a enemy ship.
    public struct EnemyShipLevelOne_Two_Three_Boss
    {
        public int row;
        public int col;
        public int Health;
    }

    // Array of structures that holds the cordinates of enemy ships level one.
    public static EnemyShipLevelOne_Two_Three_Boss[] EnemyShipsLevelOne = new EnemyShipLevelOne_Two_Three_Boss[(cols - 2) / 4 * LineEnemyShips];
    // Array of structures that holds the cordinates of enemy ships level two.
    public static EnemyShipLevelOne_Two_Three_Boss[] EnemyShipsLevelTwo = new EnemyShipLevelOne_Two_Three_Boss[(cols - 2) / 6 * LineEnemyShips];
    // Array of structures that holds the cordinates of enemy ships level two.
    public static EnemyShipLevelOne_Two_Three_Boss[] EnemyShipsLevelThree = new EnemyShipLevelOne_Two_Three_Boss[(cols - 2) / 8 * LineEnemyShips];
    public static EnemyShipLevelOne_Two_Three_Boss Boss;

    private static Sounds sounds = new Sounds();

    #endregion

    #region MainEngine

    public static void MainEngine()
    {
        Transition();
        ResetField();
        Ship();
        Print();
        EnemyParameters();
        MainLoop();
        Ending();
    }

    #endregion

    public static void Transition()
    {
        sounds.PlayTransitionSound();
        if (Level == 1)
        {
            Console.Clear();
            StreamReader LevelOneReader = new StreamReader("Level 1.txt");
            List<string> LevelOneLogo = new List<string>();
            using (LevelOneReader)
            {
                string CurrentLine = LevelOneReader.ReadLine();
                while (CurrentLine != null)
                {
                    LevelOneLogo.Add(CurrentLine);
                    CurrentLine = LevelOneReader.ReadLine();
                }
            }
            for (int i = 0; i < LevelOneLogo.Count; i++)
            {
                Console.SetCursorPosition(22, i + 8);
                Console.Write(LevelOneLogo[i]);
            }
            Thread.Sleep(3000);
        }
        else if (Level == 2)
        {
            Console.Clear();
            StreamReader LevelTwoReader = new StreamReader("Level 2.txt");
            List<string> LevelTwoLogo = new List<string>();
            using (LevelTwoReader)
            {
                string CurrentLine = LevelTwoReader.ReadLine();
                while (CurrentLine != null)
                {
                    LevelTwoLogo.Add(CurrentLine);
                    CurrentLine = LevelTwoReader.ReadLine();
                }
            }
            for (int i = 0; i < LevelTwoLogo.Count; i++)
            {
                Console.SetCursorPosition(22, i + 8);
                Console.Write(LevelTwoLogo[i]);
            }
            Thread.Sleep(3000);
        }
        else if (Level == 3)
        {
            Console.Clear();
            StreamReader LevelThreeReader = new StreamReader("Level 3.txt");
            List<string> LevelThreeLogo = new List<string>();
            using (LevelThreeReader)
            {
                string CurrentLine = LevelThreeReader.ReadLine();
                while (CurrentLine != null)
                {
                    LevelThreeLogo.Add(CurrentLine);
                    CurrentLine = LevelThreeReader.ReadLine();
                }
            }
            for (int i = 0; i < LevelThreeLogo.Count; i++)
            {
                Console.SetCursorPosition(22, i + 8);
                Console.Write(LevelThreeLogo[i]);
            }
            Thread.Sleep(3000);
        }
        else if (Level == 4)
        {
            Console.Clear();
            StreamReader BossLevelReader = new StreamReader("Boss.txt");
            List<string> BossLevelLogo = new List<string>();
            using (BossLevelReader)
            {
                string CurrentLine = BossLevelReader.ReadLine();
                while (CurrentLine != null)
                {
                    BossLevelLogo.Add(CurrentLine);
                    CurrentLine = BossLevelReader.ReadLine();
                }
            }
            for (int i = 0; i < BossLevelLogo.Count; i++)
            {
                Console.SetCursorPosition(32, i + 8);
                Console.Write(BossLevelLogo[i]);
            }
            Thread.Sleep(3000);
        }
    }

    // Enters the enemy ships to the field.
    public static void EnemiesToField()
    {
        if (Level == 1)
        {
            for (int i = 0; i < EnemyShipsLevelOne.Length; i++)
            {
                if (EnemyShipsLevelOne[i].Health > 0)
                {
                    record.row = EnemyShipsLevelOne[i].row;
                    record.col = EnemyShipsLevelOne[i].col;
                    if (AcurateShots.Contains(record))
                        EnemyShipsLevelOne[i].Health -= YourShotDmg;
                    if (EnemyShipsLevelOne[i].Health > 0)
                        field[EnemyShipsLevelOne[i].row, EnemyShipsLevelOne[i].col] = EnemyChar;
                    else sounds.PlayExplosionSound();
                    for (int j = EnemyShipsLevelOne[i].col - 1; j < EnemyShipsLevelOne[i].col + 2; j++)
                    {
                        record.row = EnemyShipsLevelOne[i].row - 1;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelOne[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelOne[i].Health > 0)
                            field[EnemyShipsLevelOne[i].row - 1, j] = EnemyChar;
                        else sounds.PlayExplosionSound();
                    }
                }
            }
            AcurateShots.Clear();
        }
        if (Level == 2)
        {
            for (int i = 0; i < EnemyShipsLevelTwo.Length; i++)
            {
                if (EnemyShipsLevelTwo[i].Health > 0)
                {
                    record.row = EnemyShipsLevelTwo[i].row;
                    record.col = EnemyShipsLevelTwo[i].col;
                    if (AcurateShots.Contains(record))
                        EnemyShipsLevelTwo[i].Health -= YourShotDmg;
                    if (EnemyShipsLevelTwo[i].Health > 0)
                        field[EnemyShipsLevelTwo[i].row, EnemyShipsLevelTwo[i].col] = EnemyChar;
                    else sounds.PlayExplosionSound();
                    for (int j = EnemyShipsLevelTwo[i].col - 1; j < EnemyShipsLevelTwo[i].col + 2; j++)
                    {
                        record.row = EnemyShipsLevelTwo[i].row - 1;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelTwo[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelTwo[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                        else sounds.PlayExplosionSound();
                    }
                    for (int j = EnemyShipsLevelTwo[i].col - 2; j < EnemyShipsLevelTwo[i].col + 3; j++)
                    {
                        record.row = EnemyShipsLevelTwo[i].row - 2;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelTwo[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelTwo[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                        else sounds.PlayExplosionSound();
                    }
                }
            }
            AcurateShots.Clear();
        }
        else if (Level == 3)
        {
            for (int i = 0; i < EnemyShipsLevelThree.Length; i++)
            {
                if (EnemyShipsLevelThree[i].Health > 0)
                {
                    record.row = EnemyShipsLevelThree[i].row;
                    record.col = EnemyShipsLevelThree[i].col;
                    if (AcurateShots.Contains(record))
                        EnemyShipsLevelThree[i].Health -= YourShotDmg;
                    if (EnemyShipsLevelThree[i].Health > 0)
                        field[EnemyShipsLevelThree[i].row, EnemyShipsLevelThree[i].col] = EnemyChar;
                    else sounds.PlayExplosionSound();
                    for (int j = EnemyShipsLevelThree[i].col - 1; j < EnemyShipsLevelThree[i].col + 2; j++)
                    {
                        record.row = EnemyShipsLevelThree[i].row - 1;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelThree[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelThree[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                        else sounds.PlayExplosionSound();
                    }
                    for (int j = EnemyShipsLevelThree[i].col - 2; j < EnemyShipsLevelThree[i].col + 3; j++)
                    {
                        record.row = EnemyShipsLevelThree[i].row - 2;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelThree[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelThree[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                        else sounds.PlayExplosionSound();
                    }
                    for (int j = EnemyShipsLevelThree[i].col - 3; j < EnemyShipsLevelThree[i].col + 4; j++)
                    {
                        record.row = EnemyShipsLevelThree[i].row - 3;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelThree[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelThree[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                        else sounds.PlayExplosionSound();
                    }
                }
            }
            AcurateShots.Clear();
        }
        else if (Level == 4)
        {
            if (Boss.Health > 0)
            {
                //1
                field[Boss.row, Boss.col - 1] = BossChar;
                field[Boss.row, Boss.col - 2] = BossChar;
                field[Boss.row, Boss.col + 1] = BossChar;
                field[Boss.row, Boss.col + 2] = BossChar;
                //2
                field[Boss.row - 1, Boss.col - 3] = BossChar;
                field[Boss.row - 1, Boss.col - 5] = BossChar;
                field[Boss.row - 1, Boss.col + 3] = BossChar;
                field[Boss.row - 1, Boss.col + 5] = BossChar;
                //3
                field[Boss.row - 2, Boss.col] = BossChar;
                field[Boss.row - 2, Boss.col - 1] = BossChar;
                field[Boss.row - 2, Boss.col + 1] = BossChar;
                field[Boss.row - 2, Boss.col - 3] = BossChar;
                field[Boss.row - 2, Boss.col + 3] = BossChar;
                field[Boss.row - 2, Boss.col - 5] = BossChar;
                field[Boss.row - 2, Boss.col + 5] = BossChar;
                //4
                for (int i = Boss.col - 5; i < Boss.col + 6; i++)
                {
                    field[Boss.row - 3, i] = BossChar;
                }
                //5
                field[Boss.row - 4, Boss.col] = BossChar;
                field[Boss.row - 4, Boss.col - 1] = BossChar;
                field[Boss.row - 4, Boss.col + 1] = BossChar;
                field[Boss.row - 4, Boss.col - 3] = BossChar;
                field[Boss.row - 4, Boss.col + 3] = BossChar;
                field[Boss.row - 4, Boss.col - 4] = BossChar;
                field[Boss.row - 4, Boss.col + 4] = BossChar;
                field[Boss.row - 4, Boss.col] = BossChar;
                //6
                for (int i = Boss.col - 3; i < Boss.col + 4; i++)
                {
                    field[Boss.row - 5, i] = BossChar;
                }
                //7
                field[Boss.row - 6, Boss.col + 2] = BossChar;
                field[Boss.row - 6, Boss.col - 2] = BossChar;
                //8
                field[Boss.row - 7, Boss.col - 3] = BossChar;
                field[Boss.row - 7, Boss.col + 3] = BossChar;
            }
        }
    }

    // Enters the parameters of the enemy ships.
    public static void EnemyParameters()
    {
        if (Level == 1)
        {
            int EnemyCol = 3;
            int EnemyRow = 3;
            for (int i = 0; i < EnemyShipsLevelOne.Length; i++)
            {
                if (EnemyCol < cols - 3)
                {
                    EnemyShipsLevelOne[i].row = EnemyRow;
                    EnemyShipsLevelOne[i].col = EnemyCol;
                    EnemyShipsLevelOne[i].Health = EnemyHealth;
                    EnemyCol += 4;
                }
                else
                {
                    EnemyRow = EnemyRow + 3;
                    EnemyCol = 3;
                    EnemyShipsLevelOne[i].row = EnemyRow;
                    EnemyShipsLevelOne[i].col = EnemyCol;
                    EnemyShipsLevelOne[i].Health = EnemyHealth;
                    EnemyCol += 4;
                }

            }
        }
        else if (Level == 2)
        {
            int EnemyCol = 5;
            int EnemyRow = 4;
            EnemyShotDmg = 2;
            for (int i = 0; i < EnemyShipsLevelTwo.Length; i++)
            {
                if (EnemyCol < cols - 4)
                {
                    EnemyShipsLevelTwo[i].row = EnemyRow;
                    EnemyShipsLevelTwo[i].col = EnemyCol;
                    EnemyShipsLevelTwo[i].Health = EnemyHealth;
                    EnemyCol += 6;
                }
                else
                {
                    EnemyRow = EnemyRow + 4;
                    EnemyCol = 5;
                    EnemyShipsLevelTwo[i].row = EnemyRow;
                    EnemyShipsLevelTwo[i].col = EnemyCol;
                    EnemyShipsLevelTwo[i].Health = EnemyHealth;
                    EnemyCol += 6;
                }

            }
        }
        else if (Level == 3)
        {
            int EnemyCol = 5;
            int EnemyRow = 5;
            EnemyShotDmg = 2;
            for (int i = 0; i < EnemyShipsLevelThree.Length; i++)
            {
                if (EnemyCol < cols - 5)
                {
                    EnemyShipsLevelThree[i].row = EnemyRow;
                    EnemyShipsLevelThree[i].col = EnemyCol;
                    EnemyShipsLevelThree[i].Health = EnemyHealth;
                    EnemyCol += 8;
                }
                else
                {
                    EnemyRow = EnemyRow + 5;
                    EnemyCol = 5;
                    EnemyShipsLevelThree[i].row = EnemyRow;
                    EnemyShipsLevelThree[i].col = EnemyCol;
                    EnemyShipsLevelThree[i].Health = EnemyHealth;
                    EnemyCol += 8;
                }

            }
        }
    }

    // End of the game.
    public static void Ending()
    {
        Console.Clear();
        Console.WriteLine("XxX Game Over XxX ");
        Console.WriteLine("Your points are {0}", Points);
        if (Level == 4 && Boss.Health < 0)
            Console.WriteLine("Good job, you beated the game!!!");
        else Console.WriteLine("Try again next time.");
        Thread.Sleep(3000);
    }

    // Main loop.
    public static void MainLoop()
    {
        Boss.Health = 200;
        Boss.row = 9;
        Boss.col = 50;
        while (Playing)
        {
            DateTime check = DateTime.Now.AddMilliseconds(Delay);
            while (check > DateTime.Now)
            {
                if (Console.KeyAvailable)
                {
                    keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (Shoot == true)
                            {
                                ShotRecord();
                                Shoot = false;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (MoveLeft == true)
                            {
                                if (ship != 3)
                                    ship--;
                                MoveLeft = false;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (MoveRight == true)
                            {
                                if (ship != cols - 4)
                                    ship++;
                                MoveRight = false;
                            }
                            break;
                        case ConsoleKey.Escape:
                            Playing = false;
                            break;
                        case ConsoleKey.P:
                            Pause();
                            break;
                        default:
                            break;
                    }
                }
            }
            if (Level == 1)
            {
                for (int i = 0; i < EnemyShipsLevelOne.Length; i++)
                {
                    if (EnemyShipsLevelOne[i].Health > 0)
                        ShipReset = false;
                }
                if (ShipReset == true)
                {
                    Level = 2;
                    Lives++;
                    //LineEnemyShips++;
                    EnemyHealth *= 2;
                    EnemyShotDmg *= 2;
                    EnemyParameters();
                    YourShots.Clear();
                    EnemyShots.Clear();
                    Transition();
                }
            }
            else if (Level == 2)
            {
                for (int i = 0; i < EnemyShipsLevelTwo.Length; i++)
                {
                    if (EnemyShipsLevelTwo[i].Health > 0)
                        ShipReset = false;
                }
                if (ShipReset == true)
                {
                    Level = 3;
                    Lives++;
                    EnemyHealth *= 2;
                    EnemyShotDmg *= 2;
                    EnemyParameters();
                    YourShots.Clear();
                    EnemyShots.Clear();
                    Transition();
                    BonusCords.row = 1;
                    BonusCords.col = EnemyShipsLevelThree[EnemyShipsLevelThree.Length / 2].col + 3;
                    BonusCords.OnBoard = true;
                }
            }
            else if (Level == 3)
            {
                for (int i = 0; i < EnemyShipsLevelThree.Length; i++)
                {
                    if (EnemyShipsLevelThree[i].Health > 0)
                        ShipReset = false;
                }
                if (ShipReset == true)
                {
                    Level = 4;
                    Lives++;
                    EnemyParameters();
                    YourShots.Clear();
                    EnemyShots.Clear();
                    Transition();
                }
            }
            else if (Level == 4)
            {
                if (Boss.Health <= 0)
                    Playing = false;
                BossMovement();
                //EnemyShotRecord();
            }
            ResetField();
            Borders();
            EnemiesToField();
            Ship();
            EnemyShotsToField();
            YourShotsToField();
            if (EnemyShoting == EnemyShotDelay)
                EnemyShotRecord();
            else EnemyShoting++;
            if (BonusCords.OnBoard == true)
                BonusToField();
            Print();
            if (Health <= 0)
            {
                Lives--;
                Health = MaxHealth;
            }
            if (Lives <= 0)
                Playing = false;
            ShipReset = true;
            MoveLeft = MoveRight = Shoot = true;
        }
    }

    private static void BonusToField()
    {
        if (BonusDelay == 3)
        {
            if (field[BonusCords.row + 1, BonusCords.col] == BorderChar)
                BonusCords.OnBoard = false;
            else if (field[BonusCords.row + 1, BonusCords.col] == FieldChar)
                field[BonusCords.row, BonusCords.col] = BonusChar;
            else if (field[BonusCords.row + 1, BonusCords.col] == ShipChar)
            {
                ShipLvl++;
                BonusCords.OnBoard = false;
            }
            BonusCords.row++;
            BonusDelay = 0;
        }
        else
        {
            BonusDelay++;
            field[BonusCords.row, BonusCords.col] = BonusChar;
        }
    }

    public static void Pause()
    {
        while (true)
        {
            DateTime PauseCheck = DateTime.Now.AddMilliseconds(50);
            while (PauseCheck > DateTime.Now)
            {
                ConsoleKeyInfo PauseKey;
                if (Console.KeyAvailable)
                {
                    PauseKey = Console.ReadKey();
                    if (PauseKey.Key == ConsoleKey.P)
                        return;
                }

            }
        }
    }

    // Records an enemy shot.
    public static void EnemyShotRecord()
    {
        if (Level == 1)
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
                if (EnemyShipsLevelOne[EnemyShipShoting].Health > 0)
                {
                    record.row = EnemyShipsLevelOne[EnemyShipShoting].row + 1;
                    record.col = EnemyShipsLevelOne[EnemyShipShoting].col;
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
        if (Level == 2)
        {
            List<int> DeadRows = new List<int>();
        NoShipsAlive:
            if (DeadRows.Count == (cols - 2) / 6 - 1)
                goto NoShipsAliveReally;
            int EnemyShotPositionRecord = EnemyShotPosition.Next(0, (cols - 2) / 6);
            if (DeadRows.Contains(EnemyShotPositionRecord))
                goto NoShipsAlive;
            int EnemyShipShoting = EnemyShotPositionRecord + (LineEnemyShips - 1) * ((cols - 2) / 6);
            while (true)
            {
                if (EnemyShipsLevelTwo[EnemyShipShoting].Health > 0)
                {
                    record.row = EnemyShipsLevelTwo[EnemyShipShoting].row + 1;
                    record.col = EnemyShipsLevelTwo[EnemyShipShoting].col;
                    EnemyShots.Add(record);
                    record.row = EnemyShipsLevelTwo[EnemyShipShoting].row;
                    record.col = EnemyShipsLevelTwo[EnemyShipShoting].col - 1;
                    EnemyShots.Add(record);
                    record.row = EnemyShipsLevelTwo[EnemyShipShoting].row;
                    record.col = EnemyShipsLevelTwo[EnemyShipShoting].col + 1;
                    EnemyShots.Add(record);
                    break;
                }
                else
                {
                    EnemyShipShoting -= (cols - 2) / 6;
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
        else if (Level == 3)
        {
            List<int> DeadRows = new List<int>();
        NoShipsAlive:
            if (DeadRows.Count == (cols - 2) / 8 - 1)
                goto NoShipsAliveReally;
            int EnemyShotPositionRecord = EnemyShotPosition.Next(0, (cols - 2) / 8);
            if (DeadRows.Contains(EnemyShotPositionRecord))
                goto NoShipsAlive;
            int EnemyShipShoting = EnemyShotPositionRecord + (LineEnemyShips - 1) * ((cols - 2) / 8);
            while (true)
            {
                if (EnemyShipsLevelThree[EnemyShipShoting].Health > 0)
                {
                    record.row = EnemyShipsLevelThree[EnemyShipShoting].row + 1;
                    record.col = EnemyShipsLevelThree[EnemyShipShoting].col;
                    EnemyShots.Add(record);
                    record.row = EnemyShipsLevelThree[EnemyShipShoting].row;
                    record.col = EnemyShipsLevelThree[EnemyShipShoting].col + 1;
                    EnemyShots.Add(record);
                    record.row = EnemyShipsLevelThree[EnemyShipShoting].row;
                    record.col = EnemyShipsLevelThree[EnemyShipShoting].col - 1;
                    EnemyShots.Add(record);
                    record.row = EnemyShipsLevelThree[EnemyShipShoting].row - 1;
                    record.col = EnemyShipsLevelThree[EnemyShipShoting].col - 2;
                    EnemyShots.Add(record);
                    record.row = EnemyShipsLevelThree[EnemyShipShoting].row - 1;
                    record.col = EnemyShipsLevelThree[EnemyShipShoting].col + 2;
                    EnemyShots.Add(record);
                    break;
                }
                else
                {
                    EnemyShipShoting -= (cols - 2) / 8;
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
        else if (Level == 4)
        {
            record.row = Boss.row;
            record.col = Boss.col - 3;
            EnemyShots.Add(record);

            record.row = Boss.row;
            record.col = Boss.col + 3;
            EnemyShots.Add(record);

            record.row = Boss.row;
            record.col = Boss.col - 5;
            EnemyShots.Add(record);

            record.row = Boss.row;
            record.col = Boss.col + 5;
            EnemyShots.Add(record);

            EnemyShoting = 0;
        }
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
        if (Level != 4)
        {
            for (int i = 0; i < EnemyShots.Count; i++)
            {

                record.row = EnemyShots[i].row;
                record.col = EnemyShots[i].col;
                Predicate<Shot> FindIndex = new Predicate<Shot>(ShotsDestroy);
                if (field[EnemyShots[i].row, EnemyShots[i].col] == BorderChar)
                {
                    EnemyShots.RemoveAt(i);
                    i--;
                }
                else if (field[EnemyShots[i].row, EnemyShots[i].col] == ShipChar)
                {
                    EnemyShots.RemoveAt(i);
                    i--;
                    Health -= EnemyShotDmg;
                }
                else if (YourShots.Contains(record))
                {
                    int Index = YourShots.FindIndex(FindIndex);
                    EnemyShots.RemoveAt(i);
                    i--;
                    YourShots.RemoveAt(Index);
                }
                else
                {
                    field[EnemyShots[i].row, EnemyShots[i].col] = EnemyShotChar;
                    record.col = EnemyShots[i].col;
                    record.row = EnemyShots[i].row + 1;
                    EnemyShots.RemoveAt(i);
                    EnemyShots.Insert(i, record);
                }

            }
        }
        else
        {
            for (int i = 0; i < EnemyShots.Count; i++)
            {
                record.row = EnemyShots[i].row;
                record.col = EnemyShots[i].col;
                Predicate<Shot> FindIndex = new Predicate<Shot>(ShotsDestroy);
                if (field[EnemyShots[i].row, EnemyShots[i].col] == BorderChar)
                {
                    EnemyShots.RemoveAt(i);
                    i--;
                }
                else if (field[EnemyShots[i].row, EnemyShots[i].col] == ShipChar)
                {
                    EnemyShots.RemoveAt(i);
                    i--;
                    Health -= EnemyShotDmg;
                }
                else if (YourShots.Contains(record))
                {
                    int Index = YourShots.FindIndex(FindIndex);
                    i--;
                    YourShots.RemoveAt(Index);
                }
                else
                {
                    field[EnemyShots[i].row, EnemyShots[i].col] = BossShotChar;
                    record.col = EnemyShots[i].col;
                    record.row = EnemyShots[i].row + 1;
                    EnemyShots.RemoveAt(i);
                    EnemyShots.Insert(i, record);
                }
            }
        }
    }

    private static bool ShotsDestroy(Shot a)
    {
        if (record.row == a.row && record.col == a.col)
            return true;
        return false;
    }


    // Enters the shots to the matrix.
    private static void YourShotsToField()
    {
        for (int i = 0; i < YourShots.Count; i++)
        {
            record.row = YourShots[i].row;
            record.col = YourShots[i].col;
            Predicate<Shot> FindIndex = new Predicate<Shot>(ShotsDestroy);
            if (field[YourShots[i].row, YourShots[i].col] == BorderChar)
            {
                YourShots.RemoveAt(i);
                i--;
            }
            else if (field[YourShots[i].row, YourShots[i].col] == BossChar)
            {
                YourShots.RemoveAt(i);
                i--;
                Boss.Health -= YourShotDmg;
                Points += 2;
                sounds.PlayExplosionSound();
            }
            else if (field[YourShots[i].row, YourShots[i].col] == EnemyChar)
            {
                AcurateShots.Add(YourShots[i]);
                YourShots.RemoveAt(i);
                i--;
                Points++;
            }
            else if (EnemyShots.Contains(record))
            {
                if (Level != 4)
                {
                    int Index = EnemyShots.FindIndex(FindIndex);
                    YourShots.RemoveAt(i);
                    EnemyShots.RemoveAt(Index);
                    i--;
                }
                else
                {
                    YourShots.RemoveAt(i);
                    i--;
                }
            }
            else
            {
                field[YourShots[i].row, YourShots[i].col] = YourShotChar;
                record.col = YourShots[i].col;
                record.row = YourShots[i].row - 1;
                YourShots.RemoveAt(i);
                YourShots.Insert(i, record);
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
        string print = ""; ;
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
        if (Level != 4)
        Console.WriteLine("Points: {0}   Health: {1}   Lives {2}", Points, Health, Lives);
        else Console.WriteLine("Points: {0}   Health: {1}   Lives {2} Boss Heath: {3}", Points, Health, Lives, Boss.Health);
    }

    // Records the shots. 
    private static void ShotRecord()
    {
        if (ShipLvl == 1)
        {
            record.row = rows - 5;
            record.col = ship;
            YourShots.Add(record);
            sounds.PlayShotSound();
        }
        else if (ShipLvl == 2)
        {
            record.row = rows - 5;
            record.col = ship;
            YourShots.Add(record);
            record.row = rows - 4;
            record.col = ship - 1;
            YourShots.Add(record);
            record.row = rows - 4;
            record.col = ship + 1;
            YourShots.Add(record);
            sounds.PlayShotSound();
        }
    }

    private static void BossMovement()
    {
        if (BossDirecion == "Left" && Boss.col < cols - 7)
            Boss.col++;
        else
            BossDirecion = "Right";
        if (BossDirecion == "Right" && Boss.col > 7)
            Boss.col--;
        else
            BossDirecion = "Left";
    }
}

