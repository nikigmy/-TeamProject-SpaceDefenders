using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

class Menu
{
    public static char FieldChar = ' ';
    public static char ShipChar = 'X';
    public static char YourShotChar = '*';
    public static char EnemyShotChar = '&';
    public static char EnemyChar = '%';
    public static char BorderChar = '#';
    public static int EnemyShoting = 0;
    public static int EnemyShotDelayInFrames = 5;
    // Progress in the game.
    public static int Level = 1;
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
    public static int shiprow = rows - 4;
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
    public static int LineEnemyShips = 2;
    // Lives of enemy ships.
    public static int EnemyHealth = 4;
    // Demage of your shot.
    public static int YourShotDmg = 1;
    // Demage of enemy shots.
    public static int EnemyShotDmg = 1;
    public static int ShipReset = 0;
    //Points at the end of the game.
    public static int Points = 0;

    // Structure that holds a cordinates of the shots.
    public struct shot
    {
        public int row;
        public int col;
    }

    // Records a shot.
    public static shot record;
    // A list to hold the cordinates of your shots.
    public static List<shot> YourShots = new List<shot>();
    // A list to hold the cordinates of the enemy shots.
    public static List<shot> EnemyShots = new List<shot>();
    // Shots that have hit a target.
    public static List<shot> AcurateShots = new List<shot>();
    public static Random EnemyShotPosition = new Random();

    // Structure for a enemy ship.
    public struct EnemyShipLevelOne_Two
    {
        public int row;
        public int col;
        public int Health;
    }

    // Array of structures that holds the cordinates of enemy ships level one.
    public static EnemyShipLevelOne_Two[] EnemyShipsLevelOne = new EnemyShipLevelOne_Two[(cols - 2) / 4 * LineEnemyShips];
    // Array of structures that holds the cordinates of enemy ships level two.
    public static EnemyShipLevelOne_Two[] EnemyShipsLevelTwo = new EnemyShipLevelOne_Two[(cols - 2) / 6 * LineEnemyShips];

    //************MAIN***************

    private const int FieldRows = 25;
    private const int FieldCols = 100;
    private const char arrowChar = '>';

    private static int musicPower = 10;
    private static int gameEffectsPower = 10;
    
    static Tuple<int, int> startGameCoordinates = new Tuple<int, int>(45, 12);
    static Tuple<int, int> settingsCoordinates = new Tuple<int,int>(46, 14);
    static Tuple<int, int> exitCoordinates = new Tuple<int,int>(48, 16);
    static Tuple<int, int> settingsHeaderCoordinates = new Tuple<int, int>(22, 2); 
    static Tuple<int, int> musicVolumeCoordinates = new Tuple<int, int>(30, 12);
    static Tuple<int, int> gameEffectsVolumeCoordinates = new Tuple<int, int>(23, 14);
    static Tuple<int, int> backCoordinates = new Tuple<int, int>(46, 17);
    static Tuple<int, int> musicLowCoordinates = new Tuple<int, int>(46, 12); 
    static Tuple<int, int> musicMediumCoordinates = new Tuple<int, int>(54, 12);
    static Tuple<int, int> musicHighCoordinates = new Tuple<int, int>(64, 12); 
    static Tuple<int, int> gameEffectsLowCoordinates = new Tuple<int, int>(46, 14);
    static Tuple<int, int> gameEffectsMediumCoordinates = new Tuple<int, int>(54, 14);
    static Tuple<int, int> gameEffectsHighCoordinates = new Tuple<int, int>(64, 14);

    static SoundPlayer menuSound = new SoundPlayer("enterSound.wav");
    static SoundPlayer menuSoundtrack = new SoundPlayer("menuMusic.wav");

    static void Main()
    {
        MainMenu();
    }

    static void Engine()
    {
        Transition();
        ResetField();
        Ship();
        Print();
        EnemyParameters();
        MainLoop();
        Ending();
    }

    private static void MainMenu()
    {
        Console.Title = "Space Defenders"; // Change title
        Console.SetWindowSize(FieldCols + 2, FieldRows + 3); // Set console width and height
        Console.SetBufferSize(FieldCols + 2, FieldRows + 3); // Setbuffer size
        Console.BackgroundColor = ConsoleColor.White; // Set background color
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Black; // Printing with white color
        Console.CursorVisible = false; // Hide curson

        
        //Load Sounds
        SoundPlayer startGameSound = new SoundPlayer("startGameSound.wav");
        using (startGameSound)
        {
            startGameSound.Load();
        }
        using (menuSound)
        {
            menuSound.Load();
        }
        using (menuSoundtrack)
        {
            menuSoundtrack.Load();
        }
        menuSoundtrack.PlayLooping();



        string logoLine = String.Empty; // Read logo
        List<string> logo = new List<string>();
        StreamReader logoReader = new StreamReader("logo-2.txt", Encoding.UTF8);
        using (logoReader)
        {
            logoLine = logoReader.ReadLine();
            while (logoLine != null)
            {
                logo.Add(logoLine);
                logoLine = logoReader.ReadLine();
            }
        }
        Console.WriteLine();
        for (int i = 0; i < logo.Count; i++)
        {
            Console.SetCursorPosition(0, i + 2);
            Console.Write(logo[i]);
        }

        string currentMenuOption = "Start Game";
        Console.SetCursorPosition(startGameCoordinates.Item1, startGameCoordinates.Item2);
        Console.WriteLine("Start Game");
        Console.SetCursorPosition(settingsCoordinates.Item1, settingsCoordinates.Item2);
        Console.WriteLine("Settings");
        Console.SetCursorPosition(exitCoordinates.Item1, exitCoordinates.Item2);
        Console.WriteLine("Exit");

        Console.SetCursorPosition(startGameCoordinates.Item1 - 1, startGameCoordinates.Item2);
        Console.Write(arrowChar);
        ConsoleKeyInfo presedKey = Console.ReadKey(true);
        while (presedKey.Key != ConsoleKey.Enter && presedKey.Key != ConsoleKey.Spacebar)
        {
            switch (presedKey.Key)
            {
                case ConsoleKey.DownArrow:
                    if (currentMenuOption.Equals("Start Game"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(startGameCoordinates.Item1 - 1, startGameCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Settings";
                    }
                    else if (currentMenuOption.Equals("Settings"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(exitCoordinates.Item1 - 1, exitCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Exit";
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (currentMenuOption.Equals("Exit"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(exitCoordinates.Item1 - 1, exitCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Settings";
                    }
                    else if (currentMenuOption.Equals("Settings"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(startGameCoordinates.Item1 - 1, startGameCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Start Game";
                    }
                    break;
                case ConsoleKey.Escape:
                    Console.SetCursorPosition(FieldCols - 1, FieldRows - 1);
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            presedKey = Console.ReadKey(true);
        }

        if (currentMenuOption.Equals("Start Game"))
        {
            menuSoundtrack.Stop();
            menuSoundtrack.Dispose();
            startGameSound.Play();
            Thread.Sleep(1500);
            Console.SetCursorPosition(0, 0);
            Engine();
        }
        else if (currentMenuOption.Equals("Settings"))
        {
            menuSoundtrack.Stop();
            menuSound.Play();
            Thread.Sleep(200);
            menuSoundtrack.PlayLooping();
            Settings("Music volume");
        }
        else if (currentMenuOption.Equals("Exit"))
        {
            menuSoundtrack.Stop();
            menuSoundtrack.Dispose();
            menuSound.Play();
            Thread.Sleep(300);
            menuSound.Dispose();
            Environment.Exit(0);
        }
    }

    private static void Settings(string currentSettingsOption)
    {
        Console.Clear();
        StreamReader settingsHeaderReader = new StreamReader("settingsHeader.txt");
        List<string> settingsHeader = new List<string>();
        using (settingsHeaderReader)
        {
            string currentLine = settingsHeaderReader.ReadLine();
            while (currentLine != null)
            {
                settingsHeader.Add(currentLine);
                currentLine = settingsHeaderReader.ReadLine();
            }
        }
        for (int i = 0; i < settingsHeader.Count; i++)
        {
            Console.SetCursorPosition(settingsHeaderCoordinates.Item1, settingsHeaderCoordinates.Item2 + i);
            Console.Write(settingsHeader[i]);
        }

        using (menuSound)
        {
            menuSound.Load();
        }
        using (menuSoundtrack)
        {
            menuSoundtrack.Load();
        }
        menuSoundtrack.PlayLooping();

        Console.SetCursorPosition(musicVolumeCoordinates.Item1, musicVolumeCoordinates.Item2);
        Console.Write("Music volume:   Low     Medium    High");
        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1, gameEffectsVolumeCoordinates.Item2);
        Console.Write("Game effects volume:   Low     Medium    High");
        Console.SetCursorPosition(backCoordinates.Item1, backCoordinates.Item2);
        Console.Write("Back");

        if (currentSettingsOption.Equals("Music volume"))
        {
            Console.SetCursorPosition(musicVolumeCoordinates.Item1 - 1, musicVolumeCoordinates.Item2);
        }
        else if (currentSettingsOption.Equals("Game effects volume"))
        {
            Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1, gameEffectsVolumeCoordinates.Item2);
        }
        Console.Write(arrowChar);
        ConsoleKeyInfo settingsPressedKey = Console.ReadKey(true);
        while (settingsPressedKey.Key != ConsoleKey.Enter && settingsPressedKey.Key != ConsoleKey.Spacebar)
        {
            switch (settingsPressedKey.Key)
            {
                case ConsoleKey.DownArrow:
                    if (currentSettingsOption.Equals("Music volume"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(musicVolumeCoordinates.Item1 - 1, musicVolumeCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Game effects volume";
                    }
                    else if (currentSettingsOption.Equals("Game effects volume"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(backCoordinates.Item1 - 1, backCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Back";
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (currentSettingsOption.Equals("Back"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(backCoordinates.Item1 - 1, backCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Game effects volume";
                    }
                    else if (currentSettingsOption.Equals("Game effects volume"))
                    {
                        menuSoundtrack.Stop();
                        menuSound.Play();
                        Thread.Sleep(200);
                        menuSoundtrack.PlayLooping();
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(musicVolumeCoordinates.Item1 - 1, musicVolumeCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Music volume";
                    }
                    break;
                default:
                    break;
            }

            settingsPressedKey = Console.ReadKey(true);
        }

        if (currentSettingsOption.Equals("Music volume"))
        {
            menuSoundtrack.Stop();
            menuSound.Play();
            Thread.Sleep(200);
            menuSoundtrack.PlayLooping();
            string currentMusicOption = "Low";
            Console.SetCursorPosition(musicLowCoordinates.Item1 - 1, musicLowCoordinates.Item2);
            Console.Write(arrowChar);
            ConsoleKeyInfo musicPressedKey = Console.ReadKey(true);
            while (musicPressedKey.Key != ConsoleKey.Enter && musicPressedKey.Key != ConsoleKey.Spacebar && musicPressedKey.Key != ConsoleKey.Escape)
            {
                switch (musicPressedKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (currentMusicOption.Equals("Low"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(musicLowCoordinates.Item1 - 1, musicLowCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "Medium";
                        }
                        else if (currentMusicOption.Equals("Medium"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicHighCoordinates.Item1 - 1, musicHighCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "High";
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentMusicOption.Equals("High"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(musicHighCoordinates.Item1 - 1, musicHighCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "Medium";
                        }
                        else if (currentMusicOption.Equals("Medium"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicLowCoordinates.Item1 - 1, musicLowCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "Low";
                        }
                        break;
                    case ConsoleKey.Escape:
                        Settings("Music volume");
                        break;
                    default:
                        break;
                }

                musicPressedKey = Console.ReadKey(true);
            }

            if (currentMusicOption.Equals("Low"))
            {
                musicPower = 10;
            }
            else if (currentMusicOption.Equals("Medium"))
            {
                musicPower = 30;
            }
            else if (currentMusicOption.Equals("High"))
            {
                musicPower = 50;
            }
            Settings(currentSettingsOption);
        }
        else if (currentSettingsOption.Equals("Game effects volume"))
        {
            menuSoundtrack.Stop();
            menuSound.Play();
            Thread.Sleep(200);
            menuSoundtrack.PlayLooping();
            string currentGameEffectsOption = "Low";
            Console.SetCursorPosition(gameEffectsLowCoordinates.Item1 - 1, gameEffectsLowCoordinates.Item2);
            Console.Write(arrowChar);
            ConsoleKeyInfo gameEffectsPressedKey = Console.ReadKey(true);
            while (gameEffectsPressedKey.Key != ConsoleKey.Enter && gameEffectsPressedKey.Key != ConsoleKey.Spacebar)
            {
                switch (gameEffectsPressedKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (currentGameEffectsOption.Equals("Low"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(gameEffectsLowCoordinates.Item1 - 1,
                                gameEffectsLowCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "Medium";
                        }
                        else if (currentGameEffectsOption.Equals("Medium"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsHighCoordinates.Item1 - 1,
                                gameEffectsHighCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "High";
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentGameEffectsOption.Equals("High"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(gameEffectsHighCoordinates.Item1 - 1,
                                gameEffectsHighCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "Medium";
                        }
                        else if (currentGameEffectsOption.Equals("Medium"))
                        {
                            menuSoundtrack.Stop();
                            menuSound.Play();
                            Thread.Sleep(200);
                            menuSoundtrack.PlayLooping();
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsLowCoordinates.Item1 - 1,
                                gameEffectsLowCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "Low";
                        }
                        break;
                    case ConsoleKey.Escape:
                        Settings("Game effects volume");
                        break;
                    default:
                        break;
                }

                gameEffectsPressedKey = Console.ReadKey(true);
            }

            if (currentGameEffectsOption.Equals("Low"))
            {
                gameEffectsPower = 10;
            }
            else if (currentGameEffectsOption.Equals("Medium"))
            {
                gameEffectsPower = 30;
            }
            else if (currentGameEffectsOption.Equals("High"))
            {
                gameEffectsPower = 50;
            }
            Settings(currentSettingsOption);
        }
        else if (currentSettingsOption.Equals("Back"))
        {
            menuSoundtrack.Stop();
            menuSound.Play();
            Thread.Sleep(200);
            menuSoundtrack.PlayLooping();
            MainMenu();
        }
    }

    public static void Transition()
    {
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
                    for (int j = EnemyShipsLevelOne[i].col - 1; j < EnemyShipsLevelOne[i].col + 2; j++)
                    {
                        record.row = EnemyShipsLevelOne[i].row - 1;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelOne[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelOne[i].Health > 0)
                            field[EnemyShipsLevelOne[i].row - 1, j] = EnemyChar;
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
                    for (int j = EnemyShipsLevelTwo[i].col - 1; j < EnemyShipsLevelTwo[i].col + 2; j++)
                    {
                        record.row = EnemyShipsLevelTwo[i].row - 1;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelTwo[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelTwo[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                    }
                    for (int j = EnemyShipsLevelTwo[i].col - 2; j < EnemyShipsLevelTwo[i].col + 3; j++)
                    {
                        record.row = EnemyShipsLevelTwo[i].row - 2;
                        record.col = j;
                        if (AcurateShots.Contains(record))
                            EnemyShipsLevelTwo[i].Health -= YourShotDmg;
                        if (EnemyShipsLevelTwo[i].Health > 0)
                            field[record.row, record.col] = EnemyChar;
                    }
                }
            }
            AcurateShots.Clear();
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
                    EnemyRow = EnemyRow + 3;
                    EnemyCol = 5;
                    EnemyShipsLevelTwo[i].row = EnemyRow;
                    EnemyShipsLevelTwo[i].col = EnemyCol;
                    EnemyShipsLevelTwo[i].Health = EnemyHealth;
                    EnemyCol += 6;
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
        if (Points < 0)
            Console.WriteLine("Very Noob");
        else if (Points >= 0 && Points < 200)
            Console.WriteLine("You Suck");
        else if (Points >= 200 && Points < 500)
            Console.WriteLine("Good");
        else if (Points >= 500)
            Console.WriteLine("WOW");
        Console.WriteLine(Console.LargestWindowHeight);
    }

    // Main loop.
    public static void MainLoop()
    {
        while (Playing)
        {
            DateTime check = DateTime.Now.AddMilliseconds(Delay);
            while (check > DateTime.Now)
            {
                if (Console.KeyAvailable)
                {
                    keyInfo = Console.ReadKey();
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
                    if (EnemyShipsLevelOne[i].Health == 0)
                        ShipReset++;
                }
                if (ShipReset == EnemyShipsLevelOne.Length)
                {
                    Level = 2;
                    EnemyParameters();
                    YourShots.Clear();
                    EnemyShots.Clear();
                }
            }
            ResetField();
            Borders();
            EnemiesToField();
            Ship();
            EnemyShotsToField();
            ShotsToField();
            if (EnemyShoting == EnemyShotDelayInFrames)
                EnemyShotRecord();
            else EnemyShoting++;
            Print();
            if (Health <= 0)
            {
                Lives--;
                Health = MaxHealth;
            }
            if (Lives <= 0)
                Playing = false;
            ShipReset = 0;
            MoveLeft = MoveRight = Shoot = true;
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

            record.row = EnemyShots[j].row;
            record.col = EnemyShots[j].col;
            Predicate<shot> FindIndex = new Predicate<shot>(ShotsDestroy);
            if (field[EnemyShots[j].row, EnemyShots[j].col] == BorderChar)
            {
                EnemyShots.RemoveAt(j);
            }
            else if (field[EnemyShots[j].row, EnemyShots[j].col] == ShipChar)
            {
                EnemyShots.RemoveAt(j);
                Health -= EnemyShotDmg;
            }
            else if (YourShots.Contains(record))
            {
                int Index = YourShots.FindIndex(FindIndex);
                EnemyShots.RemoveAt(j);
                YourShots.RemoveAt(Index);
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

    private static bool ShotsDestroy(shot a)
    {
        if (record.row == a.row && record.col == a.col)
            return true;
        return false;
    }


    // Enters the shots to the matrix.
    private static void ShotsToField()
    {
        for (int j = 0; j < YourShots.Count; j++)
        {
            record.row = YourShots[j].row;
            record.col = YourShots[j].col;
            Predicate<shot> FindIndex = new Predicate<shot>(ShotsDestroy);
            if (field[YourShots[j].row, YourShots[j].col] == BorderChar)
            {
                YourShots.RemoveAt(j);
            }
            else if (field[YourShots[j].row, YourShots[j].col] == EnemyChar)
            {
                AcurateShots.Add(YourShots[j]);
                YourShots.RemoveAt(j);
                Points++;
            }
            else if (EnemyShots.Contains(record))
            {
                int Index = EnemyShots.FindIndex(FindIndex);
                YourShots.RemoveAt(j);
                EnemyShots.RemoveAt(Index);
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
    ShipReposition:
        if (shiprow > rows - 4)
            shiprow = rows - 4;
        if (field[shiprow, ship] != FieldChar)
            shiprow++;
        field[shiprow, ship] = ShipChar;
        for (int i = ship - 1; i < ship + 2; i++)
        {
            if (field[shiprow + 1, i] != FieldChar)
            {
                ship--;
                goto ShipReposition;
            }
            else
                field[shiprow + 1, i] = ShipChar;
        }
        for (int i = ship - 2; i < ship + 3; i++)
        {
            if (field[shiprow + 2, i] != FieldChar)
            {
                ship--;
                goto ShipReposition;
            }
            field[shiprow + 2, i] = ShipChar;
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
        Console.WriteLine("Points: {0}   Health: {1}   Lives {2}", Points, Health, Lives);
    }

    // Records the shots.
    private static void ShotRecord()
    {
        record.row = shiprow - 1;
        record.col = ship;
        YourShots.Add(record);
    }
}

